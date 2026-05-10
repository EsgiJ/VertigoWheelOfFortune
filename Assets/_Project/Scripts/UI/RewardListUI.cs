using System.Collections.Generic;
using UnityEngine;

using WheelOfFortune.Reward;

namespace WheelOfFortune.UI
{
    public class RewardListUI : MonoBehaviour
    {
        [Header("Reeferences")]
        [SerializeField] private RewardBag _rewardBag;
        [SerializeField] private RectTransform _uiAnchorItemsRoot;
        [SerializeField] private RewardItemView _itemPrefab;

        private readonly Dictionary<RewardData, RewardItemView> _views = new Dictionary<RewardData, RewardItemView>();

        private void OnEnable()
        {
            _rewardBag.OnRewardAdded += HandleRewardAdded;
            _rewardBag.OnCleared += HandleCleared;
        }

        private void OnDisable()
        {
            _rewardBag.OnRewardAdded -= HandleRewardAdded;
            _rewardBag.OnCleared -= HandleCleared;
        }

        #if UNITY_EDITOR
        private void OnValidate()
        {
            if (_uiAnchorItemsRoot == null)
            {
                var t = transform.Find("ui_anchor_items_root");
                if (t != null) _uiAnchorItemsRoot = t as RectTransform;
            }
        }
        #endif

        private void HandleRewardAdded(CollectedReward reward)
        {
            if(reward.RewardData.IsStackable && _views.TryGetValue(reward.RewardData, out RewardItemView existing))
            {
                existing.SetAmount(reward.Amount);
                return;
            }

            RewardItemView item = Instantiate(_itemPrefab, _uiAnchorItemsRoot);
            item.name = $"ui_reward_item_{reward.RewardData.name}";
            item.Bind(reward.RewardData, reward.Amount);

            if (reward.RewardData.IsStackable)
                _views[reward.RewardData] = item;
        }

        private void HandleCleared()
        {
            var viewsSnapshot = new List<RewardItemView>(_views.Values);
            _views.Clear();
            
            foreach(var view in viewsSnapshot)
            {
                if(view != null)
                {
                    Destroy(view .gameObject);
                }

                // have to destroy non-stackable views manually
                for (int i = _uiAnchorItemsRoot.childCount - 1; i >= 0; i--)
                    Destroy(_uiAnchorItemsRoot.GetChild(i).gameObject);
            }
        }
    }
}