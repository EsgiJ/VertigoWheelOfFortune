using TMPro;
using UnityEngine;
using UnityEngine.UI;
using WheelOfFortune.Zone;

namespace WheelOfFortune.UI
{
    public class ZoneProgressItem : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private TMP_Text _uiTextZoneNumberValue;
        [SerializeField] private GameObject _currentIndicator;
        [SerializeField] private Image _uiImageIndicatorValue;

        [Header("Colors")]
        [SerializeField] private Color _pastColor = Color.grey;
        [SerializeField] private Color _futureColor = Color.black;
        [SerializeField] private Color _currentColor = Color.white;
        [SerializeField] private Color _safeColor = Color.green;
        [SerializeField] private Color _superColor = new Color(1f, 0.85f, 0.3f);

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
                textColor = _pastColor;
            }
            else if (isCurrent)
            {
                textColor = _currentColor;
            }
            else
            {
                switch (zoneType)
                {
                    case ZoneType.Super:
                        textColor = _superColor;
                        break;
                    case ZoneType.Safe:
                        textColor = _safeColor;
                        break;
                    default:
                        textColor = _futureColor;
                        break;
                }
            }

            _uiTextZoneNumberValue.color = textColor;
        }
    }
}