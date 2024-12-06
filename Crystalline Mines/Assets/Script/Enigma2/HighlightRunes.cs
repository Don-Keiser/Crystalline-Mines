using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighlightRunes : Interactible
{
    [SerializeField] GameObject _rune;
    [SerializeField] float _timerDuration;
    
    public void Highlight()
    {
        _rune.SetActive(true);
        
        // TODO: Play Timer()
        // TODO: change the colour of the decorative crystals to match the rune
    }

    public override void PlayerInteract()
    {
        base.PlayerInteract();
        Highlight();
    }
}
