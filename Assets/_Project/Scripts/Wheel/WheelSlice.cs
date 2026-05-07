using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace WheelOfFortune.Wheel
{
    public class WheelSlice : MonoBehaviour
    {
        [SerializeField] private Image _sliceImage;
        [SerializeField] private TMP_Text _sliceText;

        public int Index { get; private set; }
        public Reward.RewardData Reward { get; private set; }
        public int Amount { get; private set; }

        public void Initialize(int index, Reward.RewardData reward, int amount)
        {
            Index = index;
            Reward = reward;
            Amount = amount;
            if(_sliceImage != null)
                _sliceImage.sprite = reward.Icon;
            
            if(_sliceText != null)
                _sliceText.text = $"x{amount}";
        }
    }
}