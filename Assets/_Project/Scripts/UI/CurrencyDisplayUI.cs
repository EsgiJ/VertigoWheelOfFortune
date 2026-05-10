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
            _currencyController.OnCurrencyChanged += HandleGoldChanged;
        }

        private void OnDisable()
        {
            _currencyController.OnCurrencyChanged -= HandleGoldChanged;
        }

        #if UNITY_EDITOR
        private void OnValidate()
        {
            if (_uiTextGoldValue == null)
                _uiTextGoldValue = GetComponent<TMP_Text>();
        }
        #endif

        private void HandleGoldChanged(int gold)
        {
            if (_uiTextGoldValue != null)
                _uiTextGoldValue.text = gold.ToString();
        }
    }
}