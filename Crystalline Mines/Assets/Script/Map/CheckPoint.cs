using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    private Player _player;
    [SerializeField] private Transform _checkpoint;
    private Collider2D _collider;

    private void Start()
    {
        _collider = GetComponent<Collider2D>();
        _player = FindObjectOfType<Player>();
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        _player.zoneRespawnOfPlayer = _checkpoint.transform.position;
        Destroy(_collider);
    }
}
