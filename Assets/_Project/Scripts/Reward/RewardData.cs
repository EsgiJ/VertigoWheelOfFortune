
using UnityEngine;

namespace WheelOfFortune.Reward
{
    [CreateAssetMenu(fileName = "RewardData", menuName = "WheelOfFortune/RewardData")]
    public class RewardData : ScriptableObject
    {
        [SerializeField] private string _displayName = "Default";
        [SerializeField] private Sprite _icon = null;
        [SerializeField] private int _baseAmount = 1;

        public string DisplayName => _displayName;
        public Sprite Icon => _icon;       
        public int BaseAmount => _baseAmount;
    }
}