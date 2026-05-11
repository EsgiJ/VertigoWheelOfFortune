using UnityEngine;
using WheelOfFortune.Currency;
using WheelOfFortune.Reward;
using WheelOfFortune.Zone;

namespace WheelOfFortune.Debug
{
    public class DebugPanel : MonoBehaviour
    {
        [SerializeField] private ZoneController _zoneController;
        [SerializeField] private CurrencyController _currencyController;
        [SerializeField] private RewardBag _rewardBag;
        [SerializeField] private RewardData[] _testRewards;

        public ZoneController Zone => _zoneController;
        public CurrencyController Currency => _currencyController;
        public RewardBag Bag => _rewardBag;
        public RewardData[] TestRewards => _testRewards;

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (_zoneController == null)
                _zoneController = FindObjectOfType<ZoneController>(true);
            if (_currencyController == null)
                _currencyController = FindObjectOfType<CurrencyController>(true);
            if (_rewardBag == null)
                _rewardBag = FindObjectOfType<RewardBag>(true);
        }
#endif
    }
}