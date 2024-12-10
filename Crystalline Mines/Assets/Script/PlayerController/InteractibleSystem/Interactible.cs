using UnityEngine;

public abstract class Interactible : MonoBehaviour, IInteractible
{
    public virtual void PlayerInteract() //common behaviour of all interactive objects
    {
        print($"Interact with {name}");
        StartAnim();
        StartSFXAndVFX();
    }

    public virtual void StartAnim()
    {
        print("Start animation");
    }
    public virtual void StartSFXAndVFX()
    {
        print("start vfx and sfx");
    }
}
