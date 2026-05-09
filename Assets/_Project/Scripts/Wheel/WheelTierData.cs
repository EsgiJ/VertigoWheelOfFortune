using UnityEngine;

using WheelOfFortune.Reward;

namespace WheelOfFortune.Wheel
{
    [CreateAssetMenu(menuName = "WheelOfFortune/Wheel Tier", fileName = "WheelTierData")]
    public class WheelTierData : ScriptableObject
    {
        [Header("Name")]
        [SerializeField] private string _displayLabel = "SPIN";

        [Header("Visuals")]
        [SerializeField] private Sprite _wheelBaseSprite;
        [SerializeField] private Sprite _wheelIndicatorSprite;
        [SerializeField] private Color _sliceTextColor = Color.white;

        [Header("Slice Content")]
        [SerializeField] private RewardData[] _sliceRewards = new RewardData[8];

        [Header("Bomb")]
        [SerializeField] private bool _hasBomb = true;

        [Header("Spin Tuning")]
        [SerializeField, Min(0.5f)] private float _spinDuration = 4f;
        [SerializeField, Min(1)] private int _minSpinRounds = 3;

        public string DisplayLabel        => _displayLabel;
        public Sprite WheelBaseSprite     => _wheelBaseSprite;
        public Sprite WheelIndicatorSprite => _wheelIndicatorSprite;
        public Color  SliceTextColor      => _sliceTextColor;
        public RewardData[] SliceRewards  => _sliceRewards;
        public int    SliceCount          => _sliceRewards != null ? _sliceRewards.Length : 0;
        public bool   HasBomb             => _hasBomb;
        public float  SpinDuration        => _spinDuration;
        public int    MinSpinRounds       => _minSpinRounds;
    }
}