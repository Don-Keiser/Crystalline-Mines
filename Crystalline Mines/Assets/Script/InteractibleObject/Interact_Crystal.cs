using Script.Enigma1;
using UnityEngine;

public class Interact_Crystal : Interactible, ICarriable
{
    [SerializeField] SimonGame _simonGame;
    [SerializeField] PlayerGrabController _grabController;

    Vector3 _initialPosition;

    void Start()
    {
        _initialPosition = transform.position;
    }

    public override void PlayerInteract()
    {
        base.PlayerInteract();
        InteractForSimonGame();
        TryGrabObject();
    }

    private void InteractForSimonGame()
    {
        if(_simonGame is null) { return; }

        _simonGame.PlayerInteractCristal(gameObject); //check if you are in the simonGame room (in the simonGame script)
    }

    private void TryGrabObject() //notice and update GrabController Info
    {
        if(_grabController == null)
            return;

        if (_grabController.holdObject != null)
        {
            _grabController.DropObject();
        }

        if (GetComponent<Rigidbody2D>() != null && !_grabController.hasCrystal)
        {
            _grabController.holdObject = gameObject;
            _grabController.hasCrystal = true;
            gameObject.layer = 0;

            _grabController.holdObjectRb = GetComponent<Rigidbody2D>();
            _grabController.holdObjectRb.isKinematic = true;
        }
    }

    public void Reinitialize()
    {
        transform.position = _initialPosition;
    }
}