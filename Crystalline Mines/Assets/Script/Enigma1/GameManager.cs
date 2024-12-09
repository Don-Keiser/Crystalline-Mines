using UnityEngine;
using System.Collections.Generic;  // Nécessaire pour utiliser List

namespace Script.Enigma1
{
    public class GameManager : MonoBehaviour
    {
        public bool puzzleFinished = false;
        public int puzzleCristalCorrect = 0; // Compteur de pièces placées correctement
        public GameObject controller; // Le Controller du player
        public List<GameObject> placedCrystals = new List<GameObject>(); // Liste des cristaux placés

        public void CristalPlacedInPuzzle(GameObject crystal, bool isCorrect)
        {
            if (isCorrect)
            {
                // Ajoute le cristal à la liste
                if (!placedCrystals.Contains(crystal))
                {
                    placedCrystals.Add(crystal);
                }
                puzzleCristalCorrect++;
            }

            // Vérifie si le puzzle est complété
            if (puzzleCristalCorrect == 5)  // Exemple : 5 cristaux corrects
            {
                Debug.Log("Puzzle complété !");
            }
        }

        public void RemoveCrystal(GameObject crystal)
        {
            if (placedCrystals.Contains(crystal))
            {
                placedCrystals.Remove(crystal);
                puzzleCristalCorrect--;
                Debug.Log("Cristal retiré du puzzle !");
            }
        }
    }
}