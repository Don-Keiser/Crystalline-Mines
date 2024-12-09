using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer), typeof(BoxCollider2D))]
public class Rail : Interactible
{
    #region Variables

    // - Public variables - //

    [Header("Rail statistics :")]
    public RailFormHandler.RailStates railState;
    public RailPiecesFormHandler.RailPiecesFormTypes missingRailPieces;

    // - Private variables - //

    // From RailFormHandler
    RailFormHandler _railFormHandler;
    RailFormHandler.RailForm _railForm;
    RailFormHandler.RailSpritesOnGround _railFormSpritesOnGround;

    // Local
    SpriteRenderer _spriteRenderer;

    #endregion

    #region Methods

    void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();

        _railFormHandler = RailFormHandler.Instance;

        _railForm = _railFormHandler.GetRailFormValues(railState, missingRailPieces);
        _railFormSpritesOnGround = _railFormHandler.railSpritesOnGround;

        UpdateSprite();
    }

    public override void PlayerInteract()
    {
        base.PlayerInteract();
    }

    public override void StartSFXAndVFX()
    {
        
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
                Debug.LogError($"ERROR ! The given railState {railState} is not planned in the switch.");
                break;
        }
    }
    #endregion
}