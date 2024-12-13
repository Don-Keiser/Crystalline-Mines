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
            StartCoroutine(FadeOut(_duration, _displayRune.GetComponent<SpriteRenderer>()));
            //StartCoroutine(LightFadeIn(_duration, _displayRune.GetComponent<Light2D>()));
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
            _displayRune.SetActive(true);
            StartCoroutine(FadeIn(_duration, _displayRune.GetComponent<SpriteRenderer>()));
            //StartCoroutine(LightFadeOut(_duration, _displayRune.GetComponent<Light2D>()));
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
        canInteract = false;
        for(float t = duration; t > 0.01f ; t -= Time.deltaTime)
        {
            _alpha = sprite.color;
            _alpha.a = t / duration;
            sprite.color = _alpha;
            yield return null;
        }
        sprite.gameObject.SetActive(false);
        canInteract = true;
        OnRuneChanged?.Invoke();
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
    // private IEnumerator LightFadeIn(float duration, Light2D light2D)
    // {
    //     light2D.enabled = true;
    //     for(float t = 0.00f; t < duration ; t += Time.deltaTime)
    //     {
    //         // fade out the light
    //         light2D.pointLightInnerRadius = t/1.5f;
    //         
    //         yield return null;
    //     }
    //
    //     light2D.pointLightInnerRadius = 1.5f;
    // }
    //
    // private IEnumerator LightFadeOut(float duration, Light2D light2D)
    // {
    //     canInteract = false;
    //     for(float t = duration; t > 0.00f ; t -= Time.deltaTime)
    //     {
    //         // fade out the light
    //         light2D.pointLightInnerRadius = t/duration;
    //         
    //         yield return null;
    //     }
    //     
    //     light2D.pointLightInnerRadius = 0f;
    //     light2D.enabled = false;
    //     light2D.gameObject.SetActive(false);
    //     canInteract = true;
    //     OnRuneChanged?.Invoke();
    // }
    
    
    public override void PlayerInteract()
    {
        base.PlayerInteract();
        if(canInteract)
        {
            Change();
        }
    }
}
