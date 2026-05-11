using System;
using UnityEngine;

using WheelOfFortune.Core;
namespace WheelOfFortune.Zone
{
    public class ZoneController : MonoBehaviour
    {   
        [Header("Config")]
        [SerializeField] private GameConfig _gameConfig;
        
        public int CurrentZone { get; private set; }
        public ZoneType CurrentZoneType => GetZoneType(CurrentZone);

        public event Action<int, ZoneType> OnZoneChanged;
        public event Action OnPlayerCashedOut;
        public event Action OnPlayerBombed;
        public event Action OnPlayerGaveUp;
        public event Action OnPlayerRevived;

        private void Awake()
        {
            CurrentZone = _gameConfig.StartingZone;
        }

        private void Start()
        {
            OnZoneChanged?.Invoke(CurrentZone, CurrentZoneType);
        }

        public void Advance()
        {
            CurrentZone++;
            OnZoneChanged?.Invoke(CurrentZone, CurrentZoneType);
        }

        public void ResetToStart()
        {
            CurrentZone = _gameConfig.StartingZone;
            OnZoneChanged?.Invoke(CurrentZone, CurrentZoneType);
        }

        private ZoneType GetZoneType(int zone)
        {
            if (zone % _gameConfig.SuperZoneInterval == 0) return ZoneType.Super;
            if (zone % _gameConfig.SafeZoneInterval == 0)  return ZoneType.Safe;
            return ZoneType.Normal;
        }

        public void Cashout()
        {
            OnPlayerCashedOut?.Invoke();   
            ResetToStart();                 
        }

        public void Bomb()
        {
            OnPlayerBombed?.Invoke();
        }

        public void GiveUp()
        {
            OnPlayerGaveUp?.Invoke();
            ResetToStart();
        }

        public void Revive()
        {
            OnPlayerRevived?.Invoke();  
            Advance();
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
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