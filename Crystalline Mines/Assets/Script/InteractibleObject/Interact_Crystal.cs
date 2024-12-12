using Script.Enigma1;
using UnityEngine;

public class Interact_Crystal : Interactible
{
    private SimonGame _simonGame;
    [SerializeField] private PlayerGrabController _grabController;
    private Rigidbody2D _holdObjectRb;
    private void Awake()
    {
        _simonGame = FindObjectOfType<SimonGame>();
        if (_simonGame is null)
        {
            Debug.LogError("the object Controller not found on scene");
        }
        //_grabController = PlayerGrabController.Instance;
    }
    public override void PlayerInteract()
    {
        base.PlayerInteract();
        InteractForSimonGame();
        TryGrabObject();
    }

    private void InteractForSimonGame()
    {
        _simonGame.PlayerInteractCristal(gameObject); //check if you are in the simonGame room (in the simonGame script)
    }

    private void TryGrabObject() //notice and update GrabController Info
    {
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
}
