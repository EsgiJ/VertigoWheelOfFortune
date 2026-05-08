namespace WheelOfFortune.Zone
{
    public static class ZoneTypeExtensions
    {
        public static bool HasBomb(this ZoneType type)
        {
            return type == ZoneType.Normal;
        }

        public static bool AllowsLeaving(this ZoneType type)
        {
            return  (type == ZoneType.Safe) || (type ==ZoneType.Super);
        }
    }
}