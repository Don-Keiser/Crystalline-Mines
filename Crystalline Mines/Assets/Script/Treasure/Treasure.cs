using Script.Enigma1;
using System;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer), typeof(BoxCollider2D))]
public class Treasure : Interactible, ICarriable
{
    public static Action OnPlayerPickupTreasureEvent;

    bool _isCarried;

    int _initialLayer;
    Transform _initialParent;

    Transform _transform;
    Transform _playerTransform;

    private PlayerGrabController _playerGrabController;
    void Start()
    {
        _initialLayer = gameObject.layer;
        _initialParent = transform.parent;

        _transform = gameObject.transform;
        _playerTransform = Player.PlayerTransform;
        _playerGrabController = PlayerGrabController.Instance;
    }

    public override void PlayerInteract()
    {
        SetIsCarried(!_isCarried);

        StartAnim();
        StartSFXAndVFX();

        OnPlayerPickupTreasureEvent?.Invoke();
    }

    public override void StartAnim()
    {
        
    }

    public override void StartSFXAndVFX()
    {
        
    }

    void SetIsCarried(bool p_newValue)
    {
        if (p_newValue && _isCarried == false && _playerGrabController.holdObject == null)
        {
            _playerGrabController.holdObject = gameObject;
            _isCarried = true;
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
        
    }

    void UpdatePosition()
    {
        if (_isCarried)
        {
            GetCarriedByPlayer();
        }
        else
        {
            GetThrowedByPlayer();

            Debug.LogWarning("WARNING ! Not implemented yet.");
            // TODO: Launch object (use Egnima1 code)  
        }
    }

    void GetCarriedByPlayer()
    {
        _transform.position = new Vector3(
            _playerTransform.position.x,
            _playerTransform.position.y + _playerTransform.localScale.y / 2 + _transform.localScale.y / 2,
            _playerTransform.position.z
        );

        _transform.parent = _playerTransform;
    }

    void GetThrowedByPlayer()
    {
        _transform.parent = _initialParent;
    }

    public void Reinitialize()
    {
        GetCarriedByPlayer();
    }
}
