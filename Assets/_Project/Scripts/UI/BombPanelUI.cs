using DG.Tweening;
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
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private RectTransform _bombCardTransform;
        [SerializeField] private Transform _glowTransform;
        [SerializeField] private TMP_Text _uiTextMessageValue;
        [SerializeField] private TMP_Text _uiSubtextMessageValue;
        [SerializeField] private Button _reviveButton;
        [SerializeField] private Button _giveUpButton;
        [SerializeField] private TMP_Text _uiTextReviveCostValue;
        [SerializeField] private ZoneController _zoneController;
        [SerializeField] private CurrencyController _currencyController;

        [Header("Config")]
        [SerializeField] private GameConfig _gameConfig;

        private Tween _glowTween;
        
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
            _glowTween?.Kill();
        }

        private void HandleBombed()
        {
            UpdateReviveButtonState();
            ShowPanelAnimated();
        }

        private void HandleCurrencyChanged(int newGold)
        {
            if (_panel.activeSelf)
                UpdateReviveButtonState();
        }

        private void UpdateReviveButtonState()
        {
            if (_uiTextReviveCostValue != null)
                _uiTextReviveCostValue.text = _gameConfig.ReviveCost.ToString();

            _reviveButton.interactable = _currencyController.CanAfford(_gameConfig.ReviveCost);
        }

        private void OnGiveUpClicked()
        {
            _giveUpButton.PlayPressFeedback();

            if (_gameManager.RequestGiveUp())
                HidePanelAnimated();
        }

        private void OnReviveClicked()
        {
            _reviveButton.PlayPressFeedback();

            if (!_currencyController.CanAfford(_gameConfig.ReviveCost))
                return;

            if (!_gameManager.RequestRevive())
                return;

            _currencyController.TrySpend(_gameConfig.ReviveCost);
            HidePanelAnimated();
        }

        private void ShowPanelAnimated()
        {
            _panel.SetActive(true);
            _canvasGroup.alpha = 0f;
            _canvasGroup.PlayFade(1f);
            
            //_dialogTransform.PlayPopIn(duration: 0.3f);
            
            _bombCardTransform.PlayPopIn(duration: 0.4f);
            
            _uiTextMessageValue.PlayShake(duration: 0.5f, strength: 12f);
            _uiSubtextMessageValue.PlayShake(duration: 0.3f, strength: 8f);
            StartGlow();
        }
        private void HidePanelAnimated()
        {
            _glowTween?.Kill();
            _canvasGroup.PlayFade(0f);

            _uiTextMessageValue.PlayPopOut(duration: 0.2f);
            _uiSubtextMessageValue.PlayPopOut(0.2f, () => _panel.SetActive(false));
        }

        private void StartGlow()
        {
            if (_glowTransform == null) return;
            
            _glowTransform.localEulerAngles = Vector3.zero;
            _glowTween?.Kill();
            _glowTween = _glowTransform
                .DORotate(new Vector3(0, 0, 360f), 8f, RotateMode.FastBeyond360)
                .SetLoops(-1)
                .SetEase(Ease.Linear);
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
            
            if (_canvasGroup == null && _panel != null)
                _canvasGroup = _panel.GetComponent<CanvasGroup>();

            if (_bombCardTransform == null)
            {
                var t = transform.Find("ui_panel_bomb_content/ui_panel_bomb_card");
                if (t != null) _bombCardTransform = t.GetComponent<RectTransform>();
            }

            if (_glowTransform == null)
            {
                var t = transform.Find("ui_panel_bomb_content/ui_image_glow");
                if (t != null) _glowTransform = t;
            }

            if (_uiTextMessageValue == null)
            {
                var t = transform.Find("ui_panel_bomb_content/ui_text_message");
                if (t != null) _uiTextMessageValue = t.GetComponent<TMP_Text>();
            }

            if (_uiSubtextMessageValue == null)
            {
                var t = transform.Find("ui_panel_bomb_content/ui_text_subtext_message");
                if (t != null) _uiSubtextMessageValue = t.GetComponent<TMP_Text>();
            }

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

            if (_gameConfig == null)
            {
                var guids = UnityEditor.AssetDatabase.FindAssets("t:GameConfig");
                if (guids.Length > 0)
                {
                    string path = UnityEditor.AssetDatabase.GUIDToAssetPath(guids[0]);
                    _gameConfig = UnityEditor.AssetDatabase.LoadAssetAtPath<GameConfig>(path);
                }
            }
        }
#endif        
    }
}