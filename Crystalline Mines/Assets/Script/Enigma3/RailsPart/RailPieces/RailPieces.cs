using UnityEngine;

[RequireComponent(typeof(SpriteRenderer), typeof(BoxCollider2D))]
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

    // Local
    Transform _transform;
    Vector3 _initialPosition;
    SpriteRenderer _spriteRenderer;
    bool _isCarried;

    #endregion

    #region Methods

    #region Unity methods

    void Start()
    {
        _transform = transform;
        _initialPosition = _transform.position;
        _spriteRenderer = GetComponent<SpriteRenderer>();

        _playerTransform = Player.PlayerTransform;

        _railPiecesFormHandler = RailPiecesFormHandler.Instance;

        _railPiecesForm = _railPiecesFormHandler.GetRailFormValues(railPiecesFormType);
        _railPiecesParent = RailManager.Instance.railPiecesParent;

        UpdateSprite();
    }
    #endregion

    #region Interactible methods

    public override void PlayerInteract()
    {
        StartSFXAndVFX();

        SetIsCarried(!_isCarried);
    }

    public override void StartSFXAndVFX()
    {

    }
    #endregion

    public void Disable()
    {
        _isCarried = false;

        _transform.parent = _railPiecesParent;

        gameObject.SetActive(false);
    }

    #region IRail methods

    public void Reinitialize()
    {
        gameObject.SetActive(true);

        _transform.parent = _railPiecesParent;
        _transform.position = _initialPosition;

        _isCarried = false;

        UpdateSprite();
    }
    #endregion

    void SetIsCarried(bool p_newValue)
    {
        _isCarried = p_newValue;

        if (_isCarried && Player.CarriedObject == null)
            Player.CarriedObject = gameObject;

        UpdateSprite();

        UpdatePosition();
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
            Debug.LogWarning("WARNING ! Not implemented yet.");
            // TODO: Launch object (use Egnima1 code)
        }
    }
    #endregion
}