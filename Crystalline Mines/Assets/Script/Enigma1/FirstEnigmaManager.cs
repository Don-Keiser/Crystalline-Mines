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

        [Header("Enigma data")]
        [SerializeField] private int _totalCrystals = 5;
        [SerializeField] private List<PuzzleSlotController> puzzleSlots; // List of puzzle slots

        [Header("Crystal Placed")]
        private int _placedCrystals = 0;
        private List<GameObject> _placedCrystalObjects = new List<GameObject>();
        [SerializeField] private List<GameObject> DEBUGCRSITAL = new List<GameObject>();

        [Header("End Enigma Camera animation")]
        private Vector3 _doorPos;
        [SerializeField] private float _maxCameraDezoom;
        [SerializeField] private float _animDuration;
        [SerializeField] private float _fullscreenDuration;

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
            _placedCrystalObjects = DEBUGCRSITAL; //pas retirer pour le debug



            Debug.Log("Puzzle successfully completed!");
            IsPuzzleCompleted = true;
            foreach (var cristal in _placedCrystalObjects)
            {
                if (cristal.TryGetComponent<Light2D>(out Light2D light))
                {
                    StartCoroutine(FinishEnigmaAnim(light));
                }
                cristal.layer = 0;
            }
            foreach (var slot in puzzleSlots)
            {
                slot.gameObject.layer = 0;
            }

            TimerManager.StartTimer(3.0f, () => DoorHandler.Instance.GetDoor(_doorToOpen).OpenDoor(() => true));
        }

        public void CheckPuzzleCompletion()
        {
            if (_placedCrystals == _totalCrystals && CheckAllSlots())
            {
                Debug.Log("Puzzle successfully completed!");
                IsPuzzleCompleted = true;
                foreach (var cristal in _placedCrystalObjects)
                {
                    if (cristal.TryGetComponent<Light2D>(out Light2D light))
                    {
                        StartCoroutine(FinishEnigmaAnim(light));
                    }
                    cristal.layer = 0;
                }
                foreach (var slot in puzzleSlots)
                {
                    slot.gameObject.layer = 0;
                }

                TimerManager.StartTimer(3.0f, () => DoorHandler.Instance.GetDoor(_doorToOpen).OpenDoor(() => true));
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
                cristalLight.intensity += Time.deltaTime * 2;
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
            cristalLight.intensity = 0.0f;
            cristalLight.pointLightOuterRadius = 0f;
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
