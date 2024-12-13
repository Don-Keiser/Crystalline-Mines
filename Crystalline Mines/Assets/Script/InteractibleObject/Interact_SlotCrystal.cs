using Script.Enigma1;

public class Interact_SlotCrystal : Interactible
{
    private PlayerGrabController _grabController;
    private PuzzleSlotController _slotController;
    private void Start()
    {
        _grabController = PlayerGrabController.Instance;
        _slotController = GetComponent<PuzzleSlotController>();
    }
    public override void PlayerInteract()
    {
        _grabController.SetNearbySlot(_slotController);
        base.PlayerInteract();

        if (_grabController.hasCrystal && _slotController.crystalHere == null /*&& _slotController.correctCrystal == _grabController.holdObject*/)
        {
            _grabController.DropObject();
        }
        else
        {
            _slotController.InteractWithSlot();
            _grabController.PickUpCrystal();
        }
    }
}
