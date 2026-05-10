using TMPro;
using UnityEngine;

using WheelOfFortune.Zone;

public class ZonePreviewUI : MonoBehaviour
{
    [SerializeField] private TMP_Text _uiTextSuperValue;     
    [SerializeField] private TMP_Text _uiTextSafeValue;      
    [SerializeField] private ZoneController _zoneController;
    [SerializeField] private int _superInterval = 30;
    [SerializeField] private int _safeInterval = 5;

    private void OnEnable()
    {
        _zoneController.OnZoneChanged += HandleZoneChanged;
    }

    private void OnDisable()
    {
        _zoneController.OnZoneChanged -= HandleZoneChanged;
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
    }
    #endif

    private void HandleZoneChanged(int currentZone, ZoneType _)
    {
        int nextSuper = NextMultipleOf(currentZone, _superInterval);
        int nextSafe  = NextMultipleOf(currentZone, _safeInterval);

        _uiTextSuperValue.text = $"SUPER ZONE {nextSuper}";
        _uiTextSafeValue.text  = $"SAFE ZONE {nextSafe}";
    }

    private static int NextMultipleOf(int from, int step)
    {
        int remainder = from % step;
        return from - remainder + step;
    }
}