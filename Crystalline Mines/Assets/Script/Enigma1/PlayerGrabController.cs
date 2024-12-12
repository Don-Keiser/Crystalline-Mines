using UnityEngine;

namespace Script.Enigma1
{
    public class PlayerGrabController : MonoBehaviour
    {
        public static PlayerGrabController Instance;

        [Header("Cristal")]
        public bool hasCrystal; // Indicates whether a crystal is being held
        public GameObject holdObject;
        public Rigidbody2D holdObjectRb;

        public PuzzleSlotController _nearbySlot { get; private set; }

        [SerializeField] private LayerMask InteractibleMask;
        private void Awake()
        {
            if(Instance is null) { Instance = this; }
        }
        void Update()
        {
            //UpdateDirection();

            if (hasCrystal && holdObject != null)
            {
                holdObject.transform.position = Player.PlayerTransform.position + new Vector3(0, 1.5f, 0);
            }

            //if (Input.GetKeyDown(KeyCode.E)) // Press 'E' to interact or throw
            //{
            //    if (!hasCrystal)
            //        TryGrabObject(); // If the player has no crystal, try grabbing one
            //    else
            //        DropObject(); // Otherwise, drop or throw the crystal
            //}
        }


        //private void UpdateDirection()
        //{
        //    // Assumes the player uses left/right keys to move
        //    float horizontalInput = Input.GetAxisRaw("Horizontal");

        //    if (horizontalInput < 0) // If the player moves left
        //    {
        //        _direction = Vector2.left;
        //    }
        //    else if (horizontalInput > 0) // If the player moves right
        //    {
        //        _direction = Vector2.right;
        //    }
        //}

        //public void TryGrabObject()
        //{
        //    Debug.Log("TryGrabObject called");  // To check if the method is invoked
        //    Physics2D.queriesStartInColliders = false;
        //    _hit = Physics2D.Raycast(transform.position, _direction, grabDistance, interactableLayer);

        //    if (_hit.collider != null)
        //    {
        //        // Rigidbody2D rb = _hit.collider.GetComponent<Rigidbody2D>();

        //        if (_hit.collider.CompareTag("Grabbable"))
        //        {
        //            Debug.Log("The object is Grabbable");  // Check if the object is indeed "Grabbable"
        //            hasCrystal = true;
        //            holdObject = _hit.collider.gameObject;
        //            _holdObjectRb = holdObject.GetComponent<Rigidbody2D>();

        //            if (_holdObjectRb != null)
        //            {
        //                _holdObjectRb.isKinematic = true;
        //                Debug.Log("Crystal grabbed: " + holdObject.name);  // Displays the name of the grabbed object
        //            }
        //            else
        //            {
        //                Debug.LogError("Rigidbody2D of the grabbed object is null.");
        //            }
        //        }
        //    }
        //    else
        //    {
        //        Debug.Log("No object detected in the ray.");
        //    }
        //}

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
                holdObjectRb.velocity = Vector2.zero;
                _nearbySlot.PlaceCrystal(holdObject);
            }
            else
            {
                holdObjectRb.isKinematic = false;
                holdObjectRb.velocity = Vector2.zero;

                Vector2 direction = Vector2.left; //correct the direction to suit the player 

                holdObjectRb.AddForce(direction * 10.0f, ForceMode2D.Impulse);

                int LayerInteractible = LayerMask.NameToLayer("Interactible");
                holdObject.layer = LayerInteractible;

                Debug.Log("Crystal thrown.");
            }

            // Resets the state after placing or throwing the object
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
    }
}
