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

    public static RailPiecesFormHandler Instance;

    public List<RailPiecesForm> railPiecesForms = new();

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
        int atStartRailPiecesFormsLenght = railPiecesForms.Count;
        int enumLenght = Enum.GetValues(typeof(RailPiecesFormTypes)).Length;

        if (atStartRailPiecesFormsLenght != enumLenght)
        {
            Debug.LogError(
                "ERROR ! The lenght of the List 'railPiecesForms' is not equal to the size of the 'RailPiecesFormTypes' Enum lenght " +
                $": {atStartRailPiecesFormsLenght} is not equal to {enumLenght}"
            );
        }
    }

    public RailPiecesForm GetRailFormSprites(RailPiecesFormTypes p_railPiecesFormType)
    {
        foreach (RailPiecesForm railPiecesForm in railPiecesForms)
        {
            if (railPiecesForm.railPiecesFormTypes == p_railPiecesFormType)
                return railPiecesForm;
        }

        // If we do not find the given p_railPiecesFormType in railPiecesForms then we print out an error, and return an empty RailPiecesForm
        Debug.LogError($"ERROR ! The given railPiecesFormType '{p_railPiecesFormType}' has not been founded insind railPiecesForms");
        
        return new RailPiecesForm();
    }

    #endregion
}