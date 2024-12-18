using System;
using System.Collections.Generic;
using UnityEngine;

public class RailManager : MonoBehaviour
{
    public static RailManager Instance;

    public static Action onAllRailsRepairedEvent;
    public static Action<Rail> onNewReparedRailEvent;
    public static Action<Sprite> onShowDetailedDamagedRailEvent;

    public List<IRail> IRails { get; private set; } = new();

    [Header("External references :")]
    public Transform railParent;
    public Transform railPiecesParent;

    [Header("Door to open :")]
    [SerializeField] DoorHandler.LevelRoom _enigmaRoom;

    Door _enigmaDoor;

    List<Rail> _damagedRails = new();

    void Awake()
    {
        Instance = Instantiator.ReturnInstance(this, Instantiator.InstanceConflictResolutions.WarningAndPause);
    }

    void Start()
    {
        if (!IsManagerDefinedProperly())
            return;

        _enigmaDoor = DoorHandler.Instance.GetDoor(_enigmaRoom);

        IRails = GetAllIRails();
    }

    bool IsManagerDefinedProperly()
    {
        bool isDefinedProperly = true;

        #region Check if rail parents are null

        if (railParent == null)
        {
            Debug.LogError(
                $"ERROR ! The '{nameof(railParent)}' Transform variable is not defined."
            );

            isDefinedProperly = false;
        }

        if (railPiecesParent == null)
        {
            Debug.LogError(
                $"ERROR ! The '{nameof(railPiecesParent)}' Transform variable is not defined."
            );

            isDefinedProperly = false;
        }
        #endregion

        #region Check if rail parent have children

        if (!isDefinedProperly == false && railParent.childCount == 0)
        {
            Debug.LogError(
                $"ERROR ! The '{railParent.name}' GameObject has no children."
            );

            isDefinedProperly = false;
        }

        if (!isDefinedProperly == false && railPiecesParent.childCount == 0)
        {
            Debug.LogError(
                $"ERROR ! The '{railPiecesParent.name}' GameObject has no children."
            );

            isDefinedProperly = false;
        }
        #endregion

        return isDefinedProperly;
    }

    /// <summary> 
    /// Get all objects that listens the 'restartAllRailsEvent' event, and that inplements the Interface 'IRail', and
    /// add all the damaged rail into the '_damagedRails' List </summary>
    /// <returns> Return all objects that listens the 'restartAllRailsEvent' event, and that inplements the Interface 'IRail'.</returns>
    List<IRail> GetAllIRails()
    {
        List<IRail> irails = new();

        // Get all irails
        for (int i = 0; i < railParent.childCount; i++)
        {
            if (railParent.GetChild(i).TryGetComponent(out Rail rail))
            {
                irails.Add(rail);

                // Get damaged irails and add it to the _damagedRails List

                // NOTE :
                // If this part is not in a another method,
                // it's to avoid having to loop through 'railParent', and 'railPiecesParent' childrens another time
                if (rail.railState == RailFormHandler.RailStates.Damaged)
                {
                    _damagedRails.Add(rail);
                }
            }
        }

        // Get all rail pieces
        for (int i = 0; i < railPiecesParent.childCount; i++)
        {
            if (railPiecesParent.GetChild(i).TryGetComponent(out IRail rail))
                irails.Add(rail);
        }

        return irails;
    }

    /// <summary>
    /// Reset all IRails in the 'IRails' List to their initial state, position, and sprite. And resets the egnima door. </summary>
    public void ResetAllRails()
    {
        for (int i = 0; i < IRails.Count; i++)
        {
            IRails[i].Reinitialize();
        }

        IsAllDamagedRailRepared();
    }

    /// <summary>
    /// If we found one rail in '_damagedRails' List that is in Damaged state we return false, 
    /// otherwise we open the wanted door, and return true. </summary>
    /// <returns> Will return true if succeeded. </returns>
    public bool IsAllDamagedRailRepared()
    {   
        for (int i = 0; i < _damagedRails.Count; i++)
        {
            if (_damagedRails[i].railState == RailFormHandler.RailStates.Damaged)
            {
                _enigmaDoor.OpenDoor(() => false);
                return false;
            }
        }

        onAllRailsRepairedEvent?.Invoke();

        _enigmaDoor.OpenDoor(() => true);

        return true;
    }

    [ContextMenu("FinishEnigma")]
    public void FinishEnigma()
    {
        onAllRailsRepairedEvent?.Invoke();

        _enigmaDoor.OpenDoor(() => true);
    }
}