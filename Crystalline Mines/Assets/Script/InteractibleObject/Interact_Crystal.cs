using UnityEngine;

public class Interact_Crystal : Interactible
{
    private SimonGame simonGame;
    private void Awake()
    {
        simonGame = GameObject.Find("Controller").GetComponent<SimonGame>();
        if (simonGame is null)
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
        simonGame.PlayerInteractCristal(gameObject); //mettre verif pour si on est dans la salle su simon (dans script simonGame)
    }
}
