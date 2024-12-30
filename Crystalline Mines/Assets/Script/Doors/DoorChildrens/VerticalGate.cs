using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class VerticalGate : Door
{
    [Header("Opening animation statistics :")]
    [SerializeField] private Vector3 _openingPositionOffset = new(0, 9, 0);
    [SerializeField] private float _openingDurationInSeconds = 2;

    override protected void PlayOpeningAnimation()
    {
        // Will make the gate go upward
        StartCoroutine(OpeningAnimationCoroutine(_openingPositionOffset, _openingDurationInSeconds));
    }

    override protected void PlayOpeningSFX()
    {
        SoundManager.Instance.PlaySound(SoundManager.Instance.doorSound);
    }

    override protected void ResetDoor()
    {
        gameObject.SetActive(true);

        _transform.SetPositionAndRotation(_initialPosition, _initialRotation);
    }

    IEnumerator OpeningAnimationCoroutine(Vector3 p_openingPositionOffset, float p_openingDurationInSeconds)
    {
        float elapsedTime = 0;

        Vector3 startPosition = _transform.position;
        Vector3 endPosition = startPosition + p_openingPositionOffset;

        // Make progressively the object goes to the endPosition
        while (elapsedTime < p_openingDurationInSeconds)
        {
            // Calculate normalized progress based on elapsed time
            float progress = elapsedTime / p_openingDurationInSeconds;

            transform.position = Vector3.Lerp(startPosition, endPosition, progress);

            elapsedTime += Time.deltaTime;

            yield return new WaitForNextFrameUnit();
        }

        // Ensure the final position is exactly the target position at the end
        _transform.position = endPosition;
    }
}