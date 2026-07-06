using System.Collections.Generic;
using Shardwake.Loot;

namespace Shardwake.AI
{
    public static class ChronicleGenerator
    {
        public static string Create(bool survived, int enemiesKilled, IReadOnlyList<LootItem> loot, string loadoutName)
        {
            var bestRelic = GetBestRelic(loot);
            var battleLine = enemiesKilled == 0
                ? "avoided the shardlings that stalked the ruins"
                : $"cut down {enemiesKilled} shardlings among the broken stones";

            if (survived)
            {
                var relicLine = loot.Count == 0
                    ? "carrying no relics, but a map full of hard lessons"
                    : $"carrying {loot.Count} relics, including {bestRelic}";

                return $"The {loadoutName} entered the Grey Shard and {battleLine}. When the portal steadied, they crossed its violet threshold {relicLine}. Haven records the expedition as a small flame kept alive.";
            }

            var lossLine = loot.Count == 0
                ? "before any relic could be claimed"
                : $"with {loot.Count} unclaimed relics lost to the collapse";

            return $"The {loadoutName} pressed too deep into the Grey Shard and {battleLine}. The portal never became salvation. Their trail vanished {lossLine}, leaving only a warning whispered in Haven.";
        }

        private static string GetBestRelic(IReadOnlyList<LootItem> loot)
        {
            if (loot.Count == 0)
            {
                return "nothing";
            }

            var best = loot[0];
            for (var i = 1; i < loot.Count; i++)
            {
                if ((int)loot[i].Rarity > (int)best.Rarity)
                {
                    best = loot[i];
                }
            }

            return $"{best.DisplayName}, a power {best.Power} relic";
        }
    }
}
