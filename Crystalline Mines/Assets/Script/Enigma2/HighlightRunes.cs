using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighlightRunes : Interactible
{
    [SerializeField] private float _timerDuration;
    [SerializeField] private GameObject _rune;
    private SpriteRenderer _runeColor;

    private void Start()
    {
        _runeColor = _rune.GetComponent<SpriteRenderer>();
    }

    public void Highlight()
    {
        if (Crystals.crystalsColor[0].color == Color.white) // Check if lights already lit up
        {
        
            _rune.SetActive(true);
            Crystals.ChangeColor(_runeColor.color); // transform the color of the crystals to the color of the rune

            TimerManager.StartTimer(_timerDuration, () =>
            {
                _rune.SetActive(false);
                Crystals.ChangeColor(Color.white); // transform the color of the crystals to white
            });
        }
    }
    
    public override void PlayerInteract()
    {
        base.PlayerInteract();
        Highlight();
        
    }
}
