using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

using WheelOfFortune.Core;
using WheelOfFortune.Zone;

namespace WheelOfFortune.UI
{
    public class ZoneProgressItem : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private TMP_Text _uiTextZoneNumberValue;
        [SerializeField] private GameObject _currentIndicator;
        [SerializeField] private Image _uiImageIndicatorValue;

        [Header("Config")]
        [SerializeField] private GameConfig _gameConfig;

        [Header("Sprites by Current State")]
        [SerializeField] private Sprite _currentSprite;       
        [SerializeField] private Sprite _safeSprite;          
        [SerializeField] private Sprite _superSprite;         

        public void Bind(int zoneNumber, int currentZone, ZoneType zoneType)
        {
            bool isValid = zoneNumber > 0;
            bool isCurrent = isValid && zoneNumber == currentZone;
            bool isPast = isValid && zoneNumber < currentZone;

            if (!isValid)
            {
                if (_uiTextZoneNumberValue != null)
                {
                    _uiTextZoneNumberValue.text = string.Empty;
                    _uiTextZoneNumberValue.gameObject.SetActive(false);
                }

                if (_currentIndicator != null)
                    _currentIndicator.SetActive(false);

                if (_uiImageIndicatorValue != null)
                    _uiImageIndicatorValue.enabled = false;

                return;   
            }

            if (_uiTextZoneNumberValue != null)
            {
                _uiTextZoneNumberValue.gameObject.SetActive(true);
                _uiTextZoneNumberValue.text = zoneNumber.ToString();                
            }

            if (_currentIndicator != null)
            {
                _currentIndicator.SetActive(isCurrent);                
            }

            if (isCurrent && _uiImageIndicatorValue != null)
            {
                Sprite sprite = zoneType switch
                {
                    ZoneType.Super => _superSprite,
                    ZoneType.Safe  => _safeSprite,
                    _              => _currentSprite,
                };

                _uiImageIndicatorValue.sprite = sprite;
                _uiImageIndicatorValue.enabled = sprite != null;
            }

            Color textColor;

            if (isPast)
            {
                textColor = _gameConfig.PastZoneColor;
            }
            else if (isCurrent)
            {
                textColor = _gameConfig.CurrentZoneColor;
            }
            else
            {
                switch (zoneType)
                {
                    case ZoneType.Super:
                        textColor = _gameConfig.FutureSuperZoneColor;
                        break;
                    case ZoneType.Safe:
                        textColor = _gameConfig.FutureSafeZoneColor;
                        break;
                    default:
                        textColor = _gameConfig.FutureNormalZoneColor;
                        break;
                }
            }

            if (isCurrent)
            {
                this.PlayPopIn(duration: 0.3f, ease: Ease.OutBack);  
            }
            
            _uiTextZoneNumberValue.color = textColor;
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (_uiTextZoneNumberValue == null)
            {
                var t = transform.Find("ui_text_zone_number_value");
                if (t != null) _uiTextZoneNumberValue = t.GetComponent<TMP_Text>();
            }

            if (_uiImageIndicatorValue == null)
            {
                var t = transform.Find("ui_image_indicator/ui_image_indicator_value");
                if (t != null) _uiImageIndicatorValue = t.GetComponent<Image>();
            }

            if (_currentIndicator == null)
            {
                var t = transform.Find("ui_image_indicator");
                if (t != null) _currentIndicator = t.gameObject;
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