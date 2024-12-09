using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer), typeof(BoxCollider2D))]
public class RailPieces : Interactible
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
    Transform _railPiecesParent; // Will be use when drop down

    // Local
    SpriteRenderer _spriteRenderer;
    bool _isCarried;

    #endregion

    #region Methods

    #region Unity methods

    void Start()
    {
        _playerTransform = Player.PlayerTransform;
        _spriteRenderer = GetComponent<SpriteRenderer>();

        _railPiecesFormHandler = RailPiecesFormHandler.Instance;

        _railPiecesForm = _railPiecesFormHandler.GetRailFormValues(railPiecesFormType);
        _railPiecesParent = _railPiecesFormHandler.railPiecesParent;

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

        UpdatePosition();
    }

    void UpdatePosition()
    {
        if (_isCarried)
        {
            transform.position = new Vector3(
                _playerTransform.position.x,
                _playerTransform.position.y + _playerTransform.localScale.y / 2 + transform.localScale.y / 2,
                _playerTransform.position.z
            );

            transform.parent = _playerTransform;
        }
        else
        {
            Debug.LogWarning("WARNING ! Not implemented yet.");
            // TODO: Launch object (use Egnima1 code)
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
    #endregion
}