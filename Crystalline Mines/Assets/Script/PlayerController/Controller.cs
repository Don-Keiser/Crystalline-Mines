using Script.Enigma1;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;


public class Controller : MonoBehaviour
{
    [SerializeField] private Player _player;
    private PlayerGrabController _grabController;
    [SerializeField] private CameraController _camera;

    [Header("Player Interaction Range")]
    [SerializeField] private float _rangeRadius;
    [SerializeField] private LayerMask _interactibleMask;

    [Header("Show Text on nearest interactible object")]
    [SerializeField] private GameObject _interactibleText;
    private bool _textIsActive;

    [Header("Pause Menu")]
    [SerializeField] private GameObject _pauseMenu;

    public Vector2 playerInput { get; private set; }


    private void Awake()
    {
        _grabController = _player.GetComponent<PlayerGrabController>();
        if (_grabController is null) { Debug.LogError("Player has not PlayerGrabController script"); }
    }
    private void Update()
    {
        AnimationManager.Instance.SetAnimationBool(); //a modifier

        if (_pauseMenu.activeSelf || !_camera.FinishAnim) { return; }

        _player.SetMoveInput(playerInput);
        ShowTextOnNearestObject();
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


    public void Move(InputAction.CallbackContext context)
    {
        playerInput = context.ReadValue<Vector2>();

        if (context.canceled)
        {
            //anim stand
            _player.SetMoveInput(new Vector2(0,0));
        }
    }
    public void JumpInput(InputAction.CallbackContext context)
    {
        _player.Jump();
        //anim jump
    }
    public void CrouchInput(InputAction.CallbackContext context)
    {
        _player.DropThroughPlatform(-1);
        //anim fall
    }
    public void InteractInput(InputAction.CallbackContext context)
    {
        GameObject nearestObject = GetNearestInteractableObject();
        if (nearestObject is not null)
        {
            Interactible interactible = nearestObject.GetComponent<Interactible>();
            if (interactible is not null)
            {
                interactible.PlayerInteract();
                return;
            }
        }
        if (_grabController is not null && _grabController.hasCrystal)
        {
            _grabController.DropObject();
        }
    }
}
