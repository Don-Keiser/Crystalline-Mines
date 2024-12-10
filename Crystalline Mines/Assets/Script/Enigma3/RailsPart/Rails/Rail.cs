using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer), typeof(BoxCollider2D))]
public class Rail : Interactible
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

    #endregion

    #region Methods

    void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _initialRailState = railState;

        _railFormHandler = RailFormHandler.Instance;

        _railForm = _railFormHandler.GetRailFormValues(railState, missingRailPieces);
        _railFormSpritesOnGround = _railFormHandler.railSpritesOnGround;

        UpdateSprite();
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
            }
            else
            {
                // TODO: Show the little bubble with the correct sprite
            }
        }
        else
        {
            // TODO: Show the little bubble with the correct sprite
        }
    }

    public override void StartSFXAndVFX()
    {
        
    }
    #endregion

    public void Reinitialize()
    {
        gameObject.SetActive(true);

        SetRailState(_initialRailState);
    }

    void SetRailState(RailFormHandler.RailStates p_newRailState)
    {
        railState = p_newRailState;

        UpdateSprite();
    }

    bool DoesPlayerCarryRailPieces(out RailPieces p_railPieces)
    {
        p_railPieces = null;

        if (Player.CarriedObject != null)
        {
            if (Player.CarriedObject.TryGetComponent(out p_railPieces))
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
                Debug.LogError($"ERROR ! The given railState '{railState}' is not planned in the switch.");
                break;
        }
    }
    #endregion
}