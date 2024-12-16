using System.Collections.Generic;
using UnityEngine;

namespace Script.Enigma1
{
    public class FirstEnigmaManager : MonoBehaviour
    {
        public static FirstEnigmaManager Instance;
        public bool IsPuzzleCompleted { get; private set; }

        [SerializeField] private DoorHandler.LevelRoom _doorToOpen; 
        
        [Header("Enigma data")]
        [SerializeField] private int _totalCrystals = 5; 
        [SerializeField] private List<PuzzleSlotController> puzzleSlots; // List of puzzle slots

        [Header("Crystal Placed")]
        private int _placedCrystals = 0;
        private List<GameObject> _placedCrystalObjects = new List<GameObject>();

        void Awake()
        {
            if(Instance == null)
                Instance = this;
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
            if (_placedCrystals == _totalCrystals && CheckAllSlots())
            {
                Debug.Log("Puzzle successfully completed!");
                IsPuzzleCompleted = true;
                Player.CanOpenTheDoor = true; //make open door logique 
                
                DoorHandler.Instance.GetDoor(_doorToOpen).OpenDoor(() => true);

            }
            else if (_placedCrystals == _totalCrystals)
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
