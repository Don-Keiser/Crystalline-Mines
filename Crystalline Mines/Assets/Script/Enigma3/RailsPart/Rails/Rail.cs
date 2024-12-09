using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rail : Interactible
{
    #region Enums
    public enum RailCondition
    {
        NonDamaged,
        Damaged
    }

    #endregion

    #region Variables


    #endregion

    #region Methods

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void PlayerInteract()
    {
        base.PlayerInteract();
    }

    public override void StartSFXAndVFX()
    {
        
    }
    #endregion
}