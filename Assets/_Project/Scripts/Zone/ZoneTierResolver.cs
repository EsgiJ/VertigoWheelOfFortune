using UnityEngine;
using WheelOfFortune.Wheel;

namespace WheelOfFortune.Zone
{
    [CreateAssetMenu(menuName = "WheelOfFortune/Zone Tier Resolver", fileName = "ZoneTierResolver")]
    public class ZoneTierResolver : ScriptableObject
    {
        [SerializeField] private WheelTierData _normalTier;
        [SerializeField] private WheelTierData _safeTier;
        [SerializeField] private WheelTierData _superTier;

        public WheelTierData Resolve(ZoneType zoneType)
        {
            switch (zoneType)
            {
                case ZoneType.Super: return _superTier != null ? _superTier : _normalTier;
                case ZoneType.Safe:  return _safeTier  != null ? _safeTier  : _normalTier;
                default:             return _normalTier;
            }
        }
    }
}