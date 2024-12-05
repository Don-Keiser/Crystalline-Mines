using UnityEngine;
using UnityEngine.SceneManagement;

public class Pause : MonoBehaviour
{
    [SerializeField] private GameObject _settings;
    [SerializeField] private GameObject _pausePanel;
    [SerializeField] private GameObject _pauseButton;


    public void OpenPause()
    {
        Time.timeScale = 0;
        _pausePanel.SetActive(true);
        _pauseButton.SetActive(false);
    }
    public void Resume()
    {
        Time.timeScale = 1;
        _pausePanel.SetActive(false);
        _pauseButton.SetActive(true);
    }
    public void Settings()
    {
        _settings.SetActive(true);
    }
    public void Exit(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
