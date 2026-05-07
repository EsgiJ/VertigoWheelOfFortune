using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WheelOfFortune.Zone
{
    public class ZoneController : MonoBehaviour
    {
        [SerializeField, Min(1)] private int _startingZone = 1;
        [SerializeField, Min(1)] private int _safeZoneInterval = 5;
        [SerializeField, Min(1)] private int _superZoneInterval = 30;

        public int CurrentZone { get; private set; }
        public ZoneType CurrentZoneType => GetZoneType(CurrentZone);

        public event Action<int, ZoneType> OnZoneChanged;

        private void Awake()
        {
            CurrentZone = _startingZone;
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
            CurrentZone = _startingZone;
            OnZoneChanged?.Invoke(CurrentZone, CurrentZoneType);
        }

        private ZoneType GetZoneType(int zone)
        {
            if (zone % _superZoneInterval == 0) return ZoneType.Super;
            if (zone % _safeZoneInterval == 0)  return ZoneType.Safe;
            return ZoneType.Normal;
        }
    }
}