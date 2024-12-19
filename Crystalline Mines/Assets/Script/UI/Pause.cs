using UnityEngine;
using UnityEngine.SceneManagement;

public class Pause : MonoBehaviour
{
    [SerializeField] private GameObject _settings;
    [SerializeField] private GameObject _pausePanel;
    [SerializeField] private GameObject _pauseButton;
    [SerializeField] private Player _player;
    private bool _active;

    [SerializeField] private CameraController _camera;

    private void Update()
    {
        if (!_camera.FinishAnim) { _pauseButton.SetActive(false); return; }
        else { _pauseButton.SetActive(true); }
        OpenPauseWithEscape();
    }

    private void OpenPauseWithEscape()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !_active)
        {
            OpenPauseWithButton();
            return;
        }

        if (Input.GetKeyDown(KeyCode.Escape) && _active && global::Settings.Instance.settingsActive == false)
        {
            Resume();
        }
    }
    private void OpenPauseWithButton()
    {
        global::Settings.Instance.audioSource.Play();
        Time.timeScale = 0;
        _pausePanel.SetActive(true);
        _pauseButton.SetActive(false);
        _active = true;
    }


    public void Resume()
    {
        global::Settings.Instance.audioSource.Play();
        Time.timeScale = 1;
        _pausePanel.SetActive(false);
        _pauseButton.SetActive(true);
        _active = false;
    }

    public void Retry()
    {
        global::Settings.Instance.audioSource.Play();

        _player.transform.position = _player.respawnPosition;

        Resume();
    }
    public void Settings()
    {
        global::Settings.Instance.audioSource.Play();
        global::Settings.Instance.settingsActive = true;
        global::Settings.Instance._settingsPanel.SetActive(true);
    }
    public void Exit(string sceneName)
    {
        global::Settings.Instance.audioSource.Play();

        global::Settings.Instance.musicSource.Stop();
        global::Settings.Instance.musicSource.clip = global::SoundManager.Instance.mainMenuSong;
        global::Settings.Instance.musicSource.Play();

        SceneManager.LoadScene(sceneName);
    }
}
