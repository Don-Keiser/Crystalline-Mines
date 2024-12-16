using UnityEngine;

public class Rock : MonoBehaviour
{
    private Player _player;

    private void Start()
    {
        _player = FindAnyObjectByType<Player>();
    }

    private void OnCollisionEnter2D()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            _player.transform.position = _player.respawnPosition;
        }
        Destroy(gameObject);
    }
}
