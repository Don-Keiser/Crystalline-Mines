using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SolutionCheck : MonoBehaviour
{
    private List<ChangeRune> _lightPoints = new List<ChangeRune>(); // TODO: modify to eliminate the call loop and be more flexible
    [SerializeField] GameObject _outDoor;
    
    public void AddLightPoint(ChangeRune lightPoint)
    {
        _lightPoints.Add(lightPoint);
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
