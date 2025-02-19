using System.Collections;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Camera Bound")]
    [SerializeField] private float _maxX;
    [SerializeField] private float _minX;
    [Space(5)]
    [SerializeField] private float _maxY;
    [SerializeField] private float _minY;

    [Header("Target")]
    [SerializeField] private Transform _player;

    [Header("Camera Settings")]
    [SerializeField] private Vector2 _offset;
    [SerializeField] private float _smoothSpeed;

    [Header("Camera Animation Settings")]
    [SerializeField] private float _animDuration = 2f;
    [SerializeField] private float _fullScreenDuration = 2f;
    private Vector3 _startPos;
    private float _startFOV;
    private float _initialFOV;
    private float _elapsedTime = 0f;
    private Vector3 _targetPosition;
    private Vector3 _levelCenter;
    private float _targetZoom;

    private Camera _cam;
    private Vector3 _velocity = Vector3.zero;

    public bool IsAnimating { get; private set; }
    public bool FinishAnim { get; private set; }
    public bool FullSreenTime { get; private set; }

    [Header("Camera Shake")]
    private float _skakeMagnitude;
    private float _shakeDuration;

    private void OnEnable()
    {
        EventManager.CameraCinematic += GoToMapCenter; // Subscribe event
        EventManager.OnStartedCameraShake += StartCameraShake;

        _cam = GetComponent<Camera>();
        _startFOV = _cam.orthographicSize;
        _initialFOV = _startFOV;
        FinishAnim = true;
    }
    private void OnDisable()
    {
        EventManager.CameraCinematic -= GoToMapCenter; // Subscribe event
        EventManager.OnStartedCameraShake -= StartCameraShake;
    }

    public void InitializeCameraBoundary(float maxX, float minX, float maxY, float minY)
    {
        _maxX = maxX;
        _minX = minX;
        _maxY = maxY;
        _minY = minY;
    }

    public void SmoothFollowWithBounds()
    {
        if (_player == null || !FinishAnim) return;

        Vector3 targetPosition = new Vector3(_player.position.x + _offset.x, _player.position.y + _offset.y, transform.position.z);

        float clampedX = Mathf.Clamp(targetPosition.x, _minX, _maxX);
        float clampedY = Mathf.Clamp(targetPosition.y, _minY, _maxY);

        Vector3 clampedTarget = new Vector3(clampedX, clampedY, targetPosition.z);

        transform.position = Vector3.SmoothDamp(transform.position, clampedTarget, ref _velocity, _smoothSpeed);
    }

    private void StartAnimation(Vector3 targetPos, float targetZoom)
    {
        IsAnimating = true;
        _elapsedTime = 0f;

        _startPos = _cam.transform.position;
        _startFOV = _cam.orthographicSize;

        _targetPosition = targetPos;
        _targetZoom = targetZoom;
    }

    public void GoToMapCenter(Vector3 mapCenter, float zoomOutLevel, float fullScreenDuration, float animDuration)
    {
        FinishAnim = false;
        _levelCenter = mapCenter;
        _fullScreenDuration = fullScreenDuration;
        _animDuration = animDuration;
        StartAnimation(mapCenter, zoomOutLevel);
    }

    public void ReturnToPlayer()
    {
        StartAnimation(_player.position, _initialFOV);
        FullSreenTime = false;
    }

    public void AnimateCamera()
    {
        if (!IsAnimating) return;

        _elapsedTime += Time.deltaTime;

        float t = Mathf.Clamp01(_elapsedTime / _animDuration);

        Vector3 camPos = Vector3.Lerp(_startPos, _targetPosition, t);
        _cam.transform.position = camPos;

        _cam.orthographicSize = Mathf.Lerp(_startFOV, _targetZoom, t);

        if (t >= 1f)
        {
            IsAnimating = false;

            if (_targetPosition == _levelCenter)
            {
                FullSreenTime = true;
                TimerManager.StartTimer(_fullScreenDuration, ReturnToPlayer);
            }
            else if (_targetPosition == _player.position)
            {
                FinishAnim = true;
                Player.CameraAnimationTime = false;
            }
        }
    }

    public void StartCameraShake(float duration, float magnitude = 1)
    {
        _shakeDuration = duration;
        _skakeMagnitude = magnitude;
        StartCoroutine(CameraShake(duration, magnitude));   
    }
    private IEnumerator CameraShake(float duration, float magnitude)
    {
        float timer = 0;
        while (timer < duration)
        {
            yield return new WaitForEndOfFrame();
            Vector3 _camPos = _cam.transform.position;

            float _camRotationZ = Random.Range(-1.0f, 1.0f) * magnitude;

            _cam.transform.rotation = Quaternion.Euler(0, 0, _camRotationZ);

            timer += Time.deltaTime;
            yield return null;
        }
        _cam.transform.rotation = Quaternion.Euler(0, 0, 0);
    }
}
