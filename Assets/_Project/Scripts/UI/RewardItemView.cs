using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

using WheelOfFortune.Reward;

namespace WheelOfFortune.UI
{
    public class RewardItemView : MonoBehaviour
    {   
        [SerializeField] private Image _itemImage;
        [SerializeField] private TMP_Text _itemTextAmount;

        public RewardData Reward { get; private set; }

        public void Bind(RewardData reward, int amount)
        {
            Reward = reward;

            if(_itemImage != null)
            {
                _itemImage.sprite = reward.Icon;
                _itemImage.preserveAspect = true;
            }
                
            if(_itemTextAmount != null)
                _itemTextAmount.text = $"x{amount}";
        }

        public void SetAmount(int amount)
        {
            if(_itemTextAmount != null)
                _itemTextAmount.text = $"x{amount}";
        }
    }
}