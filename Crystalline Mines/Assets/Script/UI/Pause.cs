using UnityEngine;
using UnityEngine.SceneManagement;

public class Pause : MonoBehaviour
{
    [Header("External references :")]
    [SerializeField] private Player _player;

    [SerializeField] private CameraController _camera;

    [Header("Internal references :")]
    [SerializeField] private GameObject _pausePanel;
    [SerializeField] private GameObject _pauseButton;

    private Settings _settings;
    private SoundManager _soundManager;

    private bool _isActive;

    private void Start()
    {
        _settings = Settings.Instance;
        _soundManager = SoundManager.Instance;
    }

    private void Update()
    {
        if (!_camera.FinishAnim)
        {
            _pauseButton.SetActive(false); 
            return; 
        }
        else 
        { 
            _pauseButton.SetActive(true); 
        }

        OpenPauseWithEscape();
    }

    private void OpenPauseWithEscape()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !_isActive)
        {
            OpenPauseWithButton();
            return;
        }

        if (Input.GetKeyDown(KeyCode.Escape) && _isActive && _settings.settingsActive == false)
        {
            Resume();
        }
    }

    private void OpenPauseWithButton()
    {
        _settings.audioSource.Play();

        Time.timeScale = 0;

        _pausePanel.SetActive(true);
        _pauseButton.SetActive(false);

        _isActive = true;
    }

    public void Resume()
    {
        _settings.audioSource.Play();

        Time.timeScale = 1;

        _pausePanel.SetActive(false);
        _pauseButton.SetActive(true);

        _isActive = false;
    }

    public void Retry()
    {
        _settings.audioSource.Play();

        _player.Respawn();

        Resume();
    }

    public void OpenSettings()
    {
        _settings.audioSource.Play();
        _settings.settingsActive = true;
        _settings._settingsPanel.SetActive(true);
    }

    public void Exit(string p_sceneName)
    {
        _settings.audioSource.Play();
        
        _settings.musicSource.Stop();
        _settings.musicSource.clip = _soundManager.mainMenuSong;
        _settings.musicSource.Play();

        SceneManager.LoadScene(p_sceneName);
    }
}