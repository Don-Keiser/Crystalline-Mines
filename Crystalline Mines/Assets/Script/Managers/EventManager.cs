using UnityEngine;
using UnityEngine.Events;

public static class EventManager
{
    //public static event UnityAction OnLevelLoad;
    //public static event UnityAction OnLevelUnload;
    public static event UnityAction<Vector3, float> CameraCinematic;
    public static event UnityAction WagonCinematic;
    //public static void LevelLoad()
    //{
    //    OnLevelLoad?.Invoke();
    //}
    //public static void LevelUnload()
    //{
    //    OnLevelUnload?.Invoke();
    //}
    public static void StartCameraAnimation(Vector3 targetPos, float maxDezoom) // call for launch Camera animation
    {
        CameraCinematic?.Invoke(targetPos, maxDezoom);
    }
    public static void StartWagonAnimation()
    {
        WagonCinematic?.Invoke();
    }
}
