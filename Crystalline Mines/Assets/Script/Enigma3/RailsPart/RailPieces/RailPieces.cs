using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer), typeof(BoxCollider2D))]
public class RailPieces : Interactible
{
    #region Variables

    // Public variables

    [Header("Statistics :")]
    public RailPiecesFormHandler.RailPiecesFormTypes railPiecesFormType;

    // Private variables

    RailPiecesFormHandler.RailPiecesForm _railPiecesForm;

    bool _isCarried;

    SpriteRenderer _spriteRenderer;

    #endregion

    #region Methods

    #region Unity methods

    void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();

        _railPiecesForm = RailPiecesFormHandler.Instance.GetRailFormSprites(railPiecesFormType);

        UpdateSprite();
    }
    #endregion

    public override void PlayerInteract()
    {
        StartSFXAndVFX();

        SetIsCarried(!_isCarried);
    }

    public override void StartSFXAndVFX()
    {

    }

    void SetIsCarried(bool p_newValue)
    {
        _isCarried = p_newValue;

        UpdateSprite();
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
    #endregion
}