using System;
using System.Collections.Generic;
using UnityEngine;

public class RailFormHandler : MonoBehaviour
{
    public enum RailStates
    {
        NonDamaged,
        Damaged
    }

    [Serializable]
    public struct RailSpritesOnGround
    {
        public Sprite NonDamaged;
        public Sprite Damaged;
    }

    [Serializable]
    public struct RailForm
    {
        [Header("Base statistics :")]
        public RailStates railState;

        [Header("Repair statistics :")]
        [Tooltip("This sprite is shown in a bubble only when the player interact with the rail without carrying a rail piece. \n" +
                 "The sprite shown reprensent the zoomed damaged rail.")]
        public Sprite spriteShownWhenInteracted;
        public RailPiecesFormHandler.RailPiecesFormTypes missingRailPieces;
    }

    #region Variables

    public static RailFormHandler Instance;

    [Header("Statistics :")]
    [Space]
    public RailSpritesOnGround railSpritesOnGround = new();

    [Space]
    public List<RailForm> railForms = new();
    #endregion

    #region Methods

    #region Unity methods

    void Awake()
    {
        Instance = Instantiator.ReturnInstance(this, Instantiator.InstanceConflictResolutions.WarningAndPause);
    }
    #endregion

    public RailForm GetRailFormValues(RailStates p_railType, RailPiecesFormHandler.RailPiecesFormTypes p_railPiecesFormType)
    {
        foreach (RailForm railPiecesForm in railForms)
        {
            if (railPiecesForm.railState == p_railType && railPiecesForm.missingRailPieces == p_railPiecesFormType)
                return railPiecesForm;
        }

        // If we do not find the given p_railState and p_railPiecesFormType in railForms then we print out an error, and return an empty RailForm
        Debug.LogError($"ERROR ! The given railState '{p_railType}' and railPiecesFormType '{p_railPiecesFormType}' has not been founded insind railForms.");
    
        return new RailForm();
    }
    #endregion
}