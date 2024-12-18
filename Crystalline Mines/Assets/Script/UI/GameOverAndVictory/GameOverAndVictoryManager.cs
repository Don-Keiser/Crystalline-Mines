using System;
using UnityEngine;

public class GameOverAndVictoryManager : MonoBehaviour
{
    public static Action<float> OnEscapingStartEvent;
    public static Action<float> OnEscapingRestartEvent;

    /// <summary>
    /// The two parameters reprensents the type of UI showed or hided.
    /// <para> Exemple1 : 'OnEscapingEvent?.Invoke(false, false);' means the GameOverUI will be hide. </para>
    /// <para> Exemple2 : 'OnEscapingEvent?.Invoke(true, true);' means the VictoryUI will be show. </para> </summary>
    public static Action<bool, bool> OnEscapingEvent;

    [Header("Statistics :")]
    [SerializeField] float _escapingTimeInSeconds = 120;

    Player _player;
    CheckPointHandler _checkPointHandler;

    [Tooltip("Represent the Treasure check point")]
    CheckPoint _lastCheckPoint;
    BoxCollider2D _boxCollider2D;

    void Start()
    {
        _player = Player.PlayerTransform.GetComponent<Player>();
        _checkPointHandler = CheckPointHandler.Instance;
        _lastCheckPoint = _checkPointHandler.GetCheckPoint(_checkPointHandler.checkPointDictionary.Count - 1);
        _boxCollider2D = GetComponent<BoxCollider2D>();

        _boxCollider2D.enabled = false;

        Treasure.OnPlayerPickupTreasureEvent += StartEscapingTimer;
        GameOverAndVictoryUIManager.OnRestartEscapingEvent += RetryEscaping;
    }

    private void OnTriggerEnter2D(Collider2D p_collider2D)
    {
        if (!p_collider2D.gameObject.CompareTag("Player"))
            return;

        EscapingSuccess();
    }

    public static void EscapingSuccess()
    {
        OnEscapingEvent?.Invoke(true, true);
    }

    public void RetryEscaping()
    {
        Time.timeScale = 1;

        _player.respawnPosition = _lastCheckPoint.respawnPosition;

        _player.Respawn();

        StartEscapingTimer();

        OnEscapingEvent?.Invoke(false, false);
        OnEscapingRestartEvent?.Invoke(_escapingTimeInSeconds);
    }

    public void StartEscapingTimer()
    {
        OnEscapingStartEvent?.Invoke(_escapingTimeInSeconds);

        _boxCollider2D.enabled = true;

        TimerManager.StartTimer(_escapingTimeInSeconds, EscapingFailure);
    }

    void EscapingFailure()
    {
        OnEscapingEvent?.Invoke(false, true);

        Time.timeScale = 0;
    }
}