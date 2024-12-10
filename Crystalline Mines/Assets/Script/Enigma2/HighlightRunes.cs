using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighlightRunes : Interactible
{
    [SerializeField] private Crystals crystals;
    [SerializeField] private float _timerDuration;
    [SerializeField] private GameObject _rune;
    private SpriteRenderer _runeColor;

    private void Start()
    {
        _runeColor = _rune.GetComponent<SpriteRenderer>();
    }

    public void Highlight()
    {
        if (crystals.crystalsSpriteRenderers[0].color == Color.white) // Check if lights already lit up
        {
            _rune.SetActive(true);
            crystals.ChangeColor(_runeColor.color); // transform the color of the crystals to the color of the rune

            TimerManager.StartTimer(_timerDuration, () =>
            {
                _rune.SetActive(false);
                crystals.ChangeColor(Color.white); // transform the color of the crystals to white
            });
        }
    }
    
    public override void PlayerInteract()
    {
        base.PlayerInteract();
        Highlight();
        
    }
}
