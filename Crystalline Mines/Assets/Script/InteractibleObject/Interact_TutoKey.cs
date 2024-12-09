public class Interact_TutoKey : Interactible
{

    public override void PlayerInteract()
    {
        Player.TutorialKeyObtained = true;
        base.PlayerInteract();
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
