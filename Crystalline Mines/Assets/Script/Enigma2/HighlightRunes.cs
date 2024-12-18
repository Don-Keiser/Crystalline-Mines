using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Serialization;

public class HighlightRunes : Interactible
{
    [SerializeField] private Crystals _crystals; // list of room crystals
    [SerializeField] private float _timerDuration; // time during which the rune is displayed
    [SerializeField] private GameObject _rune;
    private SpriteRenderer _runeSprite;
    private Light2D _runeLight;

    [Header("Anim on interact")]
    [SerializeField] private float _fullScreenDuration;
    [SerializeField] private float _animationDuration;
    [SerializeField] private float _maxDezoom;
    [SerializeField] private GameObject _cameraCenter;

    private void Start()
    {
        _runeSprite = _rune.GetComponent<SpriteRenderer>();
        _runeLight = _rune.GetComponent<Light2D>();
    }

    public void Highlight()
    {
        if (_crystals.crystalsSpriteRenderers[0].color == Color.white) // Check if lights already lit up
        {
            _runeLight.enabled = true;
            _rune.SetActive(true);
            _runeSprite.enabled = false; // hide the rune
            EventManager.StartCameraAnimation(_cameraCenter.transform.position, _maxDezoom, _fullScreenDuration,_animationDuration);
            
            // display the rune
            TimerManager.StartTimer((_fullScreenDuration + (2 * _animationDuration)), () =>
            {
                _runeLight.enabled = false;
                _runeSprite.enabled = true;
            });
            
            _crystals.ChangeColor(_runeSprite.color, true, _animationDuration); // transform the color of the crystals to the color of the rune

            TimerManager.StartTimer((_timerDuration + _fullScreenDuration + (2 * _animationDuration)), () =>
            {
                _rune.SetActive(false);
                _crystals.ChangeColor(Color.white, false, _animationDuration); // transform the color of the crystals to white
            });
        }
    }
    
    public override void PlayerInteract()
    {
        base.PlayerInteract();
        Highlight();
        
    }
}
