using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Script.Enigma1
{
    public class FirstEnigmaManager : MonoBehaviour
    {
        public static FirstEnigmaManager Instance;
        public bool IsPuzzleCompleted { get; private set; }

        [SerializeField] private DoorHandler.LevelRoom _doorToOpen;

        [SerializeField] private float _additionalRadius;
        [SerializeField] private bool _animFinish;

        [Header("Enigma data")]
        [SerializeField] private int _totalCrystals = 5;
        [SerializeField] private List<PuzzleSlotController> puzzleSlots; // List of puzzle slots

        [Header("Crystal Placed")]
        private int _placedCrystals = 0;
        private List<GameObject> _placedCrystalObjects = new List<GameObject>();
        [SerializeField] private List<GameObject> DEBUGCRSITAL = new List<GameObject>();

        void Awake()
        {
            if (Instance == null)
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

        [ContextMenu("EnigmaFinish")]
        public void EnigmaFinish()
        {
            Debug.Log("Puzzle successfully completed!");
            IsPuzzleCompleted = true;


            foreach (var cristal in DEBUGCRSITAL)
            {
                if (cristal.GetComponent<Light2D>() != null)
                {
                    StartCoroutine(FinishEnigmaAnim(cristal.GetComponent<Light2D>()));
                }
            }
            TimerManager.StartTimer(3.0f, () => DoorHandler.Instance.GetDoor(_doorToOpen).OpenDoor(() => true));
        }
        private void CheckPuzzleCompletion()
        {
            if (_placedCrystals == _totalCrystals && CheckAllSlots())
            {
                Debug.Log("Puzzle successfully completed!");
                IsPuzzleCompleted = true;
                foreach (var cristal in DEBUGCRSITAL)
                {
                    if (cristal.GetComponent<Light2D>() != null)
                    {
                        StartCoroutine(FinishEnigmaAnim(cristal.GetComponent<Light2D>()));
                    }
                }
                TimerManager.StartTimer(3.0f, () => DoorHandler.Instance.GetDoor(_doorToOpen).OpenDoor(() => true));

            }
            else if (_placedCrystals == _totalCrystals)
            {
                Debug.Log("All crystals are placed, but some are incorrect.");
            }
        }

        private IEnumerator FinishEnigmaAnim(Light2D cristalLight)
        {
            float initialRadius = cristalLight.pointLightOuterRadius;
            float endRadius = cristalLight.pointLightOuterRadius + _additionalRadius;
            float duration = 1.5f; 
            float elapsedTime = 0f;

            while (elapsedTime < duration)
            {
                float t = elapsedTime / duration; 
                cristalLight.pointLightOuterRadius = Mathf.Lerp(initialRadius, endRadius, t);
                elapsedTime += Time.deltaTime; 
                yield return null;
            }

            cristalLight.pointLightOuterRadius = endRadius;
            yield return new WaitForSeconds(1.5f);

            initialRadius = cristalLight.pointLightOuterRadius;
            endRadius = 0f;
            elapsedTime = 0f;

            while (elapsedTime < duration)
            {
                float t = elapsedTime / duration;
                cristalLight.pointLightOuterRadius = Mathf.Lerp(initialRadius, endRadius, t);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            cristalLight.pointLightOuterRadius = 0f;
            _animFinish = true;
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
