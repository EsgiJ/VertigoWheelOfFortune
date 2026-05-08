using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using WheelOfFortune.Currency;
using WheelOfFortune.Zone;

public class BombPanelUI : MonoBehaviour
{
    [SerializeField] private GameObject _panel;
    [SerializeField] private Button _reviveButton;
    [SerializeField] private Button _giveUpButton;
    [SerializeField] private TMP_Text _uiReviveCostValue;
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
        if (_uiReviveCostValue != null)
            _uiReviveCostValue.text = _reviveCost.ToString();

        _reviveButton.interactable = _currencyController.CanAfford(_reviveCost);
    }

    private void OnGiveUpClicked()
    {
        _panel.SetActive(false);
        _zoneController.GiveUp();
    }

    private void OnReviveClicked()
    {
        if (!_currencyController.TrySpend(_reviveCost)) 
            return;
            
        _panel.SetActive(false);
        _zoneController.Revive();
    }
}
