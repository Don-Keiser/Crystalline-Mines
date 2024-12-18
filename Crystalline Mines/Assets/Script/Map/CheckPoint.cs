using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    public enum CheckPointState
    {
        Claimed,
        PlayerEnterTheMine,
        PlayerLeaveTheMine
    }
    [field: SerializeField] public CheckPointState state { get; private set; }

    [HideInInspector] public Vector3 respawnPosition;

    [Header("Camera Animation Parameter")]
    [SerializeField] private GameObject _levelCenter;
    [SerializeField] private float _maxCameraDezoom;
    [SerializeField] private float _animDuration;
    [SerializeField] private float _fullscreenDuration;

    Collider2D _collider2D;

    void Start()
    {
        _collider2D = GetComponent<Collider2D>();
        respawnPosition = transform.GetChild(0).position;
    }

    void OnTriggerEnter2D(Collider2D p_collider2D)
    {
        if (!p_collider2D.gameObject.CompareTag("Player"))
            return;
        
        Player player = p_collider2D.gameObject.GetComponent<Player>();

        if (player is null) 
            return;

        if (state == CheckPointState.PlayerEnterTheMine)
            EventManager.StartCameraAnimation(_levelCenter.transform.position, _maxCameraDezoom, _fullscreenDuration, _animDuration);
        
        player.respawnPosition = respawnPosition;

        SetNewState(CheckPointState.Claimed);
    }

    public void RespawnPlayer(Player p_player)
    {
        p_player.transform.position = p_player.respawnPosition;
    }

    public void SetNewState(CheckPointState p_newCheckPointState)
    {
        state = p_newCheckPointState;

        // TO DUBUG | All the color changement are for debug purpose
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();

        switch (state)
        {
            case CheckPointState.Claimed:
                _collider2D.enabled = false;
                spriteRenderer.color = new Color(1, 0, 0, 0.5f); // Transparent red
                break;

            case CheckPointState.PlayerEnterTheMine:
                _collider2D.enabled = true;
                spriteRenderer.color = new Color(0, 1, 0, 0.5f); // Transparent green
                break;

            case CheckPointState.PlayerLeaveTheMine:
                _collider2D.enabled = true;
                spriteRenderer.color = new Color(0, 0, 1, 0.5f); // Transparent blue
                break;

            default:
                Debug.LogError($"ERROR ! The given state '{p_newCheckPointState}' is not planned in the switch.");
                return;
        }
    }
}