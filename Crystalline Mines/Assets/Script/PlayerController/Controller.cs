using UnityEngine;

public class Controller : MonoBehaviour
{
    [SerializeField] private Player _player;

    private Vector2 _moveInput;

    void Update()
    {
        _moveInput.x = Input.GetAxisRaw("Horizontal");
        _moveInput.y = Input.GetAxisRaw("Vertical");

        _player.SetMoveInput(_moveInput);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            _player.Jump();
        }
        if (Input.GetKeyDown(KeyCode.S)) //shift
        {
            _player.DropThroughPlatform(-1);
        }
    }
}