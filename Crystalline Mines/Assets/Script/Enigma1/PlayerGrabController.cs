using UnityEngine;

namespace Script.Enigma1
{
    public class PlayerGrabController : MonoBehaviour
    {
        public static PlayerGrabController Instance;
        public PuzzleSlotController _nearbySlot { get; private set; }

        [Header("Cristal Carried")]
        public bool hasCrystal; // Indicates whether a crystal is being held
        public GameObject holdObject;
        public Rigidbody2D holdObjectRb;

        [Header("Player Launching Direction")]
        private Vector2 direction;
        private Vector2 lastDirection;
        private void Awake()
        {
            if(Instance is null) { Instance = this; }
        }
        void Update()
        {
            if (hasCrystal && holdObject != null)
            {
                holdObject.transform.position = Player.PlayerTransform.position + new Vector3(0, 1.5f, 0);
                direction = GetDirection();
            }
        }

        private Vector2 GetDirection()
        {
            float velocityX = Input.GetAxisRaw("Horizontal");

            if (velocityX != 0)
                return lastDirection = velocityX < 0 ? Vector2.left : Vector2.right;

            return lastDirection;
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
                holdObjectRb.velocity = Vector2.zero;
                _nearbySlot.PlaceCrystal(holdObject);
            }
            else
            {
                holdObjectRb.isKinematic = false;
                holdObjectRb.velocity = Vector2.zero;

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
