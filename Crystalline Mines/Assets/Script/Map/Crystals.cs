using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Crystals : MonoBehaviour
{
    [NonSerialized] public List<SpriteRenderer> crystalsSpriteRenderers = new List<SpriteRenderer>();
    [NonSerialized] public List<Light2D> crystalsLights = new List<Light2D>();
    public Light2D _globalLight;
    
    void Start()
    {
        foreach (Transform child in transform)
        {
            crystalsSpriteRenderers.Add(child.GetComponent<SpriteRenderer>());
            crystalsLights.Add(child.GetComponent<Light2D>());
        }
    }
    
    public void ChangeColor(Color color, bool isSrtartAnimation)
    {
        crystalsSpriteRenderers.ForEach(crystalsColor =>
        {
            crystalsColor.color = color;
        });
        crystalsLights.ForEach(crystalsLightColor =>
        {
            crystalsLightColor.color = color;
        });

        if (isSrtartAnimation)
        {
            _globalLight.color = color;
            Color.RGBToHSV(_globalLight.color, out float h, out float s, out float v);
            s = 0.5f; // Reduce saturation to 50%
            _globalLight.color = Color.HSVToRGB(h, s, v);
        }
        else
        {
            _globalLight.color = Color.white;
        }
        
        
    }
}
