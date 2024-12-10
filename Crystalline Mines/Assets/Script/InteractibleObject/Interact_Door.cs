using UnityEngine;

public class Interact_Door : Interactible
{
    [SerializeField] private LayerMask _defaultMask;
    public override void PlayerInteract()
    {
        if(Player.CanOpenTheDoor)
        {
            base.PlayerInteract();
            Player.CanOpenTheDoor = false;
        }
        else { print("you don't have the key !"); }
    }

    public override void StartAnim()
    {
        base.StartAnim();
        gameObject.transform.parent.transform.position += new Vector3(0, 4, 0);
        TimerManager.StartTimer(1.5f, () => Destroy(transform.parent.gameObject));
    }
}
