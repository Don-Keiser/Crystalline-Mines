using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Crystals : MonoBehaviour
{
    [NonSerialized] public List<SpriteRenderer> crystalsSpriteRenderers = new List<SpriteRenderer>();
    [NonSerialized] public List<Light2D> crystalsLights = new List<Light2D>();
    
    void Start()
    {
        foreach (Transform child in transform)
        {
            crystalsSpriteRenderers.Add(child.GetComponent<SpriteRenderer>());
            crystalsLights.Add(child.GetComponent<Light2D>());
        }
    }
    
    public void ChangeColor(Color color)
    {
        crystalsSpriteRenderers.ForEach(crystalsColor =>
        {
            crystalsColor.color = color;
        });
        crystalsLights.ForEach(crystalsLightColor =>
        {
            crystalsLightColor.color = color;
        });
    }
}
