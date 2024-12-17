using System;
using System.Collections;
using UnityEngine;

public class Animation : MonoBehaviour
{
    public static Animation Instance;
    [SerializeField] private Animator _animator;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private Player _player;

    private bool _isMoving;

    public void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
    

    public void JumpAnimation()
    {
        _animator.SetFloat("Jump", 1);
        TimerManager.StartTimer(0.25f,(() => _animator.SetFloat("Jump", 0)));
        _animator.SetFloat("Fall", 1);
    }

    public void FallAnimation()
    {
        int Falling = _player.CanJump() == true ? 0 : 1;
        _animator.SetFloat("Fall", Falling);
        if (Falling == 0 && !_isMoving)
        {
            _animator.Play("Stand", 0, 0f); // "IdleAnimation" est le nom de votre animation Idle
        }
        if (Falling == 0 && _isMoving)
        {
            _animator.Play("Run", 0, 0f); // "IdleAnimation" est le nom de votre animation Idle
        }
    }

    public void MoveAnimation()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        
        _isMoving = horizontalInput != 0;
        _animator.SetFloat("Speed", _isMoving ? 1 : 0);

        if (_isMoving)
        {
            _spriteRenderer.flipX = horizontalInput < 0;
        }
    }
}