using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WheelOfFortune.Reward
{   
    public class RewardBag : MonoBehaviour
    {
        private readonly List<CollectedReward> _collectedRewards = new List<CollectedReward>();
        public IReadOnlyList<CollectedReward> CollectedRewards => _collectedRewards;

        public void AddReward(RewardData rewardData, int amount)
        {
            if (rewardData == null || amount <= 0)
                return;
            
            if(rewardData.IsStackable)
            {
                for(int i = 0; i < _collectedRewards.Count; i++)
                {
                    if(_collectedRewards[i].RewardData == rewardData)
                    {
                        int newAmount = _collectedRewards[i].Amount + amount;
                        CollectedReward mergedReward = new CollectedReward(rewardData, newAmount);
                        _collectedRewards[i] = mergedReward;
                        return;
                    }
                }
            }
            CollectedReward newReward = new CollectedReward(rewardData, amount);
            _collectedRewards.Add(newReward);
        }

        public void ClearRewards()
        {
            _collectedRewards.Clear();
        }
    }
}