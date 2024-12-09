using UnityEngine;

namespace Script.Enigma1
{
    public class CristalPuzzle : MonoBehaviour
    {
        public GameObject[] emplacementsCorrects;  // Tableau de colliders des emplacements corrects
        private bool _isPlaced = false;  // Pour savoir si le cristal est placé
        private GameManager _gameManager;  // Référence au GameManager pour mettre à jour l'état
        private GrabController _grabController;
        private Rigidbody2D _grabrigidbody2D;
        private bool _playerNearby;

        private void Start()
        {
            _grabController = GameObject.Find("Player").GetComponent<GrabController>();
            _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        }
        void Update()
        {
            if (_grabController.grabbed == true)
            {
                if (Input.GetKey(KeyCode.B))
                {
                    PlaceCristal();
                }
            }

            if (Input.GetKey(KeyCode.P))
            {
                GetCristal();
            }
        }

        private void PlaceCristal()
        {
            if (_grabController.grabedObject != null)
            {
                Debug.Log($"Placement de l'objet : {_grabController.grabedObject.name}");

                _grabrigidbody2D = _grabController.grabedObject.GetComponent<Rigidbody2D>();

                if (_grabrigidbody2D != null)
                {
                    _grabrigidbody2D.simulated = false;
                    _grabrigidbody2D.isKinematic = true;
                    _grabController.grabedObject.transform.position = transform.position;
                }

                // Signale au GameManager que le cristal est placé
                _gameManager.CristalPlacedInPuzzle(_grabController.grabedObject, true);

                // Met à jour les états
                _isPlaced = true;
                _grabController.grabbed = false;
                _grabController.grabedObject = null;
            }
            else
            {
                Debug.LogWarning("Aucun objet n'est saisi !");
            }
        }

        private void GetCristal()
        {
            if (_grabController.grabedObject == null && _isPlaced)
            {
                _grabrigidbody2D = GetComponent<Rigidbody2D>();

                if (_grabrigidbody2D != null)
                {
                    _grabrigidbody2D.simulated = true;
                    _grabrigidbody2D.isKinematic = false;
                }

                // Signale au GameManager que le cristal est retiré
                _gameManager.RemoveCrystal(gameObject);

                // Met à jour les états
                _isPlaced = false;
                _grabController.grabbed = true;
            }
            else
            {
                Debug.LogWarning("Aucun objet n'est saisi ou déjà placé !");
            }
        }
        private void OnTriggerEnter(Collider other)
        {
            Player player = other.GetComponent<Player>();
            if (player)
            {
                _playerNearby = true; // Marquer que le joueur est proche// Afficher le texte
                Debug.Log("Joueur proche");
            }
        }
        private void OnTriggerExit(Collider other)
        {
            Player player = other.GetComponent<Player>();
            if (player)
            {
                _playerNearby = false; // Marquer que le joueur est proche// Afficher le texte
                Debug.Log("Joueur sortie");
            }
        }
    }
}