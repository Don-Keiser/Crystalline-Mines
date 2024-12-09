using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverAndVictory : MonoBehaviour
{
    private Player _player;
    [SerializeField] CheckPoint _checkpoint;
    [SerializeField] private GameObject _gameOverPanel;
    [SerializeField] private GameObject _victoryPanel;
    private void Start()
    {
        _player = FindAnyObjectByType<Player>();
        Timer();
    }

    public void Exit(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void Retry()
    {
        _gameOverPanel.SetActive(false);
        Time.timeScale = 1;
        Timer();
        _player.transform.position = _checkpoint._checkpointFinal.transform.position;
    }

    private void Timer()
    {
        TimerManager.StartTimer(10, EndOfTimer);
    }

    private void EndOfTimer()
    {
        _gameOverPanel.SetActive(true);
        Time.timeScale = 0;
    }
}
