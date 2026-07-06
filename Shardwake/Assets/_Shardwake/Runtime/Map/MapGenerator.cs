using System;
using System.Collections.Generic;
using UnityEngine;

namespace Shardwake.Map
{
    public static class MapGenerator
    {
        public const float ModuleSize = MapScale.ModuleSize;

        private static readonly Vector2Int[] ModuleSlots =
        {
            new Vector2Int(0, 2),
            new Vector2Int(-1, 1), new Vector2Int(0, 1), new Vector2Int(1, 1),
            new Vector2Int(-2, 0), new Vector2Int(-1, 0), new Vector2Int(0, 0), new Vector2Int(1, 0), new Vector2Int(2, 0),
            new Vector2Int(-1, -1), new Vector2Int(0, -1), new Vector2Int(1, -1),
            new Vector2Int(0, -2)
        };

        public static MapLayout Generate(int seed)
        {
            var random = new System.Random(seed);
            var modules = new List<PlacedMapModule>();
            var pool = CreateModules();
            Shuffle(pool, random);

            for (var i = 0; i < ModuleSlots.Length; i++)
            {
                var slot = ModuleSlots[i];
                var definition = slot == Vector2Int.zero ? CreateCenterModule() : pool[i % pool.Count];
                var origin = new Vector3(slot.x * ModuleSize, 0f, slot.y * ModuleSize);
                var rotation = slot == Vector2Int.zero ? 0 : random.Next(0, 4);
                modules.Add(new PlacedMapModule(definition, slot, origin, rotation));
            }

            return new MapLayout(seed, modules);
        }

        public static MapModuleDefinition CreateCenterModule()
        {
            return Module(
                BiomeType.ShardCrossing,
                "Shard Crossing",
                new Color(0.36f, 0.31f, 0.46f),
                new[]
                {
                    new Vector3(-18f, 0.6f, 0f),
                    new Vector3(18f, 0.6f, 0f),
                    new Vector3(0f, 0.7f, 18f),
                    new Vector3(0f, 0.7f, -18f),
                    new Vector3(-12f, 0.6f, -13f),
                    new Vector3(12f, 0.6f, 13f)
                },
                new[] { new Vector3(-16f, 1f, 12f), new Vector3(16f, 1f, -12f), new Vector3(0f, 1f, 22f), new Vector3(0f, 1f, -20f) },
                new[] { new Vector3(0f, 0.5f, 0f), new Vector3(0f, 0.5f, -24f) },
                Array.Empty<Vector3>());
        }

        private static List<MapModuleDefinition> CreateModules()
        {
            return new List<MapModuleDefinition>
            {
                Module(BiomeType.AncientGrove, "Ancient Grove", new Color(0.22f, 0.48f, 0.22f)),
                Module(BiomeType.BanditRoad, "Bandit Road", new Color(0.44f, 0.34f, 0.24f)),
                Module(BiomeType.BrokenShrine, "Broken Shrine", new Color(0.42f, 0.38f, 0.52f)),
                Module(BiomeType.FrostCamp, "Frost Camp", new Color(0.58f, 0.68f, 0.72f)),
                Module(BiomeType.GoblinDen, "Goblin Den", new Color(0.34f, 0.38f, 0.18f)),
                Module(BiomeType.OldCemetery, "Old Cemetery", new Color(0.31f, 0.31f, 0.36f)),
                Module(BiomeType.SwampRuins, "Swamp Ruins", new Color(0.18f, 0.36f, 0.32f)),
                Module(BiomeType.CrystalHollow, "Crystal Hollow", new Color(0.25f, 0.48f, 0.58f)),
                Module(BiomeType.EmberGate, "Ember Gate", new Color(0.48f, 0.2f, 0.12f)),
                Module(BiomeType.SpiderNest, "Spider Nest", new Color(0.26f, 0.22f, 0.3f)),
                Module(BiomeType.FallenWatchtower, "Fallen Watchtower", new Color(0.38f, 0.32f, 0.28f)),
                Module(BiomeType.SunkenVillage, "Sunken Village", new Color(0.22f, 0.38f, 0.44f))
            };
        }

        private static MapModuleDefinition Module(BiomeType biome, string displayName, Color color)
        {
            return Module(
                biome,
                displayName,
                color,
                new[]
                {
                    new Vector3(-20f, 0.7f, -12f),
                    new Vector3(-6f, 0.5f, 17f),
                    new Vector3(16f, 0.8f, 12f),
                    new Vector3(21f, 0.5f, -18f),
                    new Vector3(0f, 0.65f, 0f)
                },
                new[]
                {
                    new Vector3(-16f, 1f, 12f),
                    new Vector3(16f, 1f, -12f),
                    new Vector3(0f, 1f, 0f),
                    new Vector3(8f, 1f, 20f)
                },
                new[]
                {
                    new Vector3(4f, 0.5f, 22f),
                    new Vector3(-18f, 0.5f, 5f),
                    new Vector3(18f, 0.5f, -20f)
                },
                new[]
                {
                    new Vector3(-22f, 0.08f, -22f)
                });
        }

        private static MapModuleDefinition Module(BiomeType biome, string displayName, Color color, Vector3[] obstacles, Vector3[] enemies, Vector3[] chests, Vector3[] portals)
        {
            return new MapModuleDefinition(biome, displayName, color, obstacles, enemies, chests, portals);
        }

        private static void Shuffle<T>(IList<T> list, System.Random random)
        {
            for (var i = list.Count - 1; i > 0; i--)
            {
                var j = random.Next(0, i + 1);
                (list[i], list[j]) = (list[j], list[i]);
            }
        }
    }
}
