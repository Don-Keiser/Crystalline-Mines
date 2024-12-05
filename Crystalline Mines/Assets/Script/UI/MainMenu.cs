using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
 [SerializeField] private GameObject _settings;
    public void Quit()
    {
        Application.Quit();
    }

    public void Play(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void Settings()
    {
        _settings.SetActive(true);
    }
}
