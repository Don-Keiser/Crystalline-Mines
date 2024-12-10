using System.Collections.Generic;
using UnityEngine;

public class RailManager : MonoBehaviour
{
    public static RailManager Instance;

    public List<IRail> rails { get; private set; } = new();

    [Header("External references :")]
    public Transform railParent;
    public Transform railPiecesParent;

    void Awake()
    {
        Instance = Instantiator.ReturnInstance(this, Instantiator.InstanceConflictResolutions.WarningAndPause);
    }

    void Start()
    {
        if (!IsManagerDefinedProperly())
            return;

        rails = GetAllRails();
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

    /// <returns> Return all objects that listens the 'restartAllRailsEvent' event, and that inplements the Interface 'IRail'.</returns>
    List<IRail> GetAllRails()
    {
        List<IRail> rails = new();

        for (int i = 0; i < railParent.childCount; i++)
        {
            if (railParent.GetChild(i).TryGetComponent(out IRail rail))
                rails.Add(rail);
        }

        for (int i = 0; i < railPiecesParent.childCount; i++)
        {
            if (railPiecesParent.GetChild(i).TryGetComponent(out IRail rail))
                rails.Add(rail);
        }

        return rails;
    }

    /// <summary>
    /// Reset all rails in the 'rails' List to their initial state, position, and sprite. </summary>
    public void ResetAllRails()
    {
        for (int i = 0; i < rails.Count; i++)
        {
            rails[i].Reinitialize();
        }
    }
}