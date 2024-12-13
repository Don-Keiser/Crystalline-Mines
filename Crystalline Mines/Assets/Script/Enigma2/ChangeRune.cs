using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class ChangeRune : Interactible
{
    [SerializeField] List<GameObject> _runes = new List<GameObject>();
    private List<GameObject> _instantiateRunes = new List<GameObject>();
    private int _runeIndex;
    private GameObject _displayRune;
    [SerializeField] GameObject _goodRune;
    private Color _color;
    private Color _alpha;
    
    [Header("Animation")]
    [SerializeField] float _duration = 2.0f; // duration of the fade in and fade out
    
    public bool canInteract = true;
    [NonSerialized] public bool isGoodRune = false;
    public event Action OnRuneChanged;

    private void Start()
    {
        _color = GetComponent<Light2D>().color;
        
        
        foreach (GameObject rune in _runes)
        {
            GameObject _instantiateRune = Instantiate(rune, new Vector3(transform.position.x,transform.position.y + 3
                ,transform.position.z), Quaternion.identity, transform);
            _instantiateRune.GetComponent<SpriteRenderer>().color = _color;
            _instantiateRune.GetComponent<Light2D>().color = _color;
            _instantiateRune.SetActive(false);
            _instantiateRune.name = rune.name;
            _instantiateRunes.Add(_instantiateRune);
        }

        do
        {
            _runeIndex = Random.Range(0, _instantiateRunes.Count);
            _displayRune = _instantiateRunes[_runeIndex];
        } while (_displayRune.name == _goodRune.name);
        
        _displayRune.SetActive(true);
    }

    public void Change()
    {
        
        // Deactivate the current GameObject
        if (_displayRune is not null)
        {
            StartCoroutine(FadeOut(_duration/2, _displayRune.GetComponent<SpriteRenderer>()));
            StartCoroutine(LightFadeIn(_duration/2, _displayRune.GetComponent<Light2D>()));
            StartCoroutine(CrystalFadeOut(_duration/2, GetComponent<Light2D>()));
        }

        // Go to next index
        if (_runeIndex == _instantiateRunes.Count - 1)
        {
            _runeIndex = 0;
        }
        else
        {
            _runeIndex++;
        }

        // Activate the new GameObject
        _displayRune = _instantiateRunes[_runeIndex];
        
        if (_displayRune is not null)
        {
            TimerManager.StartTimer(_duration/2, () =>
            {
                _displayRune.SetActive(true);
                StartCoroutine(FadeIn(_duration/2, _displayRune.GetComponent<SpriteRenderer>()));
                StartCoroutine(LightFadeOut(_duration/2, _displayRune.GetComponent<Light2D>()));
                StartCoroutine(CrystalFadeIn(_duration/2, GetComponent<Light2D>()));
            });
        }
        
        
        // check if the rune is the good one
        if (_displayRune is not null && _displayRune.name == _goodRune.name)
        {
            isGoodRune = true;
        }
        else
        {
            isGoodRune = false;
        }
    }
    
    private IEnumerator FadeOut(float duration, SpriteRenderer sprite)
    {
        for(float t = duration; t > 0.01f ; t -= Time.deltaTime)
        {
            _alpha = sprite.color;
            _alpha.a = t / duration;
            sprite.color = _alpha;
            yield return null;
        }
    }
    private IEnumerator FadeIn(float duration, SpriteRenderer sprite)
    {
        for(float t = 0.01f; t < duration ; t += Time.deltaTime)
        {
            _alpha = sprite.color;
            _alpha.a = t / duration;
            sprite.color = _alpha;
            yield return null;
        }
    }
     private IEnumerator LightFadeIn(float duration, Light2D light2D)
     {
         canInteract = false;
         light2D.enabled = true;
         for(float t = 0.00f; t < duration ; t += Time.deltaTime)
         {
             // fade out the light
             light2D.pointLightInnerRadius = t/duration * 1.5f;
             
             yield return null;
         }
         
         light2D.pointLightInnerRadius = 1.5f;
         light2D.gameObject.SetActive(false);
         light2D.enabled = false;
         OnRuneChanged?.Invoke();
     }
    
    private IEnumerator LightFadeOut(float duration, Light2D light2D)
    {
        light2D.enabled = true;
        for(float t = duration; t > 0.00f ; t -= Time.deltaTime)
        {
            // fade out the light
            light2D.pointLightInnerRadius = t/duration * 1.5f;
            
            yield return null;
        }
        light2D.pointLightInnerRadius = 0.0f;
        light2D.enabled = false;
        canInteract = true;
    }
    
    
    private IEnumerator CrystalFadeOut(float duration, Light2D light2D)
    {
        for(float t = duration; t > 0.00f ; t -= Time.deltaTime)
        {
            light2D.pointLightInnerRadius = t/duration * 0.5f;
            light2D.pointLightOuterRadius = t/duration;
            yield return null;
        }
        light2D.pointLightInnerRadius = 0.0f;
        light2D.pointLightOuterRadius = 0.0f;
    }
    
    private IEnumerator CrystalFadeIn(float duration, Light2D light2D)
    {
        for(float t = 0.00f; t < duration ; t += Time.deltaTime)
        {
            light2D.pointLightInnerRadius = t/duration * 0.5f;
            light2D.pointLightOuterRadius = t/duration;
            yield return null;
        }
        light2D.pointLightInnerRadius = 0.5f;
        light2D.pointLightOuterRadius = 1.0f;
    }
    
    public override void PlayerInteract()
    {
        base.PlayerInteract();
        if(canInteract)
        {
            Change();
        }
    }
}
