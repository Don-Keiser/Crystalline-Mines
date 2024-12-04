using TMPro;
using Unity.VisualScripting;
using UnityEngine;

namespace Script.EnigmaKey
{
    public class KeyPiece : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI pickUpText;
        private GameManager _gameManager;
        private Collider2D _coll;
        // [SerializeField] private Animator keyPieceAnimator;
        
        void Start()
        {
            pickUpText.gameObject.SetActive(false);
            _coll = GetComponent<Collider2D>();
            _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                PickUp();
            }
        }

        private void PickUp()
        {
            _gameManager.keyPieceNumber++;
            Destroy(gameObject);
            Debug.Log("Il y a : " + _gameManager.keyPieceNumber + " de morceaux de pièces");
        }
        private void OnTriggerEnter2D(Collider2D other)
        {
            Player player = other.GetComponent<Player>();
            if (player)
            {
                pickUpText.gameObject.SetActive(true);
                Debug.Log("Show");
            }
            // Debug.Log("Marche po");
        }
        private void OnTriggerExit2D(Collider2D other)
        {
            pickUpText.gameObject.SetActive(false);
            Debug.Log("Hide");
        }
    }
}
