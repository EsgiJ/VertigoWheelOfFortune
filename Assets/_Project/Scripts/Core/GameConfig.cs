using UnityEngine;

using WheelOfFortune.Zone;

namespace WheelOfFortune.Core
{
    [CreateAssetMenu(menuName = "WheelOfFortune/Game Config", fileName = "GameConfig")]
    public class GameConfig : ScriptableObject
    {
        [Header("Zone Rules")]
        [Tooltip("Every Nth zone becomes a safe zone (no bombs, player can cash out).")]
        [SerializeField, Min(1)] private int _safeZoneInterval = 5;

        [Tooltip("Every Nth zone becomes a super zone (golden zone with special rewards).")]
        [SerializeField, Min(1)] private int _superZoneInterval = 30;

        [Tooltip("The zone number where the player starts the game.")]
        [SerializeField, Min(1)] private int _startingZone = 1;
        
        [Header("Reward Scaling")]
        [Tooltip("Reward increase per zone. baseAmount * (1 + zone * growth)")]
        [SerializeField, Min(0f)] private float _rewardGrowthPerZone = 0.5f;

        [Header("Currency")]
        [Tooltip("Starting gold amount for the player.")]
        [SerializeField, Min(0)] private int _startingGold = 100;

        [Tooltip("Gold cost to revive after hitting a bomb.")]
        [SerializeField, Min(0)] private int _reviveCost = 25;

        [Header("Zone Preview")]
        [Tooltip("How many zones are shown on each side of the current zone in the progress bar.")]
        [SerializeField, Min(2)] private int _zoneItemsAroundCurrent = 6;

        [Header("Zone Progress Bar UI Colors")]
        [SerializeField] private Color _pastZoneColor = Color.grey;
        [SerializeField] private Color _currentZoneColor = Color.white;
        [SerializeField] private Color _futureNormalZoneColor = Color.white;
        [SerializeField] private Color _futureSafeZoneColor = Color.green;
        [SerializeField] private Color _futureSuperZoneColor = new Color(1f, 0.85f, 0.3f);

        public int SafeZoneInterval => _safeZoneInterval;
        public int SuperZoneInterval => _superZoneInterval;
        public int StartingZone => _startingZone;
        public float RewardGrowthPerZone => _rewardGrowthPerZone;
        public int StartingGold => _startingGold;
        public int ReviveCost => _reviveCost;
        public int ZoneItemsAroundCurrent => _zoneItemsAroundCurrent;

        public Color PastZoneColor => _pastZoneColor;
        public Color CurrentZoneColor => _currentZoneColor;
        public Color FutureNormalZoneColor => _futureNormalZoneColor;
        public Color FutureSafeZoneColor => _futureSafeZoneColor;
        public Color FutureSuperZoneColor => _futureSuperZoneColor;
        
        public ZoneType GetZoneType(int zoneNumber)
        {
            if (zoneNumber <= 0) return ZoneType.Normal;
            if (zoneNumber % _superZoneInterval == 0) return ZoneType.Super;
            if (zoneNumber % _safeZoneInterval == 0) return ZoneType.Safe;
            return ZoneType.Normal;
        }

        public int ScaleRewardAmount(int baseAmount, int zoneNumber)
        {
            int z = Mathf.Max(1, zoneNumber);
            float scaled = baseAmount * (1f + _rewardGrowthPerZone * (z - 1));
            return Mathf.Max(1, Mathf.RoundToInt(scaled));
        }

        public int GetNextSuperZone(int currentZone)
        {
            int remainder = currentZone % _superZoneInterval;
            return currentZone - remainder + _superZoneInterval;
        }

        public int GetNextSafeZone(int currentZone)
        {
            int remainder = currentZone % _safeZoneInterval;
            return currentZone - remainder + _safeZoneInterval;
        }
    }
}