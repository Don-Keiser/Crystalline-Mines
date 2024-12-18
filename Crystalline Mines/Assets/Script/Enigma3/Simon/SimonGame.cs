using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class SimonGame : MonoBehaviour
{
    [Header("Wagon reference :")]
    [SerializeField] private Wagon _wagonScript;

    [Header("Cristal refs")]
    [SerializeField] private List<GameObject> _allCristals = new();
    [SerializeField] private GameObject _startingCristal;

    [Header("Cristal Sequence")]
    [SerializeField] private int _maxSequencelength;
    private List<GameObject> _cristalSequence = new();

    [SerializeField] private bool _enigmaIsLaunched;

    private bool _animationTime = false;
    private int _cristalIndex = 1;
    private bool _hasWin = false;

    private List<GameObject> _cristalPlayerChoose = new();

    [SerializeField] private DoorHandler.LevelRoom _levelRoom;

    [Header("Victory Screen TEST")]
    [SerializeField] private GameObject _victoryPanel;

    private void Start()
    {
        InitializeRandomSequence();
        FirstAnim();
    }
    
    private void ChangeLayer()
    {
        foreach (var cristal in _allCristals)
        {
            cristal.layer = cristal.layer == 0 ? 9 : 0;
        }
        _startingCristal.layer = _startingCristal.layer == 0 ? 9 : 0;
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
            while (!_enigmaIsLaunched)
            {
                yield return new WaitForSeconds(0.25f);
                _startingCristal.GetComponent<Light2D>().pointLightOuterRadius++;
                yield return new WaitForSeconds(0.25f);
                _startingCristal.GetComponent<Light2D>().pointLightOuterRadius--;
            }
        }
    }

    private IEnumerator CristalAnimation()
    {
        _animationTime = true;
        //if (_cristalIndex == 1) { yield return new WaitForSeconds(0.5f); }

        yield return new WaitForSeconds(0.5f);

        for (int i = 0; i < _cristalIndex; i++)
        {
            yield return new WaitForSeconds(0.25f);
            _cristalSequence[i].GetComponent<Light2D>().intensity += 2;
            yield return new WaitForSeconds(0.25f);
            _cristalSequence[i].GetComponent<Light2D>().intensity -= 2;
        }
        _animationTime = false;
    }


    private IEnumerator ClickedAnim(GameObject cristal)
    {
        yield return new WaitForSeconds(0.1f);
        cristal.GetComponent<Light2D>().intensity += 2;
        yield return new WaitForSeconds(0.1f);
        cristal.GetComponent<Light2D>().intensity -= 2;
    }

    private IEnumerator ResetAnim()
    {
        for (int i = 0; i < 3; i++)
        {
            yield return new WaitForSeconds(0.15f);
            foreach (var cristal in _allCristals)
            {
                cristal.GetComponent<Light2D>().intensity += 2;
            }

            yield return new WaitForSeconds(0.15f);

            foreach (var cristal in _allCristals)
            {
                cristal.GetComponent<Light2D>().intensity -= 2;
            }
        }
    }
    #endregion

    public void PlayerInteractCristal(GameObject choosedCristal)
    {
        if (_hasWin)
            return;
        bool reset = false;

        if (choosedCristal == _startingCristal && !_enigmaIsLaunched)
        {
            _enigmaIsLaunched = true;
            ChangeLayer();
            StopCoroutine(FirstCristalAnim());
            SimonIteration();
            return;
        }

        if (choosedCristal != _startingCristal && _enigmaIsLaunched)
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

        StartCoroutine(_wagonScript.WagonMotionRoutine());
    }

    private bool ResetEnigma()
    {
        print("wrong cristal");
        StartCoroutine(ResetAnim());

        _cristalPlayerChoose.Clear();
        _cristalIndex = 1;
        _enigmaIsLaunched = false;
        ChangeLayer();

        InitializeRandomSequence();
        FirstAnim();

        return true;
    }
    
    [ContextMenu("FinishEnigma")]
    public void FinishEnigma()
    {
        WinEnigma();
    }
}
