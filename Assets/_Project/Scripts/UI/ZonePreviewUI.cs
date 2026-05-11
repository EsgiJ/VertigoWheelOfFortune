using TMPro;
using UnityEngine;

using WheelOfFortune.Zone;
using WheelOfFortune.Core;

namespace WheelOfFortune.UI
{
    public class ZonePreviewUI : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private TMP_Text _uiTextSuperValue;     
        [SerializeField] private TMP_Text _uiTextSafeValue;      
        [SerializeField] private ZoneController _zoneController;

        [Header("Config")]
        [SerializeField] private GameConfig _gameConfig;
        
        private void OnEnable()
        {
            _zoneController.OnZoneChanged += HandleZoneChanged;
        }

        private void OnDisable()
        {
            _zoneController.OnZoneChanged -= HandleZoneChanged;
        }

        private void HandleZoneChanged(int currentZone, ZoneType _)
        {
            int nextSuper = _gameConfig.GetNextSuperZone(currentZone);
            int nextSafe  = _gameConfig.GetNextSafeZone(currentZone);

            _uiTextSuperValue.text = $"SUPER ZONE {nextSuper}";
            _uiTextSafeValue.text  = $"SAFE ZONE {nextSafe}";
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (_uiTextSuperValue == null)
            {
                var t = transform.Find("ui_panel_zone_preview_super/ui_text_zone_label_value");
                if (t != null) _uiTextSuperValue = t.GetComponent<TMP_Text>();
            }

            if (_uiTextSafeValue == null)
            {
                var t = transform.Find("ui_panel_zone_preview_safe/ui_text_zone_label_value");
                if (t != null) _uiTextSafeValue = t.GetComponent<TMP_Text>();
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