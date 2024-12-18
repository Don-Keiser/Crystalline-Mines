using System;
using System.Collections.Generic;
using UnityEngine;

public class DoorHandler : MonoBehaviour
{
    public enum LevelRoom
    {
        Turorial,
        Level1,
        Level2,
        Level3Part1,
        Level3Part2,
    }

    public static DoorHandler Instance;

    public Dictionary<LevelRoom, Door> doorsDictionary { get; private set; }

    [Header("External references :")]
    [SerializeField] List<Door> doors = new();

    void Awake()
    {
        Instance = Instantiator.ReturnInstance(this, Instantiator.InstanceConflictResolutions.WarningAndPause);

        doorsDictionary = SetDictionaryData();
    }

    bool IsDoorsListCorrectlySet()
    {
        int atStartDoorsListLenght = doors.Count;
        int enumLenght = Enum.GetValues(typeof(LevelRoom)).Length;

        if (atStartDoorsListLenght != enumLenght)
        {
            Debug.LogError(
                $"ERROR ! The lenght of the List '{nameof(doors)}' is not equal to the size of the '{nameof(LevelRoom)}' Enum lenght " +
                $": {atStartDoorsListLenght} is not equal to {enumLenght}."
            );

            return false;
        }

        return true;
    }

    Dictionary<LevelRoom, Door> SetDictionaryData()
    {
        if (!IsDoorsListCorrectlySet())
            return null;

        Dictionary<LevelRoom, Door> doorsDictionary = new();

        LevelRoom[] doormRoomEnumValues = (LevelRoom[])Enum.GetValues(typeof(LevelRoom));

        for (int i = 0; i < doors.Count; i++)
        {
            doorsDictionary.Add(doormRoomEnumValues[i], doors[i]);
        }

        return doorsDictionary;
    }

    public Door GetDoor(LevelRoom p_doorRoom)
    {
        return doorsDictionary[p_doorRoom];
    }
}