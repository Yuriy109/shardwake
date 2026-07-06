using UnityEngine;

namespace Shardwake.Map
{
    public readonly struct MapModuleDefinition
    {
        public MapModuleDefinition(BiomeType biome, string displayName, Color color, Vector3[] obstacles, Vector3[] enemies, Vector3[] chests, Vector3[] portals)
        {
            Biome = biome;
            DisplayName = displayName;
            Color = color;
            Obstacles = obstacles;
            Enemies = enemies;
            Chests = chests;
            Portals = portals;
        }

        public BiomeType Biome { get; }
        public string DisplayName { get; }
        public Color Color { get; }
        public Vector3[] Obstacles { get; }
        public Vector3[] Enemies { get; }
        public Vector3[] Chests { get; }
        public Vector3[] Portals { get; }
    }
}
