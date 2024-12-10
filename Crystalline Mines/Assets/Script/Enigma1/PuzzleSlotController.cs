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
        [SerializeField] private GameObject _player;
        public GameObject crystalHere;

        private bool interactionCooldown = false; // Variable pour gérer le cooldown

        void Start()
        {
            _puzzleManager = FindObjectOfType<PuzzleManager>();
            _player = GameObject.FindGameObjectWithTag("Player");
            grabController = _player.GetComponent<PlayerGrabController>();
        }

        private void Update()
        {
            if (playerNearby && Input.GetKeyDown(KeyCode.E) && !interactionCooldown)
            {
                InteractWithSlot();
            }
        }

        public void PlaceCrystal(GameObject crystal)
        {
            StartCoroutine(HandleInteractionCooldown()); // Démarre le cooldown
            
            if (isOccupied) return;
            crystal.transform.position = transform.position; // Place le cristal dans le slot
            isOccupied = true;
            crystalHere = crystal;
            // crystalrb = grabController.holdObjectRb;
            Debug.Log("Cristal Placed !!!");
            // Informe PuzzleManager que le cristal a été placé
            _puzzleManager.RegisterCrystal(crystal, true);
        }

        public void RemoveCrystal(GameObject crystal)
        {
            StartCoroutine(HandleInteractionCooldown()); // Démarre le cooldown
            if (!isOccupied) return;

            // Assigner le cristal à holdObject du PlayerGrabController
            grabController.holdObject = crystalHere;
            grabController.hasCrystal = true;
            isOccupied = false;
            crystalHere = null; // Retirer le cristal du slot
            Debug.Log("Cristal Removed !!!");
            // Informe PuzzleManager que le cristal a été retiré
            _puzzleManager.RegisterCrystal(crystal, false);
        }


        public void InteractWithSlot()
        {
            if (grabController == null)
            {
                Debug.LogError("Le composant PlayerGrabController est introuvable sur l'objet joueur.");
                return;
            }
            if (isOccupied && !grabController.hasCrystal) // Le slot est occupé et le joueur veut ramasser
            {
                RemoveCrystal(grabController.holdObject);
                grabController.PickUpCrystal();
                Debug.Log("Removed");
                return;
            }
            
            if (!isOccupied && grabController.hasCrystal) // Le slot est libre et le joueur veut placer
            {
                Debug.Log("Placed");
                PlaceCrystal(grabController.holdObject);
                // grabController.DropObject();
                return;
            }
        }

        private IEnumerator HandleInteractionCooldown()
        {
            interactionCooldown = true;
            yield return new WaitForSeconds(0.5f); // Durée du cooldown
            interactionCooldown = false;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            PlayerGrabController grabController = other.GetComponent<PlayerGrabController>();
            if (grabController != null)
            {
                grabController.SetNearbySlot(this); // Indique au joueur qu'il est proche de ce slot
                playerNearby = true;
                // Debug.Log("Joueur à proximité du slot : " + gameObject.name);
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            PlayerGrabController grabController = other.GetComponent<PlayerGrabController>();
            if (grabController != null)
            {
                grabController.SetNearbySlot(null); // Réinitialise la référence au slot
                playerNearby = false;
                // Debug.Log("Joueur éloigné du slot : " + gameObject.name);
            }
        }
    }
}
