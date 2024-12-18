using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SolutionCheck : MonoBehaviour
{
    private List<ChangeRune> _lightPoints = new(); 
    [SerializeField] private DoorHandler.LevelRoom _outDoor;
    [SerializeField] private List<GameObject> _masterCrystals;

    private void Start()
    {
        foreach (Transform child in transform)
        {
            _lightPoints.Add(child.GetComponent<ChangeRune>());
            AddLightPoint(child.GetComponent<ChangeRune>());
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
            DoorHandler.Instance.GetDoor(_outDoor).OpenDoor(() => true);
            _lightPoints.ForEach(lightPoint => lightPoint.gameObject.layer = 0);
            _masterCrystals.ForEach(masterCrystals => masterCrystals.layer = 0);
        }
    }

    [ContextMenu("FinishEnigma")]
    public void FinishEnigma()
    {
        DoorHandler.Instance.GetDoor(_outDoor).OpenDoor(() => true);
        _lightPoints.ForEach(lightPoint => lightPoint.gameObject.layer = 0);
    }
}
