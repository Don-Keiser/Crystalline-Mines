using UnityEngine;

public class Animation : MonoBehaviour
{
    public static Animation Instance;
    [SerializeField] private Animator _animator;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private Player _player;
    [SerializeField] private Controller _controller;

    private bool _alreadyPlayJumpSound;
    private bool _isMoving;
    private bool _isRunning;

    public void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    private void Update()
    {
        SetAnimationBool();
    }


    private void SetAnimationBool()
    {
        if (_player.velocity.x != 0 && _player.CanJump())
        {
            _animator.SetBool("playerMove", true);
            _animator.SetBool("fall", false);
            _animator.SetBool("isJumping", false);
            SoundManager.Instance.PlaySound(SoundManager.Instance.runSound, true);
        }
        else if (_player.velocity.x == 0)
        {
            _animator.SetBool("canJump", true);
            _animator.SetBool("playerMove", false);
            _animator.SetBool("fall", false);
            _animator.SetBool("isJumping", false);
            SoundManager.Instance.StopSound();
        }
        if (_player.CanJump())
        {
            _animator.SetBool("canJump", true);
            _alreadyPlayJumpSound = false;
            _animator.SetBool("fall", false);
            _animator.SetBool("isJumping", false);
        }
        else if (_player.velocity.y > 0)
        {
            SoundManager.Instance.StopSound();
            _animator.SetBool("isJumping", true);
            _animator.SetBool("canJump", false);
            if (!_alreadyPlayJumpSound) { SoundManager.Instance.PlaySound(SoundManager.Instance.jumpSound); _alreadyPlayJumpSound = true; }
        }
        else
        {
            SoundManager.Instance.StopSound();
            _animator.SetBool("fall", true);
            _animator.SetBool("isJumping", false);
            _animator.SetBool("canJump", false);
        }
        _spriteRenderer.flipX = (_player.velocity.x > 0) ? false : true;
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
