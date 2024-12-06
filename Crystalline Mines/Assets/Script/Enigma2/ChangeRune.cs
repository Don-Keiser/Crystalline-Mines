using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class ChangeRune : Interactible // TODO: Clean up the code
{
    [SerializeField] List<GameObject> _runes = new List<GameObject>();
    List<GameObject> _instantiateRunes = new List<GameObject>();
    private int _runeIndex;
    GameObject _displayRune;
    [SerializeField] GameObject _goodRune;
    [SerializeField] SolutionCheck _solutionCheck;
    
    [NonSerialized] public bool isGoodRune = false;

    private void Start()
    {
        _solutionCheck.AddLightPoint(this);
        foreach (GameObject rune in _runes)
        {
            GameObject _instantiateRune = Instantiate(rune, new Vector3(transform.position.x,transform.position.y + 3,transform.position.z), Quaternion.identity, transform);
            _instantiateRune.SetActive(false);
            _instantiateRune.name = rune.name;
            _instantiateRunes.Add(_instantiateRune);
        }
        
        
        _runeIndex = Random.Range(0, _instantiateRunes.Count);
        _displayRune = _instantiateRunes[_runeIndex];
        _displayRune.SetActive(true);
    }

    public void Change()
    {
        // Deactivate the current GameObject
        if (_displayRune is not null)
        {
            _displayRune.SetActive(false);
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
        }
        
        
        // check if the rune is the good one
        if (_displayRune is not null && _displayRune.name == _goodRune.name)
        {
            print("Good Rune");
            isGoodRune = true;
        }
        else
        {
            isGoodRune = false;
        }
        
        _solutionCheck.CheckSolution();
    }
    
    public override void PlayerInteract()
    {
        base.PlayerInteract();
        Change();
    }
}
