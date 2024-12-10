using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    private Player _player;
    [SerializeField] private Transform _checkpoint;
    public Transform _checkpointFinal;
    private Collider2D _collider;
    [SerializeField] private bool _finalCheckpoint;

    private void Start()
    {
        _collider = GetComponent<Collider2D>();
        _player = FindObjectOfType<Player>();
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (_finalCheckpoint == false)
        {
            _player.zoneRespawnOfPlayer = _checkpoint.transform.position;
            Destroy(_collider);
        }
        if (_finalCheckpoint)
        {
            _player.zoneRespawnOfPlayer = _checkpointFinal.transform.position;
            Destroy(_collider);
        }
    }
}
