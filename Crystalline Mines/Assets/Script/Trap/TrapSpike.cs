using System;
using UnityEngine;

public class TrapSpike : MonoBehaviour
{
    private Player _player;
    public void OnTriggerEnter2D(Collider2D collision)
    {
        Interact_Crystal crystal = collision.GetComponent<Interact_Crystal>();
        _player = collision.GetComponent<Player>();
        if(crystal != null)
        {
            collision.transform.position = crystal.initialPosition;
            crystal.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            crystal.GetComponent<Rigidbody2D>().isKinematic = true;
        }
        else if (collision.gameObject.CompareTag("Player"))
        {
            if (gameObject.layer == LayerMask.NameToLayer("SpikeBottom"))
            {
                Animation.Instance.DeadSpikeDownAnimation();
                TimerManager.StartTimer(0.4f, (() => collision.gameObject.GetComponent<Player>().Respawn()));
            }
            if (gameObject.layer == LayerMask.NameToLayer("SpikeTop"))
            {
                _player.isDead = true;
                Animation.Instance.DeadSpikeUpAnimation();
                TimerManager.StartTimer(0.4f, (() => collision.gameObject.GetComponent<Player>().Respawn()));
            }
            if (gameObject.layer == LayerMask.NameToLayer("TrapCrystal"))
            {
                Animation.Instance.DeadTrapCrystalAnimation();
                TimerManager.StartTimer(0.4f, (() => collision.gameObject.GetComponent<Player>().Respawn()));
            }
        }

    }
}
