using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crystals : MonoBehaviour
{
    public static List<GameObject> crystals = new List<GameObject>();

    // TODO: retrieves all the object's children and stores them in a list
    void Start()
    {
        foreach (Transform child in transform)
        {
            crystals.Add(child.gameObject);
        }
    }
}
