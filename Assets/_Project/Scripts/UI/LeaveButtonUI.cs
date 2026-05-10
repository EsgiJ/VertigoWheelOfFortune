using UnityEngine;
using UnityEngine.UI;

using WheelOfFortune.Zone;
using WheelOfFortune.Core;

namespace WheelOfFortune.UI
{
    public class LeaveButtonUI : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private GameManager _gameManager;
        [SerializeField] private Button _button;
        [SerializeField] private ZoneController _zoneController;

        private void Awake()
        {
            _button.onClick.AddListener(OnLeaveButtonClicked);
        }

        private void OnEnable()
        {
            _gameManager.OnStateChanged += HandleStateChanged;
            _zoneController.OnZoneChanged += HandleZoneChanged;
            UpdateVisibility(_zoneController.CurrentZoneType);
        }

        private void OnDisable()
        {
            _gameManager.OnStateChanged -= HandleStateChanged;
            _zoneController.OnZoneChanged -= HandleZoneChanged;
        }

        private void OnDestroy()
        {
            _button.onClick.RemoveListener(OnLeaveButtonClicked);
        }

        private void HandleStateChanged(GameState state)
        {
            UpdateVisibility();
        }

        private void HandleZoneChanged(int zone, ZoneType type)
        {
            UpdateVisibility(type);
        }

        private void UpdateVisibility()
        {
            bool canLeave = _gameManager.CurrentState == GameState.ReadyToSpin
                        && _zoneController.CurrentZoneType.AllowsLeaving();

            _button.interactable = canLeave;
        }

        private void UpdateVisibility(ZoneType type)
        {
            bool canLeave = _gameManager.CurrentState == GameState.ReadyToSpin
                        && _zoneController.CurrentZoneType.AllowsLeaving();

            _button.interactable = canLeave;
        }

        private void OnLeaveButtonClicked()
        {
            _button.PlayPressFeedback();                        
            _gameManager.RequestLeave();
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (_gameManager == null)
            {
                _gameManager = FindObjectOfType<GameManager>(true);
            }
            if (_button == null)
            {
                _button = GetComponent<Button>();
            }
            if (_zoneController == null)
            {
                _zoneController = FindObjectOfType<ZoneController>(true);
            }
        }
#endif
    }
}
