using TMPro;
using UnityEngine;

using WheelOfFortune.Currency;

namespace WheelOfFortune.UI
{
    public class CurrencyDisplayUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text _uiTextGoldValue;
        [SerializeField] private CurrencyController _currencyController;

        private void OnEnable()
        {
            _currencyController.OnCurrencyChanged += HandleCurrencyChanged;
        }

        private void OnDisable()
        {
            _currencyController.OnCurrencyChanged -= HandleCurrencyChanged;
        }

        private void HandleCurrencyChanged(int newGold)
        {
            if (_uiTextGoldValue != null)
            {
                _uiTextGoldValue.text = newGold.ToString();
                _uiTextGoldValue.PlayPulse(punchScale: 1.25f);
            }
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (_uiTextGoldValue == null)
                _uiTextGoldValue = GetComponent<TMP_Text>();
        }
#endif
    }
}