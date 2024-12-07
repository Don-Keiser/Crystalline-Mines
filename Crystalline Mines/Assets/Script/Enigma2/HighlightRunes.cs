using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighlightRunes : Interactible
{
    [SerializeField] GameObject _rune;
    [SerializeField] private float _timerDuration;
    
    public void Highlight()
    {
        _rune.SetActive(true);
        
        TimerManager.StartTimer(_timerDuration, () =>
        {
            _rune.SetActive(false);
        });
        // TODO: change the colour of the decorative crystals to match the rune
        Crystals.crystals.ForEach(crystal =>
        {
            crystal.GetComponent<SpriteRenderer>().color = _rune.GetComponent<SpriteRenderer>().color;
        });
        
        
    }

    public override void PlayerInteract()
    {
        base.PlayerInteract();
        Highlight();
    }
}
