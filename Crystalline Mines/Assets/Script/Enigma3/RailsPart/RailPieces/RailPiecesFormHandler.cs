using System;
using System.Collections.Generic;
using UnityEngine;

public class RailPiecesFormHandler : MonoBehaviour
{
    public enum RailPiecesFormTypes
    {
        NonSpecial,
        Squared,
        LittleSquared,
        Triangled,
        SpikyTriangled,
        Capsuled,
    }

    [Serializable]
    public struct RailPiecesForm
    {
        public RailPiecesFormTypes railPiecesFormTypes;
        public Sprite spriteOnGround;
        public Sprite spriteWhenCarried;
    }

    #region Variables

    public static RailPiecesFormHandler Instance;

    [Header("External references :")]
    public Transform railPiecesParent;

    [Header("Statistics :")]
    [Space]
    public List<RailPiecesForm> railPiecesForms = new();
    #endregion

    #region Methods

    #region Unity methods

    void Awake()
    {
        Instance = Instantiator.ReturnInstance(this, Instantiator.InstanceConflictResolutions.WarningAndPause);
    }

    void Start()
    {
        IsHandlerDefinedProperly();
    }
    #endregion

    void IsHandlerDefinedProperly()
    {
        if (railPiecesParent == null)
        {
            Debug.LogError(
                "ERROR ! The railParent Transform variable is not defined."
            );
        }

        int atStartRailPiecesFormsLenght = railPiecesForms.Count;
        int enumLenght = Enum.GetValues(typeof(RailPiecesFormTypes)).Length;

        if (atStartRailPiecesFormsLenght != enumLenght)
        {
            Debug.LogError(
                "ERROR ! The lenght of the List 'railForms' is not equal to the size of the 'RailPiecesFormTypes' Enum lenght " +
                $": {atStartRailPiecesFormsLenght} is not equal to {enumLenght}."
            );
        }
    }

    public RailPiecesForm GetRailFormValues(RailPiecesFormTypes p_railPiecesFormType)
    {
        foreach (RailPiecesForm railPiecesForm in railPiecesForms)
        {
            if (railPiecesForm.railPiecesFormTypes == p_railPiecesFormType)
                return railPiecesForm;
        }

        // If we do not find the given p_railPiecesFormType in railForms then we print out an error, and return an empty RailPiecesForm
        Debug.LogError($"ERROR ! The given railPiecesFormType '{p_railPiecesFormType}' has not been founded insind railForms.");
        
        return new RailPiecesForm();
    }

    #endregion
}