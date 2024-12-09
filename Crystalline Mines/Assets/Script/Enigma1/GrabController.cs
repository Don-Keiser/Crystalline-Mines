using UnityEngine;
using TMPro;

namespace Script.Enigma1
{
    public class GrabController : MonoBehaviour
    {
        public bool grabbed; // Indique si un objet est actuellement saisi
        private RaycastHit2D _hit; // Stocke le résultat du raycast
        public float distance = 2f; // Distance maximale du raycast
        public Transform holdpoint; // Position où placer l'objet saisi
        public float throwforce; // Force de lancer
        public LayerMask notgrabbed; // Layers à ignorer pour la saisie
        private Vector2 _direction = Vector2.right; // Direction du raycast
        public GameObject grabedObject; // Objet actuellement saisi

        void Update()
        {
            UpdateDirection();

            if (Input.GetKeyDown(KeyCode.E))
            {
                if (!grabbed) // Si aucun objet n'est saisi
                {
                    Physics2D.queriesStartInColliders = false;
                    _hit = Physics2D.Raycast(transform.position, _direction, distance);

                    if (_hit.collider != null && _hit.collider.CompareTag("Grabbable"))
                    {
                        grabbed = true;
                        grabedObject = _hit.collider.gameObject; // Stocke l'objet saisi
                    }
                }
                else if (!Physics2D.OverlapPoint(holdpoint.position, notgrabbed)) // Si un objet est saisi
                {
                    grabbed = false;
                    if (grabedObject != null)
                    {
                        // Appliquer une force pour lancer l'objet
                        Rigidbody2D rb = grabedObject.GetComponent<Rigidbody2D>();
                        if (rb != null)
                        {
                            rb.velocity = _direction * throwforce + Vector2.up;
                        }
                        grabedObject = null; // Réinitialise l'objet saisi
                    }
                }
            }

            if (grabbed && grabedObject != null)
            {
                // Place l'objet saisi à la position définie par holdpoint
                grabedObject.transform.position = holdpoint.position;
            }
        }

        private void UpdateDirection()
        {
            float horizontalInput = Input.GetAxisRaw("Horizontal");

            if (horizontalInput < 0)
                _direction = Vector2.left;
            else if (horizontalInput > 0)
                _direction = Vector2.right;
        }

        void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, transform.position + (Vector3)(_direction * distance));
        }
    }
}
