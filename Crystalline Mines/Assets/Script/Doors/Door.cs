using System;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer), typeof(BoxCollider2D))]
public class Door : MonoBehaviour
{
    [Header("External references :")]
    [SerializeField] protected Animator _animator;

    protected Transform _transform;

    protected Vector3 _initialPosition;
    protected Quaternion _initialRotation;

    bool _isOpen;

    virtual protected void Start()
    {
        _transform = transform;

        _initialPosition = _transform.position;
        _initialRotation = _transform.rotation;
    }

    public void OpenDoor(Func<bool> p_openningCondition)
    {
        if (p_openningCondition == null)
        {
            Debug.LogError("ERROR ! No method has been gave.");
            return;
        }

        if (_isOpen)
            return;

        if (p_openningCondition.Invoke())
        {
            _isOpen = true;

            PlayOpeningAnimation();
            PlayOpeningSFX();
        }
        else
        {
            if (_isOpen)
                ResetDoor();
        }
    }

    virtual protected void PlayOpeningAnimation()
    {
        TimerManager.StartTimer(3.0f, () => gameObject.SetActive(false));
    }

    virtual protected void PlayOpeningSFX()
    {
        SoundManager.Instance.PlaySound(SoundManager.Instance.doorSound);
    }

    virtual protected void ResetDoor()
    {
        gameObject.SetActive(true);

        _transform.SetPositionAndRotation(_initialPosition, _initialRotation);

        // TO DEBUG
        GetComponent<SpriteRenderer>().color = Color.red;
    }
}