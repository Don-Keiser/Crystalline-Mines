using UnityEngine;

public class Animation : MonoBehaviour
{
    public static Animation Instance;

    [SerializeField] private Animator _animator;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private Player _player;
    [SerializeField] private Controller _controller;

    private bool _alreadyPlayJumpSound;

    public void Awake()
    {
        Instance = Instantiator.ReturnInstance(this, Instantiator.InstanceConflictResolutions.WarningAndPause);
    }

    public void SetAnimationBool()
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

        ForceApplicator.ApplyImpulse(_player.gameObject.transform, Vector2.down, 1, 0.25f);

        _animator.Play("DeadSpikeDown", 0, 0f);

        SoundManager.Instance.PlaySound(SoundManager.Instance.deathSound);

        TimerManager.StartTimer(0.5f, () =>
        {
            _animator.Play("Stand", 0, 0f);
            _controller.gameObject.SetActive(true);
            _player.isDead = false;
        });
    }

    public void DeadSpikeUpAnimation()
    {
        _controller.gameObject.SetActive(false);

        _player.velocity = Vector2.zero;

        _animator.Play("DeadSpikeUp", 0, 0f);
        SoundManager.Instance.PlaySound(SoundManager.Instance.deathSound);

        TimerManager.StartTimer(0.5f, () =>
        {
            _animator.Play("Stand", 0, 0f);
            _controller.gameObject.SetActive(true);
            _player.isDead = false;
        });
    }

    public void DeadTrapCrystalAnimation()
    {
        _controller.gameObject.SetActive(false);

        _player.velocity = Vector2.zero;

        _animator.Play("DeadCrystal", 0, 0f);
        SoundManager.Instance.PlaySound(SoundManager.Instance.deathSound);

        TimerManager.StartTimer(0.5f, () =>
        {
            _animator.Play("Stand", 0, 0f);
            _controller.gameObject.SetActive(true);
            _player.isDead = false;
        });
    }
}