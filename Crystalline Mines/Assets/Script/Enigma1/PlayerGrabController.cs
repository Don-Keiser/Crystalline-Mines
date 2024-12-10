using UnityEngine;
using UnityEngine.Serialization;

namespace Script.Enigma1
{
    public class PlayerGrabController : MonoBehaviour
    {
        public bool hasCrystal; // Indique si un cristal est saisi
        private RaycastHit2D _hit;
        public float grabDistance = 2f;
        public Transform holdPoint; // Position où placer l'objet saisi
        public LayerMask interactableLayer;
        private Vector2 _direction = Vector2.right; // Direction du raycast
        public GameObject holdObject;
        public Rigidbody2D holdObjectRb; // Référence au Rigidbody2D de l'objet saisi
        private PuzzleSlotController _puzzleSlotController;
        public float forceThrow = 10f;
        private PuzzleSlotController _nearbySlot;

        void Start()
        {
            _puzzleSlotController = GetComponent<PuzzleSlotController>();
        }
        void Update()
        {
            UpdateDirection();
    
            if (hasCrystal && holdObject != null)
            {
                // Assure-toi que la position est mise à jour à chaque frame
                holdObject.transform.position = holdPoint.position;
            }

            if (Input.GetKeyDown(KeyCode.E)) // Appuyez sur 'E' pour interagir ou jeter
            {
                if (!hasCrystal)
                    TryGrabObject(); // Si le joueur n'a pas de cristal, essaye de le saisir
                else
                    DropObject(); // Sinon, lâche ou jette le cristal
            }
        }


        private void UpdateDirection()
        {
            // Suppose que le joueur utilise les touches gauche/droite pour se déplacer
            float horizontalInput = Input.GetAxisRaw("Horizontal");

            if (horizontalInput < 0) // Si le joueur va à gauche
            {
                _direction = Vector2.left;
            }
            else if (horizontalInput > 0) // Si le joueur va à droite
            {
                _direction = Vector2.right;
            }
        }

        public void TryGrabObject()
        {
            Debug.Log("TryGrabObject appelé");  // Pour vérifier si la méthode est appelée
            Physics2D.queriesStartInColliders = false;
            _hit = Physics2D.Raycast(transform.position, _direction, grabDistance, interactableLayer);

            if (_hit.collider != null)
            {
                Rigidbody2D rb = _hit.collider.GetComponent<Rigidbody2D>();

                if (_hit.collider.CompareTag("Grabbable"))
                {
                    Debug.Log("L'objet est Grabbable");  // Vérifie si l'objet est bien "Grabbable"
                    hasCrystal = true;
                    holdObject = _hit.collider.gameObject;
                    holdObjectRb = holdObject.GetComponent<Rigidbody2D>();

                    if (holdObjectRb != null)
                    {
                        holdObjectRb.isKinematic = true;
                        Debug.Log("Cristal saisi : " + holdObject.name);  // Affiche le nom de l'objet saisi
                    }
                    else
                    {
                        Debug.LogError("Rigidbody2D de l'objet saisi est null.");
                    }
                }
            }
            else
            {
                Debug.Log("Aucun objet détecté dans le rayon.");
            }
        }

        public void SetNearbySlot(PuzzleSlotController slot)
        {
            _nearbySlot = slot; // Met à jour la référence au slot proche
        }

        public void DropObject()
        {
            if (holdObject == null)
            {
                Debug.LogError("holdObject est null au début de DropObject.");
                return;
            }

            if (_nearbySlot != null && !_nearbySlot.isOccupied)
            {
                _nearbySlot.PlaceCrystal(holdObject);
            }
            else
            {
                holdObjectRb.isKinematic = false;
                holdObjectRb.velocity = Vector2.zero;
                holdObjectRb.AddForce(_direction * forceThrow, ForceMode2D.Impulse);
                Debug.Log("Cristal jeté.");
            }

            // Réinitialise l'état après avoir placé ou jeté l'objet
            hasCrystal = false;
            holdObject = null;
            holdObjectRb = null;
        }



        public void PickUpCrystal()
        {
            if (holdObject != null)
            {
                hasCrystal = true;
                holdObjectRb = holdObject.GetComponent<Rigidbody2D>();
                if (holdObjectRb != null)
                {
                    holdObjectRb.isKinematic = true;
                    holdObjectRb.velocity = Vector2.zero;
                    holdObjectRb.angularVelocity = 0f;
                }
            }
        }



        void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, transform.position + (Vector3)(_direction * grabDistance));
        }
    }
}
