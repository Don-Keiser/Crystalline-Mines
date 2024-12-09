using UnityEngine;
public class Interact_TutoKey : Interactible
{
    private bool _playerCarriesKey = false;
    private void OnEnable()
    {
        EventManager.OnGetTutoKey += PlayerGetKey;
    }
    public override void PlayerInteract()
    {
        base.PlayerInteract();
        _playerCarriesKey = true;
    }

    private void PlayerGetKey()
    {
        _playerCarriesKey = true;
    }
}
