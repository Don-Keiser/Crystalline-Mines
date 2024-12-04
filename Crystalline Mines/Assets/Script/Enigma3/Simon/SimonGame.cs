using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimonGame : MonoBehaviour
{
    [Header("Cristal ref")]
    [SerializeField] private List<GameObject> _allCristals = new();
    [SerializeField] private GameObject _startingCristal;


    [Header("Cristal Sequence")]
    [SerializeField] private int _maxSequencelength;
    private List<GameObject> _cristalSequence = new();
    private bool _enigmaIsLunched = false;
    private bool _animationTime = false;
    private int _cristalIndex = 1;



    private List<GameObject> _cristalPlayerChoose = new();

    private void Start()
    {
        InitializeRandomSequence();
        FirstAnim();
    }
    #region Initialize sequence
    private int PickRandomCristal()
    {
        return Random.Range(0, _allCristals.Count);
    }
    private void InitializeRandomSequence() //appel pour choisir une sequence aléatoire
    {
        if (_cristalSequence.Count > 0) { _cristalSequence.Clear(); }

        for (int i = 0; i < _maxSequencelength; i++)
        {
            _cristalSequence.Add(_allCristals[PickRandomCristal()]);
        }
    }
    #endregion

    #region Scale Animation
    private void SimonIteration()
    {
        StartCoroutine(CristalAnimation());
    }
    private void FirstAnim()
    {
        StartCoroutine(FirstCristalAnim());
    }
    private IEnumerator FirstCristalAnim()
    {
        while (!_enigmaIsLunched)
        {
            yield return new WaitForSeconds(0.25f);
            _startingCristal.transform.localScale += new Vector3(0.5f, 0.5f, 0.5f);
            yield return new WaitForSeconds(0.25f);
            _startingCristal.transform.localScale -= new Vector3(0.5f, 0.5f, 0.5f);
        }
    }
    private IEnumerator CristalAnimation()
    {
        _animationTime = true;
        if (_cristalIndex == 1) { yield return new WaitForSeconds(0.5f); }

        for (int i = 0; i < _cristalIndex; i++)
        {
            yield return new WaitForSeconds(0.25f);
            _cristalSequence[i].transform.localScale += Vector3.one;
            yield return new WaitForSeconds(0.25f);
            _cristalSequence[i].transform.localScale -= Vector3.one;
        }
        _animationTime = false;
    }

    private IEnumerator ClickedAnim(GameObject cristal)
    {
        yield return new WaitForSeconds(0.1f);
        cristal.transform.localScale += new Vector3(0.5f, 0.5f, 0.5f);
        yield return new WaitForSeconds(0.1f);
        cristal.transform.localScale -= new Vector3(0.5f, 0.5f, 0.5f);
    }
    private IEnumerator ResetAnim()
    {
        for (int i = 0; i < 3; i++)
        {
            yield return new WaitForSeconds(0.15f);

            foreach (var cristal in _allCristals)
            {
                cristal.transform.localScale += Vector3.one;
            }

            yield return new WaitForSeconds(0.15f);

            foreach (var cristal in _allCristals)
            {
                cristal.transform.localScale -= Vector3.one;
            }
        }
    }
    #endregion

    public void PlayerInteractCristal(GameObject choosedCristal)
    {
        bool reset = false;


        if (choosedCristal == _startingCristal && !_enigmaIsLunched) //lance l enigme
        {
            _enigmaIsLunched = true;
            StopCoroutine(FirstCristalAnim());
            SimonIteration();
            return;
        }


        if (choosedCristal != _startingCristal && _enigmaIsLunched)
        {
            if (_animationTime) { return; }

            StartCoroutine(ClickedAnim(choosedCristal));
            _cristalPlayerChoose.Add(choosedCristal);


            for (int i = 0; i < _cristalPlayerChoose.Count; i++)
            {
                if (_cristalSequence[i] != _cristalPlayerChoose[i])
                {
                    reset = ResetEnigma();
                }
            }

            if (_cristalPlayerChoose.Count == _cristalIndex)
            {
                if (_cristalPlayerChoose.Count >= _maxSequencelength) { print("you win!"); return; } //mettre fin enigme 

                if (!reset)
                {
                    _cristalIndex++;
                    SimonIteration();
                    _cristalPlayerChoose.Clear();
                }

            }
        }
    }
    private bool ResetEnigma()
    {
        print("wrong cristal");
        StartCoroutine(ResetAnim());

        _cristalPlayerChoose.Clear();
        _cristalIndex = 1;
        _enigmaIsLunched = false;

        InitializeRandomSequence();
        FirstAnim();

        return true;
    }
}
