using System.Collections;
using UnityEngine;

public class ForceApplicator : MonoBehaviour
{
    // Empty class, used to call static coroutine like MoveToTarget
    class ForceApplicatorExecutor : MonoBehaviour { }

    static ForceApplicatorExecutor _forceApplicatorExecutor;

    static void EnsureForceApplicatorExecutorExists()
    {
        if (_forceApplicatorExecutor == null)
        {
            GameObject forceApplicatorExecutorGameObject = new("ForceApplicatorExecutor");

            _forceApplicatorExecutor = forceApplicatorExecutorGameObject.AddComponent<ForceApplicatorExecutor>();

            DontDestroyOnLoad(forceApplicatorExecutorGameObject);
        }
    }

    public static void ApplyImpulse(Transform p_transformToMove,
        Vector3 p_impulsedirection, float p_impulsePower = 1f, float p_impulseDurationInSeconds = 1f)
    {
        EnsureForceApplicatorExecutorExists();

        Vector3 startPosition = p_transformToMove.position;
        Vector3 endPosition = startPosition + p_impulsedirection.normalized * p_impulsePower;
        
        _forceApplicatorExecutor.StartCoroutine(MoveToTarget(p_transformToMove, startPosition, endPosition, p_impulseDurationInSeconds));
    }

    static IEnumerator MoveToTarget(
        Transform p_transformToMove,
        Vector3 p_startPosition, Vector3 p_endPosition,
        float p_impulseDurationInSeconds)
    {
        float timer = 0f;

        while (timer < p_impulseDurationInSeconds)
        {
            p_transformToMove.position = Vector3.Lerp(p_startPosition, p_endPosition, timer / p_impulseDurationInSeconds);

            timer += Time.deltaTime;

            // Wait the next frame
            yield return null;
        }

        p_transformToMove.position = p_endPosition;
    }
}