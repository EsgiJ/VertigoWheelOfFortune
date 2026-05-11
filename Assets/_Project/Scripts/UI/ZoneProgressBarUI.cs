using System.Collections.Generic;
using UnityEngine;

using WheelOfFortune.Zone;
using WheelOfFortune.Core;

namespace WheelOfFortune.UI
{
    public class ZoneProgressBarUI : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private ZoneController _zoneController;
        [SerializeField] private RectTransform _itemsRoot;
        [SerializeField] private ZoneProgressItem _itemPrefab;

        [Header("Config")]
        [SerializeField] private GameConfig _gameConfig;

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
            int totalItems = _gameConfig.ZoneItemsAroundCurrent * 2 + 1;

            for (int i = 0; i < totalItems; i++)
            {
                int offset = i - _gameConfig.ZoneItemsAroundCurrent;
                int zoneNumber = currentZone + offset;
                
                _items[i].gameObject.SetActive(true);

                ZoneType zoneType = GetZoneType(zoneNumber);
                _items[i].Bind(zoneNumber, currentZone, zoneType);
            }
        }

        private void EnsureItemCount()
        {
            int needed = _gameConfig.ZoneItemsAroundCurrent * 2 + 1;
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
            if (zone % _gameConfig.SuperZoneInterval == 0)
                return ZoneType.Super;
            if (zone % _gameConfig.SafeZoneInterval == 0)  
                return ZoneType.Safe;

            return ZoneType.Normal;
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (_itemsRoot == null)
            {
                var t = transform.Find("ui_anchor_zone_items_root");
                if (t != null) _itemsRoot = t as RectTransform;
            }

            if(_zoneController == null)
                _zoneController = FindObjectOfType<ZoneController>(true);

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