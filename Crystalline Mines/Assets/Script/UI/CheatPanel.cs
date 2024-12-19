using Script.Enigma1;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheatPanel : MonoBehaviour
{
    [Header("enigma manager reference")]
    [SerializeField] private FirstEnigmaManager _firstEnigmaManager;
    [SerializeField] private SimonGame _simonManager;
    [SerializeField] private SolutionCheck _secondEnigmaManager;
    [SerializeField] private RailManager _railManager;


    public void OpenTutoDoor(GameObject keys)
    {
        keys.GetComponent<Interact_TutoKey>().PlayerInteract();
        OpenDoorAnim();
    }
    public void OpenFirstEnigma()
    {
        _firstEnigmaManager.EnigmaFinish();
        OpenDoorAnim();
    }
    public void OpenSecondDoor()
    {
        _secondEnigmaManager.FinishEnigma();
        OpenDoorAnim();
    }
    public void OpenRailDoor()
    {
        _railManager.FinishEnigma();
        OpenDoorAnim();
    }
    public void OpenSimonDoor()
    {
        _simonManager.FinishEnigma();
        OpenDoorAnim();
    }

    private void OpenDoorAnim()
    {

    }
}
