
using UnityEngine;

namespace WheelOfFortune.Reward
{
    [CreateAssetMenu(fileName = "RewardData", menuName = "WheelOfFortune/RewardData")]
    public class RewardData : ScriptableObject
    {
        [Header("Config")]
        [SerializeField] private string _displayName = "Default";
        [SerializeField] private int _baseAmount = 1;
        [SerializeField] private bool _isStackable = true;
        [SerializeField] private Sprite _icon = null;

        public string DisplayName => _displayName;
        public Sprite Icon => _icon;       
        public int BaseAmount => _baseAmount;
        public bool IsStackable => _isStackable;
    }
}