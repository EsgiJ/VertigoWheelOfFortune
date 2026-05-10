using TMPro;
using UnityEngine;
using UnityEngine.UI;
using WheelOfFortune.Zone;
using WheelOfFortune.Core;

namespace WheelOfFortune.UI
{
    public class CashoutPanelUI : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private GameManager _gameManager;
        [SerializeField] private ZoneController _zoneController;
        [SerializeField] private GameObject _panel;
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private Button _continueButton;
        [SerializeField] private TMP_Text _uiTextMessageValue;

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

            ShowPanelAnimated();
        }

        private void OnContinueClicked()
        {
            _continueButton.PlayPressFeedback();

            if (_gameManager.RequestContinue())
                HidePanelAnimated();
        }

        private void ShowPanelAnimated()
        {
            _panel.SetActive(true);
            _canvasGroup.alpha = 0f;
            _canvasGroup.PlayFade(1f);                          
            _uiTextMessageValue.PlayPopIn(duration: 0.3f);        
        }

        private void HidePanelAnimated()
        {
            _canvasGroup.PlayFade(0f);                          
            _uiTextMessageValue.PlayPopOut(duration: 0.2f, onComplete: () => _panel.SetActive(false));     
        }
#if UNITY_EDITOR
        private void OnValidate()
        {
            if (_gameManager == null)
                _gameManager = FindObjectOfType<GameManager>(true);

            if (_zoneController == null)
                _zoneController = FindObjectOfType<ZoneController>(true);

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

            if (_canvasGroup == null && _panel != null)
                _canvasGroup = _panel.GetComponent<CanvasGroup>();

            if (_uiTextMessageValue == null)
            {
                var t = transform.Find("ui_panel_cashout_content/ui_panel_cashout_dialog/ui_text_message_value");
                if (t != null) _uiTextMessageValue = t.GetComponent<TMP_Text>();
            }
        }
#endif
    }
}