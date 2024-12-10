using UnityEngine;

public class Interact_Crystal : Interactible
{
    private SimonGame _simonGame;
    private void Awake()
    {
        _simonGame = GameObject.Find("Manager").GetComponent<SimonGame>();
        if (_simonGame is null)
        {
            Debug.LogError("the object Controller not found on scene");
        }
    }
    public override void PlayerInteract()
    {
        base.PlayerInteract();
        InteractForSimonGame();
    }

    private void InteractForSimonGame()
    {
        _simonGame.PlayerInteractCristal(gameObject); //check if you are in the simonGame room (in the simonGame script)
    }
}
