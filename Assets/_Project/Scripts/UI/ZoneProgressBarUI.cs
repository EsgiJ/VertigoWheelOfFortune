using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using WheelOfFortune.Zone;

namespace WheelOfFortune.UI
{
    public class ZoneProgressBarUI : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private ZoneController _zoneController;
        [SerializeField] private RectTransform _itemsRoot;
        [SerializeField] private ZoneProgressItem _itemPrefab;

        [Header("Layout")]
        [SerializeField, Min(1)] private int _itemsAroundCurrent = 6;
        [SerializeField, Min(1)] private int _safeZoneInterval = 5;
        [SerializeField, Min(1)] private int _superZoneInterval = 30;


        private readonly List<ZoneProgressItem> _items = new List<ZoneProgressItem>();

        private void OnEnable()
        {
            _zoneController.OnZoneChanged += HandleZoneChanged;
        }

        private void OnDisable()
        {
            _zoneController.OnZoneChanged -= HandleZoneChanged;
        }

        private void HandleZoneChanged(int currentZone, ZoneType currentType)
        {
            EnsureItemCount();
            int totalItems = _itemsAroundCurrent * 2 + 1;

            for (int i = 0; i < totalItems; i++)
            {
                int offset = i - _itemsAroundCurrent;
                int zoneNumber = currentZone + offset;
                
                _items[i].gameObject.SetActive(true);

                ZoneType zoneType = GetZoneType(zoneNumber);
                _items[i].Bind(zoneNumber, currentZone, zoneType);
            }
        }

        private void EnsureItemCount()
        {
            int needed = _itemsAroundCurrent * 2 + 1;
            while (_items.Count < needed)
            {
                var item = Instantiate(_itemPrefab, _itemsRoot);
                _items.Add(item);
            }
        }

        private ZoneType GetZoneType(int zone)
        {
            if (zone <= 0) 
                return ZoneType.Normal;
            if (zone % _superZoneInterval == 0)
                return ZoneType.Super;
            if (zone % _safeZoneInterval == 0)  
                return ZoneType.Safe;

            return ZoneType.Normal;
        }
    }
}