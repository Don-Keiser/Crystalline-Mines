using UnityEngine;

/// <summary>
/// This script is for debugging purpose only, he have to be removed when producing the final build (after presentation). </summary>
public class TESTING_SCRIPT : MonoBehaviour
{
    [SerializeField] KeyCode _resetAllRailKey = KeyCode.P;
    [SerializeField] KeyCode _respawnPlayerKey = KeyCode.O;
    [SerializeField] KeyCode _setCheckPointStateToPlayerLeaveTheMineKey = KeyCode.I;
    [SerializeField] KeyCode _playerWonKey = KeyCode.U;
    [SerializeField] KeyCode _launchFinishRailEnigmaEvent = KeyCode.Y;

    void Update()
    {
        if (Input.GetKeyDown(_resetAllRailKey))
        {
            print("DEBUGGING ! Reset rails");

            RailManager.Instance.ResetAllRails();
        }

        if (Input.GetKeyDown(_respawnPlayerKey))
        {
            print("DEBUGGING ! Respawn player");

            Player.PlayerTransform.GetComponent<Player>().Respawn();
        }

        if (Input.GetKeyDown(_setCheckPointStateToPlayerLeaveTheMineKey))
        {
            print($"DEBUGGING ! Check point's state changed to {nameof(CheckPoint.CheckPointState.PlayerLeaveTheMine)}");

            CheckPointHandler.Instance.SetAllCheckPointState(CheckPoint.CheckPointState.PlayerLeaveTheMine);
        }

        if (Input.GetKeyDown(_playerWonKey))
        {
            print($"DEBUGGING ! Launch player victory");

            GameOverAndVictoryManager.EscapingSuccess();
        }

        if (Input.GetKeyDown(_launchFinishRailEnigmaEvent))
        {
            print($"DEBUGGING ! Launch finished rail enigma event");

            RailManager.onAllRailsRepairedEvent?.Invoke();
        }
    }
}