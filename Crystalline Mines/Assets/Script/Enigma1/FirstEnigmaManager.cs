using System.Collections.Generic;
using UnityEngine;

namespace Script.Enigma1
{
    public class FirstEnigmaManager : MonoBehaviour
    {
        public static FirstEnigmaManager Instance;
        public int totalCrystals = 5; 
        private int _placedCrystals = 0;

        private List<GameObject> _placedCrystalObjects = new List<GameObject>();
        [SerializeField] private List<PuzzleSlotController> puzzleSlots; // List of puzzle slots
        public bool IsPuzzleCompleted { get; private set; } = false;

        void Awake()
        {
            if(Instance == null)
                Instance = this;
            //// Automatically finds all slots with the PuzzleSlotController script in the scene
            //puzzleSlots = new List<PuzzleSlotController>(FindObjectsOfType<PuzzleSlotController>());
        }

        
        public void RegisterCrystal(GameObject crystal, bool isPlaced)
        {
            if (isPlaced)
            {
                if (!_placedCrystalObjects.Contains(crystal))
                {
                    _placedCrystalObjects.Add(crystal);
                    _placedCrystals++;
                }
            }
            else
            {
                if (_placedCrystalObjects.Contains(crystal))
                {
                    _placedCrystalObjects.Remove(crystal);
                    _placedCrystals--;
                }
            }

            CheckPuzzleCompletion();
        }

        private void CheckPuzzleCompletion()
        {
            if (_placedCrystals == totalCrystals && CheckAllSlots())
            {
                Debug.Log("Puzzle successfully completed!");
                IsPuzzleCompleted = true;
                Player.CanOpenTheDoor = true; //make open door logique 

                //var allCrystalPieceInteractions = FindObjectsOfType<CrystalPieceInteraction>();
                //foreach (var crystalPieceInteraction in allCrystalPieceInteractions)
                //{
                //    var canvas = crystalPieceInteraction.GetComponentInChildren<Canvas>();
                //    if (canvas != null)
                //    {
                //        canvas.gameObject.SetActive(false);
                //    }
                //}
            }
            else if (_placedCrystals == totalCrystals)
            {
                Debug.Log("All crystals are placed, but some are incorrect.");
            }
        }

        public bool CheckAllSlots()
        {
            foreach (var slot in puzzleSlots)
            {
                if (!slot.IsCorrectCrystal())
                {
                    return false; // At least one slot has an incorrect crystal
                }
            }
            return true; // All slots have the correct crystals
        }
    }
}
