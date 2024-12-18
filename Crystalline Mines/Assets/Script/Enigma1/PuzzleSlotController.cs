using UnityEngine;

namespace Script.Enigma1
{
    public class PuzzleSlotController : MonoBehaviour
    {
        [Header("Manager References")]
        private FirstEnigmaManager _puzzleManager;

        [Header("Player Interaction")]
        [SerializeField] private PlayerGrabController grabController;
        [SerializeField] public GameObject correctCrystal;

        [Header("Slot State")]
        public bool isOccupied = false;
        private bool playerNearby = false;
        public GameObject crystalHere;

        [Header("Cooldown")]
        private bool _interactionCooldown = false; // Variable to manage cooldown


        void Start()
        {
            _puzzleManager = FirstEnigmaManager.Instance;
            grabController = PlayerGrabController.Instance;
        }
        public bool IsCorrectCrystal()
        {
            return crystalHere == correctCrystal;
        }

        public void PlaceCrystal(GameObject crystal)
        {
            HandleInteractionCooldown(); // Starts the cooldown
            if (isOccupied) return;
            crystal.transform.position = transform.position; // Places the crystal in the slot
            isOccupied = true;
            crystalHere = crystal;
            if (IsCorrectCrystal())
            {
                Debug.Log("The placed crystal is correct!");
            }
            else
            {
                Debug.LogWarning("The placed crystal is not correct.");
            }
            // Informs the PuzzleManager
            _puzzleManager.RegisterCrystal(crystal, true);
        }

        private void SwapCrystal(GameObject holderCrystal, GameObject crystalAlreadyPlaced)
        {
            _puzzleManager.RegisterCrystal(crystalAlreadyPlaced, false);
            crystalHere = holderCrystal;
            holderCrystal.transform.position = transform.position;


            grabController.holdObject = crystalAlreadyPlaced;
            grabController.holdObjectRb = crystalAlreadyPlaced.GetComponent<Rigidbody2D>();
            _puzzleManager.RegisterCrystal(crystalAlreadyPlaced, true);
            //_puzzleManager.CheckPuzzleCompletion();

        }

        public void RemoveCrystal()
        {
            HandleInteractionCooldown(); // Starts the cooldown
            if (!isOccupied) return;

            // Assign the crystal to the PlayerGrabController's holdObject
            grabController.holdObject = crystalHere;
            grabController.hasCrystal = true;
            isOccupied = false;
            crystalHere = null; // Removes the crystal from the slot
            // Informs the PuzzleManager that the crystal has been removed
            _puzzleManager.RegisterCrystal(crystalHere, false);
        }


        public void InteractWithSlot()
        {
            if (_puzzleManager.IsPuzzleCompleted)
            {
                return; // Bloque toute interaction si le puzzle est terminé
            }

            if (grabController == null)
            {
                Debug.LogError("The PlayerGrabController component is missing on the player object.");
                return;
            }

            if (isOccupied && !grabController.hasCrystal) // The slot is occupied and the player wants to pick up
            {
                RemoveCrystal();
                //grabController.PickUpCrystal();
                return;
            }

            if(isOccupied && grabController.hasCrystal)
            {
                SwapCrystal(grabController.holdObject, crystalHere);
            }

            if (!isOccupied && grabController.hasCrystal) // The slot is free and the player wants to place
            {
                PlaceCrystal(grabController.holdObject);
            }
        }
        private void HandleInteractionCooldown()
        {
            _interactionCooldown = true;
            TimerManager.StartTimer(0.5f, () => _interactionCooldown = false);
        }
        private void OnTriggerExit2D(Collider2D other)
        {
            PlayerGrabController.Instance.SetNearbySlot(null);
        }
    }
}
