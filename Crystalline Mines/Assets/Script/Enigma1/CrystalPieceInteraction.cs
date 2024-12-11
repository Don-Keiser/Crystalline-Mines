using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace Script.Enigma1
{
    public class CrystalPieceInteraction : MonoBehaviour
    {
        private TextMeshProUGUI _interactionText;
        

        void Start()
        {
            _interactionText = GetComponentInChildren<TextMeshProUGUI>();
            if (_interactionText != null) _interactionText.gameObject.SetActive(false);

            
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            // Checks if the colliding object is the player and if interaction is possible
            if (other.GetComponent<PlayerGrabController>() != null)
            {
                if (_interactionText != null) _interactionText.gameObject.SetActive(true);
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.GetComponent<PlayerGrabController>() != null)
            {
                if (_interactionText != null) _interactionText.gameObject.SetActive(false);
            }
        }
    }
}