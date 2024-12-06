using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Interact_TutoPanel : Interactible
{
    public Sprite InfoImage;
    public GameObject InfoPanel;
    private bool _isActive;

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
        InfoPanel.SetActive(_isActive);
        InfoPanel.transform.GetChild(0).GetComponent<Image>().sprite = InfoImage;
    }
}
