using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Wagon : MonoBehaviour
{
    [Serializable]
    public struct WaypointsValue
    {
        public Transform waypointTransform;
        public Vector3 newRotation;
        public AnimationCurve speedCurve;
    }

    [Header("Statistics :")]
    [SerializeField] DoorHandler.LevelRoom _doorToOpen;

    [Header("Mouvement statistics :")]
    [SerializeField] float _speed = 2;
    [SerializeField] List<WaypointsValue> _waypointsValue = new();

    List<Vector3> _waypointsPositionAtStart = new();

    void Start()
    {
        // Save the position of the waypoints at start
        for (int i = 0; i < _waypointsValue.Count; i++)
        {
            _waypointsPositionAtStart.Add(_waypointsValue[i].waypointTransform.position);
        }

        StartCoroutine(WagonMotionRoutine());
    }

    public IEnumerator WagonMotionRoutine()
    {
        for (int i = 0; i < _waypointsValue.Count; i++)
        {
            Vector3 startPointPosition = transform.position;
            Vector3 endPointPosition = _waypointsPositionAtStart[i];
            AnimationCurve speedCurve = _waypointsValue[i].speedCurve;

            transform.eulerAngles = _waypointsValue[i].newRotation;

            float totalDistance = Vector3.Distance(startPointPosition, endPointPosition);
            float elapsedTime = 0f;

            while (elapsedTime * _speed < totalDistance)
            {
                // Calculate normalized progress based on elapsed time and curve
                float progress = Mathf.Clamp01(elapsedTime / (totalDistance / _speed));
                float curveSpeedModifier = speedCurve.Evaluate(progress);

                transform.position = Vector3.Lerp(startPointPosition, endPointPosition, progress);

                elapsedTime += Time.deltaTime * curveSpeedModifier;

                yield return new WaitForNextFrameUnit();
            }
        }

        DoorHandler.Instance.GetDoor(_doorToOpen).OpenDoor(() => true);
    }
}