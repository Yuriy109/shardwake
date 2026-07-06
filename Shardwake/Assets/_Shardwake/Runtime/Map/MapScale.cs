namespace Shardwake.Map
{
    /// <summary>
    /// Centralized greybox scale values for the modular 13-module map.
    /// Keep these values here so art, camera and spawning can be tuned together.
    /// </summary>
    public static class MapScale
    {
        public const float ModuleSize = 60f;
        public const float HalfModule = ModuleSize * 0.5f;
        public const float OuterHalfExtent = ModuleSize * 2.5f;

        public const float MinimumCorridorWidth = 6f;
        public const float CombatArenaMinWidth = 15f;
        public const float CombatArenaMaxWidth = 25f;

        public const float CameraViewRadius = 40f;
        public const float PortalDiscoveryRadius = 10f;
        public const float PortalInteractRadius = 3f;
        public const float HellgateInteractRadius = 3.2f;
    }
}
