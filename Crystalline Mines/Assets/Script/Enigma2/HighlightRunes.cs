using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class HighlightRunes : Interactible
{
    [SerializeField] private Crystals _crystals; // list of room crystals
    [SerializeField] private float _timerDuration; // time during which the rune is displayed
    [SerializeField] private GameObject _rune;
    private SpriteRenderer _runeSprite;
    private Light2D _runeLight;

    private Coroutine _runeCoroutine;
    private bool _pointRuneLightIsActive;

    [Header("Anim on interact")]
    [SerializeField] private float _fullScreenDuration;
    [SerializeField] private float _animationDuration;
    [SerializeField] private float _maxDezoom;
    [SerializeField] private GameObject _cameraCenter;

    private void Start()
    {
        _runeSprite = _rune.GetComponent<SpriteRenderer>();
        _runeLight = _rune.GetComponent<Light2D>();
    }

    public void Highlight()
    {

        CancelAnimationHandler();
        _runeLight.enabled = true;
        _rune.SetActive(true);
        _runeSprite.enabled = false; // hide the rune
        EventManager.StartCameraAnimation(_cameraCenter.transform.position, _maxDezoom, _fullScreenDuration, _animationDuration);

        // display the rune
        _pointRuneLightIsActive = true;
        StartCoroutine(PointRuneLightAnimation());
        TimerManager.StartTimer((_fullScreenDuration + (2 * _animationDuration)), () =>
        {
            _runeLight.enabled = false;
            _runeSprite.enabled = true;

            _pointRuneLightIsActive = false;
            StopCoroutine(PointRuneLightAnimation());
        });

        _crystals.ChangeColor(_runeSprite.color, true, _animationDuration); // transform the color of the crystals to the color of the rune
        
        _runeCoroutine = StartCoroutine(AnimationHandler());
    }

    private IEnumerator PointRuneLightAnimation()
    {
        while (_pointRuneLightIsActive)
        {
            yield return new WaitForSeconds(0.25f);
            _runeLight.pointLightOuterRadius++;
            _runeLight.intensity++;
            yield return new WaitForSeconds(0.25f);
            _runeLight.pointLightOuterRadius--;
            _runeLight.intensity--;
        }
    }

    private IEnumerator AnimationHandler()
    {
        yield return new WaitForSeconds(_timerDuration + _fullScreenDuration + (2 * _animationDuration));

        _rune.SetActive(false);
        _crystals.ChangeColor(Color.white, false, _animationDuration); // transform the color of the crystals to white
    }
    private void CancelAnimationHandler()
    {
        if (_runeCoroutine != null)
        {
            StopCoroutine(_runeCoroutine);
            _runeCoroutine = null;
        }
    }

    public override void PlayerInteract()
    {
        base.PlayerInteract();
        Highlight();

    }
}
