using UnityEngine;

public class Interact_TutoPanel : Interactible
{
    [Header("Anim scale")]
    [SerializeField] private Vector3 _animScale = new Vector3(0.5f, 0.5f, 0.5f);

    [Header("Explanory text")]
    public string ExplanatoryText;
    public Sprite ExplanatoryImage;

    public override void PlayerInteract()
    {
        base.PlayerInteract();
        EventManager.StartActiveTutoPanel(gameObject);
    }
    public override void StartAnim()
    {
        base.StartAnim();
        gameObject.transform.localScale += _animScale;
        TimerManager.StartTimer(0.15f, () => gameObject.transform.localScale -= _animScale);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        EventManager.StartDisableTutoPanel(gameObject);
    }
}
