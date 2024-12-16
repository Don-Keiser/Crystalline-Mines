using System;
using UnityEngine;

public class TrapSpike : MonoBehaviour
{
    [SerializeField] private Player _player;

    private void Start()
    {
        _player = FindAnyObjectByType<Player>();
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        _player.transform.position = _player.respawnPosition;
    }
}
