using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections.Generic;

using WheelOfFortune.Reward;
using WheelOfFortune.Zone;

namespace WheelOfFortune.Wheel
{
    public class WheelController : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private RectTransform _wheelBase;
        [SerializeField] private RectTransform _wheelAnchorRotor;
        [SerializeField] private WheelSlice _slicePrefab;
        [SerializeField] private Button _spinButton;
        [SerializeField] private RewardBag _rewardBag;
        [SerializeField] private ZoneController _zoneController;
        
        [Header("Config")]
        [SerializeField] private int _sliceCount = 8;
        [SerializeField] private float _sliceRadius = 140f; // Distance from center to slice position
        [SerializeField] private float _spinDuration = 4f;
        [SerializeField] private int _minSpinRounds= 3;

        [Header("Slices")]
        [SerializeField] private RewardData[] _sliceRewards;

        [Header("Bomb")]
        [SerializeField] private Sprite _bombSprite;

        private List<WheelSlice> _slices = new List<WheelSlice>();
        private bool _isSpinning = false;
        private int _bombSliceIndex = -1;

        public bool CanLeave => !_isSpinning && _zoneController.CurrentZoneType.AllowsLeaving();

        void Awake()
        {
            _spinButton.onClick.AddListener(Spin);
            _zoneController.OnZoneChanged += HandleZoneChanged;
        }
        
        void Start()
        {
            BuildSlices();
        }

        void OnDestroy()
        {
            _spinButton.onClick.RemoveListener(Spin);
            _zoneController.OnZoneChanged -= HandleZoneChanged;
        }

        private void HandleZoneChanged(int zone, ZoneType type)
        {
            BuildSlices(); 
        }

        private void BuildSlices()
        {
            foreach(var slice in _slices)
            {
                if(slice != null)
                {
                    Destroy(slice.gameObject);
                }
            }
            _slices.Clear();

            bool includeBomb = _zoneController.CurrentZoneType.HasBomb();
            _bombSliceIndex = includeBomb ? Random.Range(0, _sliceCount) : -1;

            float anglePerSlice = 360f / _sliceCount;

            for(int i = 0; i < _sliceCount; i++)
            {
                WheelSlice slice = Instantiate(_slicePrefab, _wheelAnchorRotor);
                slice.name = $"Slice_ {i}";

                float angle = i * anglePerSlice;
                float rad = (angle + 90f) * Mathf.Deg2Rad; // +90 to start from top 

                RectTransform rectTransform = slice.transform as RectTransform;
                rectTransform.anchoredPosition = new Vector2(Mathf.Cos(rad) * _sliceRadius, Mathf.Sin(rad) * _sliceRadius);
                rectTransform.localRotation = Quaternion.Euler(0, 0, angle);

                if(i == _bombSliceIndex)
                {
                    slice.InitializeAsBomb(i, _bombSprite);
                }
                else
                {
                    RewardData reward = _sliceRewards[i];
                    int amount = reward.BaseAmount;
                    slice.Initialize(i, reward, amount);
                }

                _slices.Add(slice);
            }
        }
        
        private void Spin()
        {
            if(_isSpinning)
                return;

            _isSpinning = true;
            _spinButton.interactable = false;

            int selectedIndex = Random.Range(0, _sliceCount);

            float anglePerSlice = 360f / _sliceCount;
            float targetAngle = anglePerSlice * selectedIndex;
            float total = _minSpinRounds * 360f + targetAngle;

            _wheelAnchorRotor.localEulerAngles = Vector3.zero;
            _wheelAnchorRotor.transform.DORotate(new Vector3(0f, 0f, -total), _spinDuration, RotateMode.FastBeyond360)
                .SetEase(Ease.OutCubic)
                .SetLink(gameObject)
                .OnComplete(() => OnSpinComplete(selectedIndex));
        }

        private void OnSpinComplete(int selectedIndex)
        {
            _isSpinning = false;
            _spinButton.interactable = true; 

            WheelSlice slice = _slices[selectedIndex];

            if (slice.IsBomb)
            {
                Debug.Log($"[Wheel] Bomb hit! selected slice {selectedIndex}, clearing rewards");
                _rewardBag.ClearRewards();
                _zoneController.Bomb(); 
                return;
            }

            Debug.Log($"[Wheel] Selected slice {selectedIndex} - icon: {slice.Reward?.DisplayName} x{slice.Amount}");
            
            if(slice != null && slice.Amount > 0)
            {
                _rewardBag.AddReward(slice.Reward, slice.Amount);
            }

            _zoneController.Advance();
        }
    }
}