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

    [Header("Anim on interact")]
    [SerializeField] private float _fullScreenDuration;
    [SerializeField] private float _animationDuration;
    [SerializeField] private float _maxDezoom;
    [SerializeField] private Vector3 _cameraCenter = new Vector3(60, 9, -10);

    private void Start()
    {
        _runeColor = _rune.GetComponent<SpriteRenderer>();
    }

    public void Highlight()
    {
        if (crystals.crystalsSpriteRenderers[0].color == Color.white) // Check if lights already lit up
        {
            _rune.SetActive(true);
            EventManager.StartCameraAnimation(_cameraCenter, _maxDezoom, _fullScreenDuration,_animationDuration);
            crystals.ChangeColor(_runeColor.color); // transform the color of the crystals to the color of the rune

            TimerManager.StartTimer((_timerDuration + _fullScreenDuration + (2 * _animationDuration)), () =>
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
