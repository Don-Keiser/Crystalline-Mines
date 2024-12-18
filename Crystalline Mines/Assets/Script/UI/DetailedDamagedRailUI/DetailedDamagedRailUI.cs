using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DetailedDamagedRailUI : MonoBehaviour
{
    [Header("Statistics :")]
    [SerializeField] private float _visibilityDurationInSeconds = 2;

    [Header("Internal references :")]
    [SerializeField] private GameObject _detailedDamagedRailCanvasGameObject;
    [SerializeField] private Image _damagedRailImage;

    private IEnumerator _timerCoroutine;

    void Start()
    {
        if (_damagedRailImage == null)
        {
            Debug.LogError($"ERROR ! The variable '{nameof(_damagedRailImage)}' has not been setted.");
            return;
        }

        RailManager.onShowDetailedDamagedRailEvent += ShowDetailDamagedRailUI;
    }

    void ShowDetailDamagedRailUI(Sprite p_newSprite)
    {
        _damagedRailImage.sprite = p_newSprite;

        if (_timerCoroutine != null)
            StopCoroutine(_timerCoroutine);

        _timerCoroutine = StartShowingDetailDamagedRailUI(_visibilityDurationInSeconds);
        StartCoroutine(_timerCoroutine);
    }

    IEnumerator StartShowingDetailDamagedRailUI(float p_visibilityDurationInSeconds)
    {
        _detailedDamagedRailCanvasGameObject.SetActive(true);

        yield return new WaitForSeconds(p_visibilityDurationInSeconds);

        _detailedDamagedRailCanvasGameObject.SetActive(false);

        _timerCoroutine = null;
    }
}