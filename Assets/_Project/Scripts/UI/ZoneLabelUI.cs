using TMPro;
using UnityEngine;

using WheelOfFortune.Zone; 

namespace WheelOfFortune.UI
{
    public class ZoneLabelUI : MonoBehaviour
    {
        [SerializeField] private ZoneController _zoneController;
        [SerializeField] private TMP_Text _uiTextZoneValue;

        private void OnEnable()
        {
            _zoneController.OnZoneChanged += HandleZoneChanged;
        }

        private void OnDisable()
        {
            _zoneController.OnZoneChanged -= HandleZoneChanged;
        }

        private void HandleZoneChanged(int newZone, ZoneType newZoneType)
        {
            if(_uiTextZoneValue == null)
                return;

            string label = newZoneType switch
            {
                ZoneType.Normal => $"ZONE {newZone}",
                ZoneType.Safe => $"ZONE {newZone} (SAFE)",
                ZoneType.Super => $"ZONE {newZone} (SUPER)",
                _ => $"Unknown Zone {newZone}"
            };

            _uiTextZoneValue.text = label;
        }
    }
}
