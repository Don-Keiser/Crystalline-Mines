using System;
using System.Collections.Generic;
using UnityEngine;
using static DoorHandler;

public class CheckPointHandler : MonoBehaviour
{
    public static CheckPointHandler Instance;

    public Dictionary<LevelRoom, CheckPoint> checkPointDictionary { get; private set; }

    [Header("External references :")]
    [SerializeField] List<CheckPoint> checkPoints = new();

    void Awake()
    {
        Instance = Instantiator.ReturnInstance(this, Instantiator.InstanceConflictResolutions.WarningAndPause);

        checkPointDictionary = SetDictionaryData();
    }

    void Start()
    {
        Treasure.OnPlayerPickupTreasureEvent += SetAllCheckPointStateToPlayerLeaveTheMine;
    }

    bool IsCheckPointsListCorrectlySet()
    {
        int atStartCheckPointListLenght = checkPoints.Count;
        int enumLenght = Enum.GetValues(typeof(LevelRoom)).Length;

        if (atStartCheckPointListLenght != enumLenght)
        {
            Debug.LogError(
                $"ERROR ! The lenght of the List '{nameof(checkPoints)}' is not equal to the size of the '{nameof(LevelRoom)}' Enum lenght " +
                $": {atStartCheckPointListLenght} is not equal to {enumLenght}."
            );

            return false;
        }

        return true;
    }

    Dictionary<LevelRoom, CheckPoint> SetDictionaryData()
    {
        if (!IsCheckPointsListCorrectlySet())
            return null;

        Dictionary<LevelRoom, CheckPoint> checkPointDictionary = new();

        LevelRoom[] levelRoomEnumValues = (LevelRoom[])Enum.GetValues(typeof(LevelRoom));

        for (int i = 0; i < checkPoints.Count; i++)
        {
            checkPointDictionary.Add(levelRoomEnumValues[i], checkPoints[i]);
        }

        return checkPointDictionary;
    }

    public CheckPoint GetCheckPoint(LevelRoom p_levelRoom)
    {
        return checkPointDictionary[p_levelRoom];
    }

    public CheckPoint GetCheckPoint(int p_index)
    {
        #region Securities

        if (p_index < 0)
        {
            Debug.LogError($"ERROR ! The given index '{p_index}' is less then 0.");
            return null;
        }

        if (p_index > checkPoints.Count - 1)
        {
            Debug.LogError($"ERROR ! The given index '{p_index}' is greater then the check point dictionnary size less one.");
            return null;
        }
        #endregion

        return checkPoints[p_index];
    }

    public void SetAllCheckPointState(CheckPoint.CheckPointState p_newCheckPointState)
    {
        for (int i = 0; i < checkPoints.Count; i++)
        {
            checkPoints[i].SetNewState(p_newCheckPointState);
        }
    }

    void SetAllCheckPointStateToPlayerLeaveTheMine()
    {
        SetAllCheckPointState(CheckPoint.CheckPointState.PlayerLeaveTheMine);
    }
}
