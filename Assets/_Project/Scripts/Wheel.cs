using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections.Generic;

namespace WheelOfFortune.Wheel
{
    public class Wheel : MonoBehaviour
    {
        [SerializeField] private RectTransform _wheelBase;
        [SerializeField] private RectTransform _wheelAnchorRotor;
        [SerializeField] private WheelSlice _slicePrefab;
        [SerializeField] private Button _spinButton;
        [SerializeField] private Sprite[] _icons;

        [SerializeField] private int _sliceCount = 8;
        [SerializeField] private float _sliceRadius = 140f; // Distance from center to slice position
        [SerializeField] private float _spinDuration = 4f;
        [SerializeField] private int _minSpinRounds= 3;
        [SerializeField] private int[] _amounts;

        private List<WheelSlice> _slices = new List<WheelSlice>();
        private bool _isSpinning = false;

        void Awake()
        {
            BuildSlices();
            _spinButton.onClick.AddListener(Spin);
        }
        
        private void BuildSlices()
        {
            float anglePerSlice = 360f / _sliceCount;

            for(int i = 0; i < _sliceCount; i++)
            {
                WheelSlice slice = Instantiate(_slicePrefab, _wheelAnchorRotor);
                slice.name = $"Slice_ {i}";

                float angle = -i * anglePerSlice;
                float rad = (angle + 90f) * Mathf.Deg2Rad; // +90 to start from top 

                RectTransform rectTransform = slice.transform as RectTransform;
                rectTransform.anchoredPosition = new Vector2(Mathf.Cos(rad) * _sliceRadius, Mathf.Sin(rad) * _sliceRadius);

                Sprite icon = _icons[i];
                int amount = _amounts[i];
                slice.Initialize(i, icon, amount);

                _slices.Add(slice);
            }
        }
        private void Spin()
        {
            if(_isSpinning)
                return;

            _isSpinning = true;
            _spinButton.interactable = false;

            float randomEndAngle = Random.Range(0f, 360f);
            float total = _minSpinRounds * 360f + randomEndAngle;

            _wheelBase.transform.DORotate(new Vector3(0f, 0f, -total), _spinDuration, RotateMode.FastBeyond360)
                .SetEase(Ease.OutCubic)
                .SetLink(gameObject)
                .OnComplete(() =>
                {
                _isSpinning = false;
                _spinButton.interactable = true; 
                });
        }
    }
}