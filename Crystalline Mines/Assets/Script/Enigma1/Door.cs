// using TMPro;
// using UnityEngine;
//
// namespace Script.Enigma1
// {
//     public class Door : MonoBehaviour
//     {
//         [SerializeField] private TextMeshProUGUI enterDoorText; // Texte à afficher pour interagir
//         private PuzzleManager _gameManager;
//         private bool _playerNearby; // Indique si le joueur est proche de la porte
//
//         void Start()
//         {
//             if (enterDoorText != null)
//                 enterDoorText.gameObject.SetActive(false); // Désactiver le texte au départ
//
//             _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
//         }
//
//         void Update()
//         {
//             // Vérifier si le joueur peut interagir avec la porte
//             if (_playerNearby && Input.GetKeyDown(KeyCode.E) && _gameManager.puzzleCristalCorrect == 5 && !_gameManager.puzzleFinished)
//             {
//                 EnterDoor();
//             }
//         }
//
//         private void EnterDoor()
//         {
//             Debug.Log("Porte Ouverte !");
//             // Ajoutez ici la logique pour entrer dans la porte (ex : changer de scène)
//         }
//
//         private void OnTriggerEnter2D(Collider2D other)
//         {
//             Player player = other.GetComponent<Player>();
//             if (player)
//             {
//                 _playerNearby = true;
//                 UpdateDoorTextVisibility(); // Met à jour la visibilité du texte
//             }
//         }
//
//         private void OnTriggerExit2D(Collider2D other)
//         {
//             Player player = other.GetComponent<Player>();
//             if (player)
//             {
//                 _playerNearby = false;
//                 UpdateDoorTextVisibility(); // Masque le texte lorsque le joueur s'éloigne
//             }
//         }
//
//         // Méthode pour gérer la visibilité du texte
//         private void UpdateDoorTextVisibility()
//         {
//             if (enterDoorText != null)
//             {
//                 // Affiche le texte uniquement si le joueur est proche et que le puzzle est terminé
//                 if (_playerNearby && _gameManager.puzzleCristalCorrect == 5 && !_gameManager.puzzleFinished)
//                 {
//                     enterDoorText.gameObject.SetActive(true);
//                 }
//                 else
//                 {
//                     enterDoorText.gameObject.SetActive(false);
//                 }
//             }
//         }
//     }
// }
