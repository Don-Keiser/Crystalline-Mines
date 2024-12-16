using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] GameObject _settingsParent;
    [SerializeField] GameObject _credits;
    [SerializeField] AudioSource _audioButton;
    
    private void Start()
    {
        Settings settings = global::Settings.Instance;

        if (settings.isInOnDestroyOnLoad)
        {
            _audioButton = settings.audioSource;
            
            return;
        }

        DontDestroyOnLoad(_settingsParent.gameObject);

        settings.isInOnDestroyOnLoad = true;
        
    }

    public void Quit()
    {
        _audioButton.Play();
        Application.Quit();
    }

    public void Play(string sceneName)
    {
        _audioButton.Play();
        SceneManager.LoadScene(sceneName);
        Time.timeScale = 1.0f;
    }

    public void Settings()
    {
        _audioButton.Play();
        global::Settings.Instance._settingsPanel.SetActive(true);
    }

    public void Credits()
    {
        _audioButton.Play();
        _credits.SetActive(true);
    }

    public void BackToMainMenu()
    {
        _audioButton.Play();
        _credits.SetActive(false);
    }
}
