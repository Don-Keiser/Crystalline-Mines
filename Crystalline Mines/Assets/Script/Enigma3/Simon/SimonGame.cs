using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimonGame : MonoBehaviour
{
    [Header("Cristal refs")]
    [SerializeField] private List<GameObject> _allCristals = new();
    [SerializeField] private GameObject _startingCristal;

    [Header("Cristal Sequence")]
    [SerializeField] private int _maxSequencelength;
    private List<GameObject> _cristalSequence = new();
    public bool EnigmaIsLaunched { get; private set; }

    private bool _animationTime = false;
    private int _cristalIndex = 1;
    private bool _hasWin = false;

    private List<GameObject> _cristalPlayerChoose = new();


    [Header("Victory Screen TEST")]
    [SerializeField] private GameObject _victoryPanel;

    private void Start()
    {
        EnigmaIsLaunched = false;
        InitializeRandomSequence();
        FirstAnim();
    }

    #region Initialize sequence
    private int PickRandomCristal()
    {
        return Random.Range(0, _allCristals.Count);
    }
    private void InitializeRandomSequence()
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
        if (_startingCristal != null)
        {
            while (!EnigmaIsLaunched)
            {
                yield return new WaitForSeconds(0.25f);
                _startingCristal.transform.localScale += new Vector3(0.5f, 0.5f, 0.5f);
                yield return new WaitForSeconds(0.25f);
                _startingCristal.transform.localScale -= new Vector3(0.5f, 0.5f, 0.5f);
            }
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
        if (_hasWin)
            return;
        bool reset = false;

        if (choosedCristal == _startingCristal && !EnigmaIsLaunched)
        {
            EnigmaIsLaunched = true;
            StopCoroutine(FirstCristalAnim());
            SimonIteration();
            return;
        }

        if (choosedCristal != _startingCristal && EnigmaIsLaunched)
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
                if (_cristalPlayerChoose.Count >= _maxSequencelength)
                {
                    WinEnigma();
                    return;
                }

                if (!reset)
                {
                    _cristalIndex++;
                    SimonIteration();
                    _cristalPlayerChoose.Clear();
                }
            }
        }
    }

    private void WinEnigma()
    {
        Player.CanOpenTheDoor = true;

        TimerManager.StartTimer(0.5f, () =>
        {
            foreach (var crystal in _allCristals)
            {
                crystal.GetComponent<SpriteRenderer>().color = Color.black;
                crystal.layer = 0;
            }
            _startingCristal.GetComponent<SpriteRenderer>().color = Color.black;
            _startingCristal.layer = 0;
        });

        //TEST BUILD
        TimerManager.StartTimer(3.0f, () =>
        {
            _victoryPanel.gameObject.SetActive(true);
            Time.timeScale = 0.0f;
        });
    }

    private bool ResetEnigma()
    {
        print("wrong cristal");
        StartCoroutine(ResetAnim());

        _cristalPlayerChoose.Clear();
        _cristalIndex = 1;
        EnigmaIsLaunched = false;

        InitializeRandomSequence();
        FirstAnim();

        return true;
    }
}
