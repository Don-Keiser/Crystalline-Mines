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
    [HideInInspector] public bool AnimTIme = false;
    [SerializeField] private float _animDuration = 3f;
    private Vector3 _startPos;
    private float _startFOV;
    private float _initialFOV;
    private float _elapsedTime = 0f;
    private bool _backToPlayer = true;

    [Header("DEBUG")]
    [SerializeField] private Transform _levelCenter;
    [SerializeField] private float _maxDezoom;

    private Camera _cam;
    private Vector3 _velocity = Vector3.zero;

    private void Awake()
    {
        _cam = GetComponent<Camera>();
        _startFOV = _cam.orthographicSize;
        _initialFOV = _startFOV;
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
        if (_player == null || AnimTIme) return;

        Vector3 targetPosition = new Vector3(_player.position.x + _offset.x, _player.position.y + _offset.y, transform.position.z);

        float clampedX = Mathf.Clamp(targetPosition.x, _minX, _maxX);
        float clampedY = Mathf.Clamp(targetPosition.y, _minY, _maxY);

        Vector3 clampedTarget = new Vector3(clampedX, clampedY, targetPosition.z);

        transform.position = Vector3.SmoothDamp(transform.position, clampedTarget, ref _velocity, _smoothSpeed);
    }

    [ContextMenu("StartCentering")] //debug
    public void StartCenteringAnimation()
    {
        _backToPlayer = false;
        StartCameraAnimation(_levelCenter.position, _maxDezoom);
    }

    private void StartCameraAnimation(Vector3 targetPos, float targetZoom)
    {
        _startPos = _cam.transform.position;
        _startFOV = _cam.orthographicSize;
        _elapsedTime = 0f;
        AnimTIme = true;
    }

    public void AnimateCamera()
    {
        _elapsedTime += Time.deltaTime;

        float t = _elapsedTime / _animDuration;
        t = Mathf.Clamp01(t);

        Vector3 camPos = Vector3.Lerp(_startPos, (_backToPlayer ? _player.position : _levelCenter.position), t);
        _cam.transform.position = camPos;

        _cam.orthographicSize = Mathf.Lerp(_startFOV, _backToPlayer ? _initialFOV : _maxDezoom, t);

        if (t >= 1f)
        {
            if (!_backToPlayer)
            {
                _backToPlayer = true;
                StartCameraAnimation(_player.position, _initialFOV); //set a timer before restarting the animation
            }
            else
            {
                AnimTIme = false;
            }
        }
    }
}
