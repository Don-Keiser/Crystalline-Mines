using UnityEngine;

public class Trap : MonoBehaviour
{
    [SerializeField] private Player _player;
    public void OnTriggerEnter2D(Collider2D other)
    {
        _player.transform.position = _player.zoneRespawnOfPlayer;
    }
}
