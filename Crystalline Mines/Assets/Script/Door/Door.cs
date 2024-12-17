using System;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer), typeof(BoxCollider2D))]
public class Door : MonoBehaviour
{
    [Header("External references :")]
    [SerializeField] Animator animator;

    Transform _transform;

    Vector3 _initialPosition;
    Quaternion _initialRotation;

    bool _isOpen;

    void Start()
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

    void PlayOpeningAnimation()
    {
        // TEMPORARY : TO DEBUG
        GetComponent<SpriteRenderer>().color = Color.green;
        TimerManager.StartTimer(3.0f, () => Destroy(gameObject));
        //animator.Play();
    }

    void PlayOpeningSFX()
    {

    }

    void ResetDoor()
    {
        _transform.position = _initialPosition;
        _transform.rotation = _initialRotation;

        // TEMPORARY : TO DEBUG
        GetComponent<SpriteRenderer>().color = Color.red;
    }
}