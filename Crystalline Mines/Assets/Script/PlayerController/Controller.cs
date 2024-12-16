using Script.Enigma1;
using System.Linq;
using UnityEngine;


public class Controller : MonoBehaviour
{
    [SerializeField] private Player _player;
    private PlayerGrabController _grabController;
    [SerializeField] private CameraController _camera;

    [Header("Player Interaction Range")]
    [SerializeField] private float _rangeRadius;
    [SerializeField] private LayerMask _interactibleMask;

    [Header("TESTCamera")]
    [SerializeField] private float _maxDezoom;
    [SerializeField] private Vector3 _levelCenter;

    [Header("Show Text on nearest interactible object")]
    [SerializeField] private GameObject _interactibleText;
    private bool _textIsActive;

    private void Awake()
    {
        _grabController = _player.GetComponent<PlayerGrabController>();
        if(_grabController is null) { Debug.LogError("Player has not PlayerGrabController script"); }
    }
    private void Update()
    {
        if (!_camera.FinishAnim) { return; }

        Vector2 _moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        _player.SetMoveInput(_moveInput);

        ShowTextOnNearestObject();
        Animation.Instance.MoveAnimation();
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _player.Jump();
        }
        if (Input.GetKeyDown(KeyCode.LeftShift) | Input.GetKeyDown(KeyCode.S))
        {
            _player.DropThroughPlatform(-1);
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            GameObject nearestObject = GetNearestInteractableObject();
            if (nearestObject != null)
            {
                Interactible interactible = nearestObject.GetComponent<Interactible>();
                if (interactible != null)
                {
                    interactible.PlayerInteract();
                    return;
                }
            }
            if(_grabController != null && _grabController.hasCrystal)
            {
                _grabController.DropObject();
            }
        }
    }
    public void ShowTextOnNearestObject()
    {
        GameObject nearestObject = GetNearestInteractableObject();

        if (nearestObject is null || (nearestObject is null && _textIsActive))
        {
            _textIsActive = false;
            _interactibleText.SetActive(false);
            return;
        }

        _textIsActive = true;
        _interactibleText.transform.position = nearestObject.transform.position + new Vector3(0, nearestObject.transform.localScale.y, 0);
        _interactibleText.SetActive(true);
    }

    private void LateUpdate()
    {
        _camera.SmoothFollowWithBounds();

        if (_camera.IsAnimating) { _camera.AnimateCamera(); }
    }

    private GameObject GetNearestInteractableObject()
    {
        RaycastHit2D[] allHits = Physics2D.CircleCastAll(_player.transform.position, _rangeRadius, Vector2.zero, 0f, _interactibleMask);
        if (allHits.Length == 0) { return null; }

        GameObject nearestObject = allHits.OrderBy(hit => (hit.transform.position - _player.transform.position).sqrMagnitude).FirstOrDefault().collider.gameObject;
        return nearestObject;
    }
}
