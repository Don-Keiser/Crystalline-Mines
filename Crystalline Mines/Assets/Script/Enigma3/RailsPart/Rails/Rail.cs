using Script.Enigma1;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer), typeof(BoxCollider2D))]
public class Rail : Interactible, IRail
{
    #region Variables

    // - Public variables - //

    [field: Header("Rail statistics :")]
    [field: SerializeField]
    public RailFormHandler.RailStates railState { get; private set; }
    public RailPiecesFormHandler.RailPiecesFormTypes missingRailPieces;

    // - Private variables - //

    // From RailFormHandler
    RailFormHandler _railFormHandler;
    RailFormHandler.RailForm _railForm;
    RailFormHandler.RailSpritesOnGround _railFormSpritesOnGround;

    // Local
    SpriteRenderer _spriteRenderer;
    RailFormHandler.RailStates _initialRailState;

    [Tooltip("Contains the layer Interactible and maybe others")]
    int _initialLayer;

    #endregion

    #region Methods

    void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _initialRailState = railState;
        _initialLayer = gameObject.layer;

        _railFormHandler = RailFormHandler.Instance;

        if (railState == RailFormHandler.RailStates.Damaged)
            _railForm = _railFormHandler.GetRailFormValues(railState, missingRailPieces);

        _railFormSpritesOnGround = _railFormHandler.railSpritesOnGround;

        UpdateSprite();
        UpdateLayer();
    }

    #region Interactible methods

    public override void PlayerInteract()
    {
        if (DoesPlayerCarryRailPieces(out RailPieces railPieces))
        {
            if (DoesRailPiecesCanRepairMe(railPieces))
            {
                // The rail will not have any layers, it can't be interact again, and the in-game tooltip will not appeared
                gameObject.layer = 0;

                railPieces.Disable();

                SetRailState(RailFormHandler.RailStates.NonDamaged);

                RailManager.onNewReparedRailEvent?.Invoke(this);

                PlayerGrabController.Instance.hasCrystal = false;
                PlayerGrabController.Instance.holdObject = null;
                PlayerGrabController.Instance.holdObjectRb = null;
            }
            else
            {
                // Show the little bubble with the correct sprite, and do not throw the carried object
                RailManager.onShowDetailedDamagedRailEvent?.Invoke(_railForm.spriteShownWhenInteracted);
            }
        }
        else
        {
            // Show the little bubble with the correct sprite
            RailManager.onShowDetailedDamagedRailEvent?.Invoke(_railForm.spriteShownWhenInteracted);
        }
    }

    public override void StartSFXAndVFX()
    {
        
    }
    #endregion

    #region IRail methods

    public void Reinitialize()
    {
        gameObject.SetActive(true);

        SetRailState(_initialRailState);
    }
    #endregion

    void SetRailState(RailFormHandler.RailStates p_newRailState)
    {
        railState = p_newRailState;

        if (railState != _initialRailState && railState == RailFormHandler.RailStates.NonDamaged)
            RailManager.Instance.IsAllDamagedRailRepared();

        UpdateSprite();
        UpdateLayer();
    }

    bool DoesPlayerCarryRailPieces(out RailPieces p_railPieces)
    {
        p_railPieces = null;

        if (PlayerGrabController.Instance.holdObject != null)
        {
            if (PlayerGrabController.Instance.holdObject.TryGetComponent(out p_railPieces))
            {
                return true;
            }
        }

        return false;
    }

    bool DoesRailPiecesCanRepairMe(RailPieces p_railPieces)
    {
        if (p_railPieces.railPiecesFormType == missingRailPieces)
            return true;

        return false;
    }

    void UpdateSprite()
    {
        switch (railState)
        {
            case RailFormHandler.RailStates.NonDamaged:
                _spriteRenderer.sprite = _railFormSpritesOnGround.NonDamaged;
                break;

            case RailFormHandler.RailStates.Damaged:
                _spriteRenderer.sprite = _railFormSpritesOnGround.Damaged;
                break;

            default:
                Debug.LogError($"ERROR ! The railState '{railState}' is not planned in the switch.");
                break;
        }
    }

    void UpdateLayer()
    {
        switch (railState)
        {
            case RailFormHandler.RailStates.NonDamaged:
                gameObject.layer = 0;
                break;

            case RailFormHandler.RailStates.Damaged:
                gameObject.layer = _initialLayer;
                break;

            default:
                Debug.LogError($"ERROR ! The railState '{railState}' is not planned in the switch.");
                break;
        }
    }
    #endregion
}