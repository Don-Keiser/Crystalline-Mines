using UnityEngine;
public class Interact_TutoKey : Interactible
{
    [SerializeField] private DoorHandler.LevelRoom _doorRoom;
    public override void PlayerInteract()
    {
        base.PlayerInteract();
       TimerManager.StartTimer(0.25f, () => DoorHandler.Instance.GetDoor(_doorRoom).OpenDoor(() => true));
    }
    public override void StartAnim()
    {
        base.StartAnim();
        Destroy(gameObject);
    }

    public override void StartSFXAndVFX()
    {
        base.StartSFXAndVFX();
    }
}
