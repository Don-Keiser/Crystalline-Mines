using System.Collections.Generic;
using UnityEngine;

namespace Script.Enigma1
{
    public class PuzzleManager : MonoBehaviour
    {
        public int totalCrystals = 5; 
        private int _placedCrystals = 0;
        public List<GameObject> placedCrystalObjects = new List<GameObject>();

        public void RegisterCrystal(GameObject crystal, bool isPlaced)
        {
            if (isPlaced)
            {
                if (!placedCrystalObjects.Contains(crystal))
                {
                    placedCrystalObjects.Add(crystal);
                    _placedCrystals++;
                }
            }
            else
            {
                if (placedCrystalObjects.Contains(crystal))
                {
                    placedCrystalObjects.Remove(crystal);
                    _placedCrystals--;
                }
            }

            CheckPuzzleCompletion();
        }

        private void CheckPuzzleCompletion()
        {
            if (_placedCrystals == totalCrystals)
            {
                Debug.Log("Puzzle complété !");
            }
        }
    }
}