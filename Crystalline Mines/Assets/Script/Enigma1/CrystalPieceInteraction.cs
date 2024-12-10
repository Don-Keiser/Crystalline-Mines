using TMPro;
using UnityEngine;

namespace Script.Enigma1
{
    public class CrystalPieceInteraction : MonoBehaviour
    {
        private TextMeshProUGUI _interactionText;
        private PlayerGrabController _playerGrabController;

        void Start()
        {
            _interactionText = GetComponentInChildren<TextMeshProUGUI>();
            if (_interactionText != null) _interactionText.gameObject.SetActive(false);

            _playerGrabController = FindObjectOfType<PlayerGrabController>();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            // Vérifie si l'objet en collision est le joueur et si l'interaction est possible
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