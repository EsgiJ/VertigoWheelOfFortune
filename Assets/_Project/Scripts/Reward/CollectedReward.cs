using UnityEngine;

namespace WheelOfFortune.Reward
{
    public readonly struct CollectedReward
    {
        public readonly RewardData RewardData;
        public readonly int Amount;
        
        public CollectedReward(RewardData rewardData, int amount)
        {
            RewardData = rewardData;
            Amount = amount;
        }
    }
}