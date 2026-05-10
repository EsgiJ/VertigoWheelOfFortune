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

            #if UNITY_EDITOR
            private void OnValidate()
            {
                if (_continueButton == null)
                {
                    var t = transform.Find("ui_panel_cashout_content/ui_panel_cashout_dialog/ui_button_continue");
                    if (t != null) _continueButton = t.GetComponent<Button>();
                }

                if (_panel == null)
                {
                    var t = transform.Find("ui_panel_cashout_content");
                    if (t != null) _panel = t.gameObject;
                }

                if (_uiTextMessageValue == null)
                {
                    var t = transform.Find("ui_panel_cashout_content/ui_panel_cashout_dialog/ui_text_message_value");
                    if (t != null) _uiTextMessageValue = t.GetComponent<TMP_Text>();
                }
            }
            #endif

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