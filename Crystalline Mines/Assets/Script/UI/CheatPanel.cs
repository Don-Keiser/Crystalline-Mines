using Script.Enigma1;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheatPanel : MonoBehaviour
{
    [Header("enigma manager reference")]
    [SerializeField] private GameObject _manager;
    [SerializeField] private GameObject _secondEnigmaManager;
    [SerializeField] private GameObject _railManager;


    public void OpenTutoDoor(GameObject keys)
    {
        keys.GetComponent<Interact_TutoKey>().PlayerInteract();
        OpenDoorAnim();
    }
    public void OpenFirstEnigma()
    {
        _manager.GetComponent<FirstEnigmaManager>().EnigmaFinish();
        OpenDoorAnim();
    }
    public void OpenSecondDoor()
    {
        _secondEnigmaManager.GetComponent<SolutionCheck>().FinishEnigma();
        OpenDoorAnim();
    }
    public void OpenRailDoor()
    {
        _railManager.GetComponent<RailManager>().FinishEnigma();
        OpenDoorAnim();
    }
    public void OpenSimonDoor()
    {
        _manager.GetComponent<SimonGame>().FinishEnigma();
        OpenDoorAnim();
    }

    private void OpenDoorAnim()
    {

    }
}
