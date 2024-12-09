using TMPro;
using UnityEngine;

namespace Script.Enigma1
{
    public class CristalPiece : MonoBehaviour
    {
        private TextMeshProUGUI _pickUpText; // Texte à afficher pour ramasser la clé
        private GameManager _gameManager;
        private bool _playerNearby; // Indique si le joueur est proche de la clé
        private GrabController _grabController;

        void Start()
        {
            // Récupérer automatiquement le texte si non assigné dans l'inspecteur
            if (_pickUpText == null)
            {
                // Rechercher dans les enfants du prefab
                _pickUpText = GetComponentInChildren<TextMeshProUGUI>();

                if (_pickUpText == null)
                {
                    Debug.LogError("Aucun TextMeshProUGUI trouvé dans les enfants du prefab !");
                }
            }

            // Désactiver le texte au départ
            if (_pickUpText != null)
                _pickUpText.gameObject.SetActive(false);

            // Récupérer les autres composants nécessaires
            _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
            _grabController = GameObject.Find("Player").GetComponent<GrabController>();
        }

        void Update()
        {
            // Si le joueur est proche et appuie sur 'E'
            if (_playerNearby && Input.GetKeyDown(KeyCode.E))
            {
                PickUp();
            }
        }

        private void PickUp()
        {
            Debug.Log($"Cristal Ramassé ! Nombre total de morceaux : {_gameManager.puzzleCristalCorrect}");
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            Player player = other.GetComponent<Player>();
            if (player && _grabController.grabbed == false)
            {
                _playerNearby = true; // Marquer que le joueur est proche
                if (_pickUpText != null)
                    _pickUpText.gameObject.SetActive(true); // Afficher le texte

                Debug.Log("Texte affiché");
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            Player player = other.GetComponent<Player>();
            if (player)
            {
                _playerNearby = false; // Marquer que le joueur est parti
                if (_pickUpText != null)
                    _pickUpText.gameObject.SetActive(false); // Cacher le texte

                Debug.Log("Texte caché");
            }
        }
    }
}
