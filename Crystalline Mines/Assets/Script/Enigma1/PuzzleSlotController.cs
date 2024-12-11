using UnityEngine;
using System.Collections;

namespace Script.Enigma1
{
    public class PuzzleSlotController : MonoBehaviour
    {
        private PuzzleManager _puzzleManager;
        public bool isOccupied = false;
        [SerializeField] private bool playerNearby = false;
        [SerializeField] private PlayerGrabController grabController;
        [SerializeField] private GameObject player;
        [SerializeField] private GameObject correctCrystal;
        public GameObject crystalHere;

        private bool _interactionCooldown = false; // Variable to manage cooldown

        void Start()
        {
            _puzzleManager = FindObjectOfType<PuzzleManager>();
            player = GameObject.FindGameObjectWithTag("Player");
            grabController = player.GetComponent<PlayerGrabController>();
        }

        private void Update()
        {
            if (playerNearby && Input.GetKeyDown(KeyCode.E) && !_interactionCooldown)
            {
                InteractWithSlot();
            }
        }
        public bool IsCorrectCrystal()
        {
            return crystalHere == correctCrystal;
        }
        
        public void PlaceCrystal(GameObject crystal)
        {
            StartCoroutine(HandleInteractionCooldown()); // Starts the cooldown
            if (isOccupied) return;
            crystal.transform.position = transform.position; // Places the crystal in the slot
            isOccupied = true;
            crystalHere = crystal;

            Debug.Log("Crystal placed!");
    
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


        public void RemoveCrystal()
        {
            StartCoroutine(HandleInteractionCooldown()); // Starts the cooldown
            if (!isOccupied) return;

            // Assign the crystal to the PlayerGrabController's holdObject
            grabController.holdObject = crystalHere;
            grabController.hasCrystal = true;
            isOccupied = false;
            crystalHere = null; // Removes the crystal from the slot
            Debug.Log("Crystal removed!");
            // Informs the PuzzleManager that the crystal has been removed
            _puzzleManager.RegisterCrystal(crystalHere, false);
        }


        public void InteractWithSlot()
        {
            if (_puzzleManager.IsPuzzleCompleted)
            {
                Debug.Log("Puzzle is completed. Interaction is disabled.");
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
                grabController.PickUpCrystal();
                Debug.Log("Removed");
                return;
            }

            if (!isOccupied && grabController.hasCrystal) // The slot is free and the player wants to place
            {
                Debug.Log("Placed");
                PlaceCrystal(grabController.holdObject);
            }
        }


        private IEnumerator HandleInteractionCooldown()
        {
            _interactionCooldown = true;
            yield return new WaitForSeconds(0.5f); // Cooldown duration
            _interactionCooldown = false;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            PlayerGrabController playerGrabController = other.GetComponent<PlayerGrabController>();
            if (playerGrabController != null)
            {
                playerGrabController.SetNearbySlot(this); // Notifies the player that they are near this slot
                playerNearby = true;
                // Debug.Log("Player near the slot: " + gameObject.name);
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            PlayerGrabController playerGrabController = other.GetComponent<PlayerGrabController>();
            if (playerGrabController != null)
            {
                playerGrabController.SetNearbySlot(null); // Resets the reference to the slot
                playerNearby = false;
                // Debug.Log("Player away from the slot: " + gameObject.name);
            }
        }
    }
}