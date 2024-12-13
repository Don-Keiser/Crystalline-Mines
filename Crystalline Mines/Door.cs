using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI pickUpText; // Référence au texte
        private GameManager _gameManager;
        private Collider2D _coll;

        void Start()
        {
            if (pickUpText != null)
                pickUpText.gameObject.SetActive(false); // Désactiver le texte au départ
            
            _coll = GetComponent<Collider2D>();
            _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        }

        void Update()
        {
            // Détecter si la touche 'E' est pressée et que le joueur est proche
            if (pickUpText != null && pickUpText.gameObject.activeSelf && Input.GetKeyDown(KeyCode.E))
            {
                PickUp();
            }
        }

        private void PickUp()
        {
			// Faire un truc
			GameObject.e
            // Détruire la clé (et son texte)
            Destroy(gameObject);

            Debug.Log("Il y a : " + _gameManager.keyPieceNumber + " morceaux de clé.");
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            Player player = other.GetComponent<Player>();
            if (player && pickUpText != null)
            {
                pickUpText.gameObject.SetActive(true); // Afficher le texte
                Debug.Log("Texte affiché");
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            Player player = other.GetComponent<Player>();
            if (player && pickUpText != null)
            {
                pickUpText.gameObject.SetActive(false); // Cacher le texte
                Debug.Log("Texte caché");
            }
        }
}
