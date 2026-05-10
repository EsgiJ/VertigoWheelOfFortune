using UnityEngine;
using UnityEngine.UI;

using WheelOfFortune.Zone;

namespace WheelOfFortune.UI
{
    public class LeaveButtonUI : MonoBehaviour
    {
        [SerializeField] Button _button;
        [SerializeField] ZoneController _zoneController;

        private void Awake()
        {
            _button.onClick.AddListener(OnLeaveButtonClicked);
        }

        private void OnEnable()
        {
            _zoneController.OnZoneChanged += HandleZoneChanged;
            UpdateVisibility(_zoneController.CurrentZoneType);
        }

        private void OnDisable()
        {
            _zoneController.OnZoneChanged -= HandleZoneChanged;
        }

        #if UNITY_EDITOR
        private void OnValidate()
        {
            if (_button == null)
                _button = GetComponent<Button>();
        }
        #endif

        private void OnDestroy()
        {
            _button.onClick.RemoveListener(OnLeaveButtonClicked);
        }

        private void HandleZoneChanged(int zone, ZoneType type)
        {
            UpdateVisibility(type);
        }

        private void UpdateVisibility(ZoneType type)
        {
            bool canLeave = type.AllowsLeaving();
            _button.interactable = canLeave;
        }

        private void OnLeaveButtonClicked()
        {
            if (!_zoneController.CurrentZoneType.AllowsLeaving()) 
                return;

            _zoneController.Cashout();
        }
    }
}
