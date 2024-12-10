using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject _settings;
    [SerializeField] private GameObject _credits;
    public void Quit()
    {
        Application.Quit();
    }

    public void Play(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
        Time.timeScale = 1.0f;
    }

    public void Settings()
    {
        _settings.SetActive(true);
    }

    public void Credits()
    {
        _credits.SetActive(true);
    }

    public void BackToMainMenu()
    {
        _credits.SetActive(false);
    }
}
