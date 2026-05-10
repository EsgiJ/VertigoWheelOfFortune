using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

using WheelOfFortune.Reward;

namespace WheelOfFortune.UI
{
    public class RewardItemView : MonoBehaviour
    {   
        [Header("References")]
        [SerializeField] private Image _itemImage;
        [SerializeField] private TMP_Text _itemTextAmount;

        public RewardData Reward { get; private set; }

        #if UNITY_EDITOR
        private void OnValidate()
        {
            if (_itemImage == null)
            {
                var t = transform.Find("ui_image_icon_value");
                if (t != null) _itemImage = t.GetComponent<Image>();
            }

            if (_itemTextAmount == null)
            {
                var t = transform.Find("ui_text_amount_value");
                if (t != null) _itemTextAmount = t.GetComponent<TMP_Text>();
            }
        }
        #endif

        public void Bind(RewardData reward, int amount)
        {
            Reward = reward;

            if(_itemImage != null)
            {
                _itemImage.sprite = reward.Icon;
                _itemImage.preserveAspect = true;
            }
                
            if(_itemTextAmount != null)
                _itemTextAmount.text = $"{amount}";
        }

        public void SetAmount(int amount)
        {
            if(_itemTextAmount != null)
                _itemTextAmount.text = $"{amount}";
        }
    }
}