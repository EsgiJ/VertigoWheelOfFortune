using TMPro;
using UnityEngine;
using UnityEngine.UI;
using WheelOfFortune.Core;
using WheelOfFortune.Currency;
using WheelOfFortune.Zone;

namespace WheelOfFortune.UI
{
    public class BombPanelUI : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private GameManager _gameManager;
        [SerializeField] private GameObject _panel;
        [SerializeField] private Button _reviveButton;
        [SerializeField] private Button _giveUpButton;
        [SerializeField] private TMP_Text _uiTextReviveCostValue;
        [SerializeField] private ZoneController _zoneController;
        [SerializeField] private CurrencyController _currencyController;

        [SerializeField, Min(0)] private int _reviveCost = 25;

        private void Awake()
        {
            _giveUpButton.onClick.AddListener(OnGiveUpClicked);
            _reviveButton.onClick.AddListener(OnReviveClicked);

            _panel.SetActive(false);
        }

        private void OnEnable()
        {
            _zoneController.OnPlayerBombed += HandleBombed;
            _currencyController.OnCurrencyChanged += HandleCurrencyChanged;
        }

        private void OnDisable()
        {
            _zoneController.OnPlayerBombed -= HandleBombed;
            _currencyController.OnCurrencyChanged -= HandleCurrencyChanged;
        }

        private void OnDestroy()
        {
            _giveUpButton.onClick.RemoveListener(OnGiveUpClicked);
            _reviveButton.onClick.RemoveListener(OnReviveClicked);
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (_gameManager == null)
                _gameManager = FindObjectOfType<GameManager>(true);

            if (_zoneController == null)
                _zoneController = FindObjectOfType<ZoneController>(true);

            if (_currencyController == null)
                _currencyController = FindObjectOfType<CurrencyController>(true);

            if (_giveUpButton == null)
            {
                var t = transform.Find("ui_panel_bomb_content/ui_panel_bomb_dialog/ui_button_giveup");
                if (t != null) _giveUpButton = t.GetComponent<Button>();
            }

            if (_reviveButton == null)
            {
                var t = transform.Find("ui_panel_bomb_content/ui_panel_bomb_dialog/ui_button_revive");
                if (t != null) _reviveButton = t.GetComponent<Button>();
            }

            if (_uiTextReviveCostValue == null)
            {
                var t = transform.Find("ui_panel_bomb_content/ui_panel_bomb_dialog/ui_button_revive/ui_text_revive_cost_value");
                if (t != null) _uiTextReviveCostValue = t.GetComponent<TMP_Text>();
            }

            if (_panel == null)
            {
                var t = transform.Find("ui_panel_bomb_content");
                if (t != null) _panel = t.gameObject;
            }
        }
#endif

        private void HandleBombed()
        {
            UpdateReviveButtonState();
            _panel.SetActive(true);
        }

        private void HandleCurrencyChanged(int newGold)
        {
            if (_panel.activeSelf)
                UpdateReviveButtonState();
        }

        private void UpdateReviveButtonState()
        {
            if (_uiTextReviveCostValue != null)
                _uiTextReviveCostValue.text = _reviveCost.ToString();

            _reviveButton.interactable = _currencyController.CanAfford(_reviveCost);
        }

        private void OnGiveUpClicked()
        {
            if (_gameManager.RequestGiveUp())
                _panel.SetActive(false);
        }

        private void OnReviveClicked()
        {
            if (!_currencyController.CanAfford(_reviveCost))
                return;

            if (!_gameManager.RequestRevive())
                return;

            _currencyController.TrySpend(_reviveCost);
            _panel.SetActive(false);
        }
    }
}