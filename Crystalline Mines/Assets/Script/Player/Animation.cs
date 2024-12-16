using System;
using System.Collections;
using UnityEngine;

public class Animation : MonoBehaviour
{
    public static Animation Instance;
    [SerializeField] private Animator _animator;
    [SerializeField] private SpriteRenderer _spriteRenderer;

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