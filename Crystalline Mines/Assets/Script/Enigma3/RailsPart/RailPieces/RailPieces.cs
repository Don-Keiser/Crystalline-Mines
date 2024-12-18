using Script.Enigma1;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer), typeof(BoxCollider2D), typeof(Rigidbody2D))]
public class RailPieces : Interactible, IRail
{
    #region Variables

    // - Public variables - //

    [Header("Statistics :")]
    public RailPiecesFormHandler.RailPiecesFormTypes railPiecesFormType;

    // - Private variables - //

    // Form Player
    Transform _playerTransform;

    // From RailPiecesFormHandler
    RailPiecesFormHandler _railPiecesFormHandler;
    RailPiecesFormHandler.RailPiecesForm _railPiecesForm;

    // From RailManager
    Transform _railPiecesParent; // Will be use when drop down

    // From PlayerGrabController
    private PlayerGrabController _playerGrabController;

    // Local
    Transform _transform;
    Transform _initialParent;
    int _initialLayer;
    Vector3 _initialPosition;
    SpriteRenderer _spriteRenderer;
    bool _isCarried;

    #endregion

    #region Methods

    #region Unity methods

    void Start()
    {
        _transform = transform;
        _initialParent = _transform.parent;
        _initialLayer = gameObject.layer;
        _initialPosition = _transform.position;
        _spriteRenderer = GetComponent<SpriteRenderer>();

        _playerTransform = Player.PlayerTransform;

        _railPiecesFormHandler = RailPiecesFormHandler.Instance;

        _railPiecesForm = _railPiecesFormHandler.GetRailFormValues(railPiecesFormType);
        _railPiecesParent = RailManager.Instance.railPiecesParent;

        _playerGrabController = PlayerGrabController.Instance;

        UpdateSprite();

        RailManager.onAllRailsRepairedEvent += () => gameObject.layer = 0;
    }
    #endregion

    #region Interactible methods

    public override void PlayerInteract()
    {
        SetIsCarried(!_isCarried);

        StartSFXAndVFX();
    }

    public override void StartSFXAndVFX()
    {

    }
    #endregion

    public void Disable()
    {
        _isCarried = false;

        if (_playerGrabController.holdObject == gameObject)
            _playerGrabController.holdObject = null;

        _transform.parent = _railPiecesParent;

        gameObject.SetActive(false);
    }

    #region IRail methods

    public void Reinitialize()
    {
        gameObject.SetActive(true);

        _transform.parent = _railPiecesParent;
        _transform.position = _initialPosition;

        if (_playerGrabController.holdObject == gameObject)
            _playerGrabController.holdObject = null;

        _isCarried = false;

        UpdateLayer();
        UpdateSprite();
    }
    #endregion

    public void SetIsCarried(bool p_newValue)
    {
        if (p_newValue && _isCarried == false && _playerGrabController.holdObject == null)
        {
            _playerGrabController.holdObject = gameObject;
            _isCarried = true;

            _playerGrabController.holdObject = gameObject;
            _playerGrabController.hasCrystal = true;

            _playerGrabController.holdObjectRb = GetComponent<Rigidbody2D>();
            _playerGrabController.holdObjectRb.isKinematic = true;
        }

        else if (p_newValue == false && _isCarried && _playerGrabController.holdObject == gameObject)
        {
            _playerGrabController.holdObject = null;
            _isCarried = false;
        }

        UpdateLayer();
        UpdateSprite();
        UpdatePosition();
    }

    void UpdateLayer()
    {
        if (_isCarried)
        {
            gameObject.layer = 0;
        }
        else
        {
            gameObject.layer = _initialLayer;
        }
    }

    void UpdateSprite()
    {
        if (_isCarried)
        {
            _spriteRenderer.sprite = _railPiecesForm.spriteWhenCarried;
        }
        else
        {
            _spriteRenderer.sprite = _railPiecesForm.spriteOnGround;
        }
    }

    void UpdatePosition()
    {
        if (_isCarried)
        {
            _transform.position = new Vector3(
                _playerTransform.position.x,
                _playerTransform.position.y + _playerTransform.localScale.y / 2 + _transform.localScale.y / 2,
                _playerTransform.position.z
            );

            _transform.parent = _playerTransform;
        }
        else
        {
            transform.parent = _initialParent;
        }
    }
    #endregion
}