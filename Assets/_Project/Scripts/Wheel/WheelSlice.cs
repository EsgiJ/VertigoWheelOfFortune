using TMPro;
using UnityEngine;
using UnityEngine.UI;

using WheelOfFortune.Reward;

namespace WheelOfFortune.Wheel
{
    public class WheelSlice : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Image _sliceImage;
        [SerializeField] private TMP_Text _sliceTextAmount;

        public int Index { get; private set; }
        public RewardData Reward { get; private set; }
        public int Amount { get; private set; }
        public bool IsBomb { get; private set; }

        public void Initialize(int index, RewardData reward, int amount, Color textColor)
        {
            Index = index;
            Reward = reward;
            Amount = amount;
            IsBomb = false;

            if(_sliceImage != null)
                _sliceImage.sprite = reward.Icon;
            
            if (_sliceTextAmount != null)
            {
                _sliceTextAmount.color = textColor;
                _sliceTextAmount.text = $"x{amount}";
            }
        }

        public void InitializeAsBomb(int index, Sprite bombSprite)
        {
            Index = index;
            Reward = null;
            Amount = 0;
            IsBomb = true;

            if(_sliceImage != null)
                _sliceImage.sprite = bombSprite;
            
            if(_sliceTextAmount != null)
                _sliceTextAmount.text = string.Empty;
        }

        #if UNITY_EDITOR
        private void OnValidate()
        {
            if (_sliceTextAmount == null)
            {
                var t = transform.Find("ui_text_amount_value");
                if (t != null) _sliceTextAmount = t.GetComponent<TMP_Text>();
            }
        }
        #endif
    }
}