using UnityEngine;
using UnityEngine.Events;

public static class EventManager
{
    //public static event UnityAction OnLevelLoad;
    //public static event UnityAction OnLevelUnload;
    public static event UnityAction<Vector3, float, float, float> CameraCinematic;
    public static event UnityAction<float, float> OnStartedCameraShake;
    public static event UnityAction WagonCinematic;
    public static event UnityAction<GameObject> ActiveTutoPanel;
    public static event UnityAction<GameObject> DisableTutoPanel;
    public static event UnityAction OnGetTutoKey;
    //public static void LevelLoad()
    //{
    //    OnLevelLoad?.Invoke();
    //}
    //public static void LevelUnload()
    //{
    //    OnLevelUnload?.Invoke();
    //}
    public static void StartCameraAnimation(Vector3 targetPos, float maxDezoom, float fullScreenDuration, float animDuration) // call for launch Camera animation
    {
        CameraCinematic?.Invoke(targetPos, maxDezoom, fullScreenDuration, animDuration);
    }
    public static void StartCameraShake(float duration, float magnitude)
    {
        OnStartedCameraShake?.Invoke(duration, magnitude);
    }
    public static void StartWagonAnimation()
    {
        WagonCinematic?.Invoke();
    }
    public static void StartActiveTutoPanel(GameObject currentObject)
    {
        ActiveTutoPanel?.Invoke(currentObject);
    }
    public static void StartDisableTutoPanel(GameObject currentObject)
    {
        DisableTutoPanel?.Invoke(currentObject);
    }
    public static void PlayerGetTutoKey()
    {
        OnGetTutoKey?.Invoke();
    }
}