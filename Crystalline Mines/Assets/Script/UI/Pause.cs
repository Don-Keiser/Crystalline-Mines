using UnityEngine;
using UnityEngine.SceneManagement;

public class Pause : MonoBehaviour
{
    [SerializeField] private GameObject _settings;
    [SerializeField] private GameObject _pausePanel;
    [SerializeField] private GameObject _pauseButton;
    [SerializeField] private Player _player;
    private bool _active;

    private void Update()
    {
        OpenPauseWithEscape();
    }

    private void OpenPauseWithEscape()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !_active)
        {
            OpenPauseWithButton();
            return;
        }

        if (Input.GetKeyDown(KeyCode.Escape) && _active)
        {
            Resume();
        }
    }
    private void OpenPauseWithButton()
    {
        Time.timeScale = 0;
        _pausePanel.SetActive(true);
        _pauseButton.SetActive(false);
        _active = true;
    }
    

    public void Resume()
    {
        Time.timeScale = 1;
        _pausePanel.SetActive(false);
        _pauseButton.SetActive(true);
        _active = false;
    }

    public void Retry()
    {
        _player.transform.position = _player.zoneRespawnOfPlayer;
        Resume();
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
