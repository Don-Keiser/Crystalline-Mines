using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SolutionCheck : MonoBehaviour
{
    private List<ChangeRune> _lightPoints = new List<ChangeRune>(); // TODO: modify to eliminate the call loop and be more flexible
    [SerializeField] GameObject _outDoor;

    private void Start()
    {
        foreach (Transform child in transform)
        {
            _lightPoints.Add(child.GetComponent<ChangeRune>());
            AddLightPoint(child.GetComponent<ChangeRune>());
            print(child.name);
        }
    }

    public void AddLightPoint(ChangeRune lightPoint)
    {
        _lightPoints.Add(lightPoint);
        lightPoint.OnRuneChanged += CheckSolution;
    }
    public void CheckSolution()
    {
        bool isCorrect = true;
        foreach (ChangeRune lightPoint in _lightPoints)
        {
            if (!lightPoint.isGoodRune)
            {
                isCorrect = false;
                break;
            }
        }

        if (isCorrect)
        {
            _outDoor.SetActive(false);
            _lightPoints.ForEach(lightPoint => lightPoint.canInteract = false);
        }
    }
}
