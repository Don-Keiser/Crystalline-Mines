using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    private Player _player;
    [SerializeField] private Transform _checkpoint;
    public Transform _checkpointFinal;
    private Collider2D _collider;
    [SerializeField] private bool _finalCheckpoint;

    [Header("Camera Animation Parameter")]
    [SerializeField] private GameObject _levelCenter;
    [SerializeField] private float _maxCameraDezoom;
    [SerializeField] private float _animDuration;
    [SerializeField] private float _fullscreenDureation;


    private void Start()
    {
        _collider = GetComponent<Collider2D>();
        _player = FindObjectOfType<Player>();
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (_finalCheckpoint == false)
        {
            EventManager.StartCameraAnimation(_levelCenter.transform.position, _maxCameraDezoom, _animDuration, _fullscreenDureation);
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
