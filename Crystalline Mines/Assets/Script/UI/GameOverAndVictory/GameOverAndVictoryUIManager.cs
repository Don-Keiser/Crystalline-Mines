using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverAndVictoryUIManager : MonoBehaviour
{
    public static Action OnRestartEscapingEvent;

    [Header("Internal references :")]
    [SerializeField] GameObject _gameOverPanel;
    [SerializeField] GameObject _victoryPanel;

    void Start()
    {
        if (!IsCorrectlySet())
            return;

        GameOverAndVictoryManager.OnEscapingEvent += HandleGameOverAndVictoryUIVisibility;
    }

    bool IsCorrectlySet()
    {
        bool isDefinedProperly = true;

        if (_gameOverPanel == null)
        {
            Debug.LogError(
                $"ERROR ! The '{nameof(_gameOverPanel)}' GameObject variable is not defined."
            );

            isDefinedProperly = false;
        }

        if (_victoryPanel == null)
        {
            Debug.LogError(
                $"ERROR ! The '{nameof(_victoryPanel)}' GameObject variable is not defined."
            );

            isDefinedProperly = false;
        }

        return isDefinedProperly;
    }

    void HandleGameOverAndVictoryUIVisibility(bool p_isVictoryUI, bool p_newVisibility)
    {
        Time.timeScale = 0.0f;
        if (p_isVictoryUI)
            _victoryPanel.SetActive(p_newVisibility);
        else
            _gameOverPanel.SetActive(p_newVisibility);
    }

    public void RestartEscaping()
    {
        OnRestartEscapingEvent?.Invoke();
    }

    public void ChangeScene(string p_sceneName)
    {
        SceneManager.LoadScene(p_sceneName);
    }
}