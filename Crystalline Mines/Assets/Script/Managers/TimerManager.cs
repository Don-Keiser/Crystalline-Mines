using System;
using System.Collections;
using System.Data.Common;
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
            DontDestroyOnLoad(obj); 
        }
    }


    #region Timer with parameter
    public static void StartTimer<T>(float duration, Action<T> callback, T parameter) //call the timer with a parameter function
    {
        EnsureHelperExists();
        _helper.StartCoroutine(TimerCoroutineWithParam(duration, callback, parameter));
    }

    private static IEnumerator TimerCoroutineWithParam<T>(float duration, Action<T> callback, T parameter)
    {
        yield return new WaitForSeconds(duration);
        callback?.Invoke(parameter);
    }
    #endregion

    #region Timer without parameter
    public static void StartTimer(float duration, Action callback) //call the timer without a parameter function
    {
        EnsureHelperExists();
        _helper.StartCoroutine(TimerCoroutineWithoutParam(duration, callback));
    }
    private static IEnumerator TimerCoroutineWithoutParam(float duration, Action callback)
    {
        yield return new WaitForSeconds(duration);
        callback?.Invoke();
    }
    #endregion
}
