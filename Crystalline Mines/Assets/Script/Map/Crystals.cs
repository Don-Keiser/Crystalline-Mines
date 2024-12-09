using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Crystals : MonoBehaviour
{
    public static List<GameObject> crystals = new List<GameObject>(); // currently unused
    public static List<SpriteRenderer> crystalsColor = new List<SpriteRenderer>();
    public static List<Light2D> crystalsLightColor = new List<Light2D>();
    
    void Start()
    {
        foreach (Transform child in transform)
        {
            crystals.Add(child.gameObject);
            crystalsColor.Add(child.GetComponent<SpriteRenderer>());
            crystalsLightColor.Add(child.GetComponent<Light2D>());
        }
    }
    
    public static void ChangeColor(Color color)
    {
        crystalsColor.ForEach(crystalsColor =>
        {
            crystalsColor.color = color;
        });
        crystalsLightColor.ForEach(crystalsLightColor =>
        {
            crystalsLightColor.color = color;
        });
    }
}
