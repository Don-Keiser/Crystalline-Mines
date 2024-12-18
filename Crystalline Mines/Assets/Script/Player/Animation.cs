using System;
using System.Collections;
using UnityEngine;

public class Animation : MonoBehaviour
{
    public static Animation Instance;
    [SerializeField] private Animator _animator;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private Player _player;
    [SerializeField] private Controller _controller;

    private bool _isMoving;
    private bool _isRunning;

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
        TimerManager.StartTimer(0.25f, (() => _animator.SetFloat("Jump", 0)));
        _animator.SetFloat("Fall", 1);

        // Jouer le son de saut
        SoundManager.Instance.PlaySound(SoundManager.Instance.jumpSound);
    }

    public void FallAnimation()
    {
        int Falling = _player.CanJump() == true ? 0 : 1;
        _animator.SetFloat("Fall", Falling);
        if (Falling == 0 && !_isMoving)
        {
            _animator.Play("Stand", 0, 0f);
        }
        if (Falling == 0 && _isMoving)
        {
            _animator.Play("Run", 0, 0f);
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

        HandleRunningSound(); // Gérer le son de la course
    }

    private void HandleRunningSound()
    {
        bool isCurrentlyRunning = _isMoving && _player.velocity.y == 0;

        if (isCurrentlyRunning && !_isRunning)
        {
            _isRunning = true;
            SoundManager.Instance.PlaySound(SoundManager.Instance.runSound, true);
        }
        else if (!isCurrentlyRunning && _isRunning)
        {
            _isRunning = false;
            SoundManager.Instance.StopSound();
        }
    }

    public void DeadSpikeDownAnimation()
    {
        _controller.gameObject.SetActive(false);
        _player.velocity = Vector2.zero;
        _animator.Play("DeadSpikeDown", 0, 0f);

        // Jouer le son de mort
        SoundManager.Instance.PlaySound(SoundManager.Instance.deathSound);

        TimerManager.StartTimer(0.5f, (() => _animator.Play("Stand", 0, 0f)));
        TimerManager.StartTimer(0.5f, (() => _controller.gameObject.SetActive(true)));
    }

    public void DeadSpikeUpAnimation()
    {
        _controller.gameObject.SetActive(false);
        _player.velocity = Vector2.zero;
        _animator.Play("DeadSpikeUp", 0, 0f);

        // Jouer le son de mort
        SoundManager.Instance.PlaySound(SoundManager.Instance.deathSound);

        TimerManager.StartTimer(0.5f, (() => _animator.Play("Stand", 0, 0f)));
        TimerManager.StartTimer(0.5f, (() => _controller.gameObject.SetActive(true)));
        TimerManager.StartTimer(0.5f, (() => _player.isDead = false));
    }

    public void DeadTrapCrystalAnimation()
    {
        _controller.gameObject.SetActive(false);
        _player.velocity = Vector2.zero;
        _animator.Play("DeadCrystal", 0, 0f);

        // Jouer le son de mort
        SoundManager.Instance.PlaySound(SoundManager.Instance.deathSound);

        TimerManager.StartTimer(0.5f, (() => _animator.Play("Stand", 0, 0f)));
        TimerManager.StartTimer(0.5f, (() => _controller.gameObject.SetActive(true)));
    }

}
