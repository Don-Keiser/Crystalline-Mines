using System;
using System.Collections;
using UnityEngine;

public class TimerManager : MonoBehaviour
{
    private class TimerHelper : MonoBehaviour { }
    private static TimerHelper _helper;
    private static void EnsureHelperExists()
    {
        if (_helper == null)
        {
            GameObject obj = new GameObject("GlobalTimerHelper");
            _helper = obj.AddComponent<TimerHelper>();
            //UnityEngine.Object.DontDestroyOnLoad(obj); 
        }
    }
    public static void StartTimer(float duration, Action callback)
    {
        EnsureHelperExists();
        _helper.StartCoroutine(TimerCoroutine(duration, callback));
    }

    private static IEnumerator TimerCoroutine(float duration, Action callback)
    {
        yield return new WaitForSeconds(duration);
        callback?.Invoke();
    }
}
