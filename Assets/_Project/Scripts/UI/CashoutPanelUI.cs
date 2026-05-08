using TMPro;
using UnityEngine;
using UnityEngine.UI;

using WheelOfFortune.Zone;

namespace WheelOfFortune.UI
{
    public class CashoutPanelUI : MonoBehaviour
        {
            [SerializeField] private GameObject _panel;
            [SerializeField] private Button _continueButton;
            [SerializeField] private TMP_Text _uiTextMessageValue;
            [SerializeField] private ZoneController _zoneController;

            private void Awake()
            {
                _continueButton.onClick.AddListener(OnContinueClicked);
                _panel.SetActive(false);
            }

            private void OnEnable()
            {
                _zoneController.OnPlayerCashedOut += HandleCashedOut;
            }

            private void OnDisable()
            {
                _zoneController.OnPlayerCashedOut -= HandleCashedOut;
            }

            private void OnDestroy()
            {
                _continueButton.onClick.RemoveListener(OnContinueClicked);
            }

            private void HandleCashedOut()
            {
                if (_uiTextMessageValue != null)
                    _uiTextMessageValue.text = "REWARDS COLLECTED!";

                _panel.SetActive(true);
            }

            private void OnContinueClicked()
            {
                _panel.SetActive(false);
            }
    }
}