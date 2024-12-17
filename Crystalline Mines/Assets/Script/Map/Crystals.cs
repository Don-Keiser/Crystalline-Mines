using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Crystals : MonoBehaviour
{
    [NonSerialized] public List<SpriteRenderer> crystalsSpriteRenderers = new List<SpriteRenderer>();
    [NonSerialized] public List<Light2D> crystalsLights = new List<Light2D>();
    public Light2D globalLight;
    
    void Start()
    {
        foreach (Transform child in transform)
        {
            crystalsSpriteRenderers.Add(child.GetComponent<SpriteRenderer>());
            crystalsLights.Add(child.GetComponent<Light2D>());
        }
    }
    
    public void ChangeColor(Color color, bool isSrtartAnimation, float animDuration)
    {
        StartCoroutine(CrystalsColor(color, animDuration));

        if (isSrtartAnimation)
        {
            StartCoroutine(GlobalColor(color, animDuration, 0.5f));
        }
        else
        {
            StartCoroutine(GlobalColor(Color.white, animDuration,0f));
        }
    }
    
    private IEnumerator CrystalsColor(Color color, float duration)
    {
        for (float t = 0.00f; t < duration; t += Time.deltaTime)
        {
            crystalsSpriteRenderers.ForEach(crystalsColor =>
            {
                crystalsColor.color = Color.Lerp(crystalsColor.color, color, t / duration);
            });
            crystalsLights.ForEach(crystalsLightColor =>
            {
                crystalsLightColor.color = Color.Lerp(crystalsLightColor.color, color, t / duration);
            });
            yield return null;
        }
        
        crystalsSpriteRenderers.ForEach(crystalsColor =>
        {
            crystalsColor.color = color;
        });
        crystalsLights.ForEach(crystalsLightColor =>
        {
            crystalsLightColor.color = color;
        });
        
    }
    
    private IEnumerator GlobalColor(Color color, float duration, float saturation)
    {
        for (float t = 0.00f; t < duration; t += Time.deltaTime)
        {
            globalLight.color = Color.Lerp(globalLight.color, color, t / duration);
            Color.RGBToHSV(globalLight.color, out float h, out float s, out float v);
            s = saturation > 0 ? t/duration*saturation : Mathf.Lerp(s, 0, t / duration);
            globalLight.color = Color.HSVToRGB(h, s, v);
            
            yield return null;
        }
    }
}
