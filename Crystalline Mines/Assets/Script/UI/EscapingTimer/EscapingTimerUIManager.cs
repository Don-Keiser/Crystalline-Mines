using System;
using System.Collections;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class EscapingTimerUIManager : MonoBehaviour
{
    [Header("Internal references :")]
    [SerializeField] GameObject _timerBackground;
    [SerializeField] TextMeshProUGUI _timerText;
    [SerializeField] Image _timerSlider;

    IEnumerator _escapingTimerCoroutine;

    void Start()
    {
        GameOverAndVictoryManager.OnEscapingStartEvent += StartTimer;
        GameOverAndVictoryManager.OnEscapingRestartEvent += RestartTimer;
        GameOverAndVictoryManager.OnEscapingEvent += StopTimer;
    }

    void RestartTimer(float p_timerStartInSeconds)
    {
        StopCoroutine(_escapingTimerCoroutine);

        _escapingTimerCoroutine = LaunchTimer(p_timerStartInSeconds);

        StartCoroutine(_escapingTimerCoroutine);
    }

    /// <summary>
    /// Stops the escaping timer if the player won. If this method need two boolean parameters it's because of the OnEscapingEvent. </summary>
    void StopTimer(bool p_isVictoryUI, bool p_newVisibility)
    {
        // If the victory UI showed up, that means the player wins the game, so we stops the coroutine.
        if (p_isVictoryUI && p_newVisibility)
            StopCoroutine(_escapingTimerCoroutine);
    }

    void StartTimer(float p_timerStartInSeconds)
    {
        _escapingTimerCoroutine = LaunchTimer(p_timerStartInSeconds);

        StartCoroutine(_escapingTimerCoroutine);
    }

    IEnumerator LaunchTimer(float p_timerStartInSeconds)
    {
        if (!_timerBackground.activeSelf)
            _timerBackground.SetActive(true);

        float remainingTimeInSeconds = p_timerStartInSeconds;

        while (remainingTimeInSeconds > 0)
        {
            remainingTimeInSeconds -= Time.deltaTime;

            // We update the slider with the normalized ramaining time [0:1]
            float normalizedRemainingTime = Mathf.InverseLerp(0, p_timerStartInSeconds, remainingTimeInSeconds);

            _timerSlider.fillAmount = normalizedRemainingTime;

            // Computing of the ramaining time (in minutes, seconds, millisecond)
            int roundedRemainingTimeInMinutes = Mathf.FloorToInt(remainingTimeInSeconds / 60);
            int roundedRemainingTimeInSeconds = Mathf.FloorToInt(remainingTimeInSeconds % 60);
            int roundedRemainingTimeInMilliseconds = Mathf.FloorToInt(remainingTimeInSeconds % 1 * 100);

            // We update the timer text UI
            _timerText.text = $"{roundedRemainingTimeInMinutes} : {roundedRemainingTimeInSeconds:D2} : {roundedRemainingTimeInMilliseconds:D2}";

            yield return new WaitForEndOfFrame();
        }

        _timerText.text = "0 : 00 : 00";
    }
}