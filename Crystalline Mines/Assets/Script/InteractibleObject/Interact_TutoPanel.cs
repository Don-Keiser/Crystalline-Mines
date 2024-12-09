using TMPro;
using UnityEngine;

public class Interact_TutoPanel : Interactible
{
    private bool _isActive;
    [Header("Explanory text")]
    [SerializeField] private string _explanatoryText;

    public void ActivePanel()
    {
        _isActive = true;
    }
    public void DesactivatePanel()
    {
        _isActive = false;
    }
    public override void PlayerInteract()
    {
        base.PlayerInteract();

        _isActive = !_isActive;
        gameObject.SetActive(_isActive);
        gameObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = _explanatoryText;
    }
}
