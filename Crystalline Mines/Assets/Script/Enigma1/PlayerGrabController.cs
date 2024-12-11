using UnityEngine;
using UnityEngine.Serialization;

namespace Script.Enigma1
{
    public class PlayerGrabController : MonoBehaviour
    {
        [Header("Cristal")]
        public bool hasCrystal; // Indicates whether a crystal is being held
        public GameObject holdObject;
        private Rigidbody2D _holdObjectRb; // Reference to the Rigidbody2D of the held object
        
        [Header("Raycast")]
        private RaycastHit2D _hit;
        public float grabDistance = 2f;
        public Transform holdPoint; // Position where the grabbed object is held
        public LayerMask interactableLayer;
        private Vector2 _direction = Vector2.right; // Direction of the raycast
        public float forceThrow = 10f;
        private PuzzleSlotController _nearbySlot;
        
        
        void Update()
        {
            UpdateDirection();
    
            if (hasCrystal && holdObject != null)
            {
                // Ensures the position is updated every frame
                holdObject.transform.position = holdPoint.position;
            }

            if (Input.GetKeyDown(KeyCode.E)) // Press 'E' to interact or throw
            {
                if (!hasCrystal)
                    TryGrabObject(); // If the player has no crystal, try grabbing one
                else
                    DropObject(); // Otherwise, drop or throw the crystal
            }
        }


        private void UpdateDirection()
        {
            // Assumes the player uses left/right keys to move
            float horizontalInput = Input.GetAxisRaw("Horizontal");

            if (horizontalInput < 0) // If the player moves left
            {
                _direction = Vector2.left;
            }
            else if (horizontalInput > 0) // If the player moves right
            {
                _direction = Vector2.right;
            }
        }

        public void TryGrabObject()
        {
            Debug.Log("TryGrabObject called");  // To check if the method is invoked
            Physics2D.queriesStartInColliders = false;
            _hit = Physics2D.Raycast(transform.position, _direction, grabDistance, interactableLayer);

            if (_hit.collider != null)
            {
                // Rigidbody2D rb = _hit.collider.GetComponent<Rigidbody2D>();

                if (_hit.collider.CompareTag("Grabbable"))
                {
                    Debug.Log("The object is Grabbable");  // Check if the object is indeed "Grabbable"
                    hasCrystal = true;
                    holdObject = _hit.collider.gameObject;
                    _holdObjectRb = holdObject.GetComponent<Rigidbody2D>();

                    if (_holdObjectRb != null)
                    {
                        _holdObjectRb.isKinematic = true;
                        Debug.Log("Crystal grabbed: " + holdObject.name);  // Displays the name of the grabbed object
                    }
                    else
                    {
                        Debug.LogError("Rigidbody2D of the grabbed object is null.");
                    }
                }
            }
            else
            {
                Debug.Log("No object detected in the ray.");
            }
        }

        public void SetNearbySlot(PuzzleSlotController slot)
        {
            _nearbySlot = slot; // Updates the reference to the nearby slot
        }

        public void DropObject()
        {
            if (holdObject == null)
            {
                Debug.LogError("holdObject is null at the start of DropObject.");
                return;
            }

            if (_nearbySlot != null && !_nearbySlot.isOccupied)
            {
                _holdObjectRb.velocity = Vector2.zero;
                _nearbySlot.PlaceCrystal(holdObject);
            }
            else
            {
                _holdObjectRb.isKinematic = false;
                _holdObjectRb.velocity = Vector2.zero;
                _holdObjectRb.AddForce(_direction * forceThrow, ForceMode2D.Impulse);
                Debug.Log("Crystal thrown.");
            }

            // Resets the state after placing or throwing the object
            hasCrystal = false;
            holdObject = null;
            _holdObjectRb = null;
        }



        public void PickUpCrystal()
        {
            if (holdObject != null)
            {
                hasCrystal = true;
                _holdObjectRb = holdObject.GetComponent<Rigidbody2D>();
                if (_holdObjectRb != null)
                {
                    _holdObjectRb.isKinematic = true;
                    _holdObjectRb.velocity = Vector2.zero;
                    _holdObjectRb.angularVelocity = 0f;
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
