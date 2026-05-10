using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections.Generic;
using System;

using WheelOfFortune.Reward;
using WheelOfFortune.Zone;
using WheelOfFortune.Core;
using WheelOfFortune.UI;

namespace WheelOfFortune.Wheel
{
    public class WheelController : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private GameManager _gameManager;
        [SerializeField] private RectTransform _wheelAnchorRotor;
        [SerializeField] private Image _wheelBaseImage;             
        [SerializeField] private Image _wheelIndicatorImage;        
        [SerializeField] private WheelSlice _slicePrefab;
        [SerializeField] private Button _spinButton;
        [SerializeField] private TMP_Text _spinLabelText;           
        [SerializeField] private RewardBag _rewardBag;
        [SerializeField] private ZoneController _zoneController;
        
        [Header("Config")]
        [SerializeField] private ZoneTierResolver _tierResolver;
        [SerializeField] private float _sliceRadius = 140f; // Distance from center to slice position
        [SerializeField, Min(0f)] private float _rewardGrowthPerZone = 0.5f;

        [Header("Bomb")]
        [SerializeField] private Sprite _bombSprite;

        private List<WheelSlice> _slices = new List<WheelSlice>();
        private bool _isSpinning = false;
        private int _bombSliceIndex = -1;
        private WheelTierData _currentTier;

        public event Action OnSpinStarted;
        public event Action OnSpinResolved;

        public bool CanLeave => !_isSpinning && _zoneController.CurrentZoneType.AllowsLeaving();

        void Awake()
        {
            _spinButton.onClick.AddListener(OnSpinButtonClicked);

            _zoneController.OnZoneChanged += HandleZoneChanged;
            _zoneController.OnPlayerCashedOut += HandleCashout;
            _zoneController.OnPlayerGaveUp += HandleGiveUp;
            _zoneController.OnPlayerRevived += HandleRevive;
            _zoneController.OnPlayerBombed += HandleBombed;

            _gameManager.OnStateChanged += HandleStateChanged;
        }
        
        void Start()
        {
        }

        void OnDestroy()
        {
            _spinButton.onClick.RemoveListener(Spin);

            _zoneController.OnZoneChanged -= HandleZoneChanged;
            _zoneController.OnPlayerCashedOut -= HandleCashout;
            _zoneController.OnPlayerGaveUp -= HandleGiveUp;
            _zoneController.OnPlayerRevived -= HandleRevive;
            _zoneController.OnPlayerBombed -= HandleBombed;

            _gameManager.OnStateChanged -= HandleStateChanged;
        }

        private void HandleZoneChanged(int zone, ZoneType type)
        {
            _currentTier = _tierResolver.Resolve(type);
            ApplyTierVisuals(_currentTier);
            BuildSlices(); 
            _spinButton.interactable = true;
        }

        private void ApplyTierVisuals(WheelTierData tier)
        {
            if (tier == null) return;

            if (_wheelBaseImage != null && tier.WheelBaseSprite != null)
                _wheelBaseImage.sprite = tier.WheelBaseSprite;

            if (_wheelIndicatorImage != null && tier.WheelIndicatorSprite != null)
                _wheelIndicatorImage.sprite = tier.WheelIndicatorSprite;

            if (_spinLabelText != null)
                _spinLabelText.text = tier.DisplayLabel;
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

            int count = _currentTier.SliceCount;
            bool includeBomb = _zoneController.CurrentZoneType.HasBomb();
            _bombSliceIndex = includeBomb ? UnityEngine.Random.Range(0, count) : -1;

            float anglePerSlice = 360f / count;

            for(int i = 0; i < count; i++)
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
                    RewardData reward = _currentTier.SliceRewards[i];
                    int amount = ScaleAmountForZone(reward != null ? reward.BaseAmount : 0, _zoneController.CurrentZone);
                    slice.Initialize(i, reward, amount, _currentTier.SliceTextColor);
                }

                _slices.Add(slice);
            }
        }
        
        private int ScaleAmountForZone(int baseAmount, int zone)
        {
            int z = Mathf.Max(1, zone);
            float scaled = baseAmount * (1f + _rewardGrowthPerZone * (z - 1));
            return Mathf.Max(1, Mathf.RoundToInt(scaled));
        }

        private void OnSpinButtonClicked()
        {
            _gameManager.RequestSpin();
        }

        public void Spin()
        {
            if(_isSpinning)
                return;

            _spinButton.PlayPressFeedback();

            _isSpinning = true;
            _spinButton.interactable = false;

            OnSpinStarted?.Invoke();

            int selectedIndex = UnityEngine.Random.Range(0, _currentTier.SliceCount);
            float anglePerSlice = 360f / _currentTier.SliceCount;
            float targetAngle = anglePerSlice * selectedIndex;
            float total = _currentTier.MinSpinRounds * 360f + targetAngle;

            _wheelAnchorRotor.localEulerAngles = Vector3.zero;
            _wheelAnchorRotor.transform.DORotate(new Vector3(0f, 0f, -total), _currentTier.SpinDuration, RotateMode.FastBeyond360)
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
                OnBombHit();
                return;
            }

            Debug.Log($"[Wheel] Selected slice {selectedIndex} - icon: {slice.Reward?.DisplayName} x{slice.Amount}");
            
            if(slice != null && slice.Amount > 0)
            {
                _rewardBag.AddReward(slice.Reward, slice.Amount);
            }

            _zoneController.Advance();

            OnSpinResolved?.Invoke();
        }

        private void OnBombHit()
        {
            Debug.Log($"[Wheel] Bomb hit! Clearing rewards");
            _zoneController.Bomb(); 
        }

        private void HandleBombed()
        {
            _spinButton.interactable = false;
        }

        private void HandleCashout()
        {
            Debug.Log($"[Wheel] Cashout triggered! Clearing rewards");
            _rewardBag.ClearRewards();
        }

        private void HandleGiveUp()
        {
            Debug.Log($"[Wheel] Give Up triggered! Clearing rewards");
            _rewardBag.ClearRewards();
        }

        private void HandleRevive()
        {
            Debug.Log($"[Wheel] Revive triggered! Clearing rewards");
        }

        private void HandleStateChanged(GameState state)
        {
            _spinButton.interactable = state == GameState.ReadyToSpin;
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (_gameManager == null)
            {
                _gameManager = FindObjectOfType<GameManager>(true);
            }

            if (_spinButton == null)
            {
                var t = transform.Find("ui_button_spin");
                if (t != null) _spinButton = t.GetComponent<Button>();
            }

            if (_wheelAnchorRotor == null)
            {
                var t = transform.Find("ui_anchor_rotor");
                if (t != null) _wheelAnchorRotor = t as RectTransform;
            }

            if (_wheelBaseImage == null)
            {
                var t = transform.Find("ui_anchor_rotor/ui_image_wheel_base");
                if (t != null) _wheelBaseImage = t.GetComponent<Image>();
            }

            if (_wheelIndicatorImage == null)
            {
                var t = transform.Find("ui_image_wheel_indicator");
                if (t != null) _wheelIndicatorImage = t.GetComponent<Image>();
            }
        }
#endif
    }
}