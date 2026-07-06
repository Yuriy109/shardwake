using System.Collections.Generic;
using Shardwake.Loot;
using UnityEngine;

namespace Shardwake.Core
{
    public readonly struct HavenProgressSnapshot
    {
        public HavenProgressSnapshot(int extractedRelics, int totalRelicPower, string bestRelicName, int bestRelicPower)
        {
            ExtractedRelics = extractedRelics;
            TotalRelicPower = totalRelicPower;
            BestRelicName = bestRelicName;
            BestRelicPower = bestRelicPower;
        }

        public int ExtractedRelics { get; }
        public int TotalRelicPower { get; }
        public string BestRelicName { get; }
        public int BestRelicPower { get; }
    }

    public static class HavenProgress
    {
        private const string ExtractedRelicsKey = "Shardwake.Haven.ExtractedRelics";
        private const string TotalRelicPowerKey = "Shardwake.Haven.TotalRelicPower";
        private const string BestRelicNameKey = "Shardwake.Haven.BestRelicName";
        private const string BestRelicPowerKey = "Shardwake.Haven.BestRelicPower";
        private const string ShardDustKey = "Shardwake.Haven.ShardDust";
        private const string ContractsCompletedKey = "Shardwake.Haven.ContractsCompleted";

        public static int ShardDust => PlayerPrefs.GetInt(ShardDustKey, 0);
        public static int ContractsCompleted => PlayerPrefs.GetInt(ContractsCompletedKey, 0);

        public static HavenProgressSnapshot Current => new(
            PlayerPrefs.GetInt(ExtractedRelicsKey, 0),
            PlayerPrefs.GetInt(TotalRelicPowerKey, 0),
            PlayerPrefs.GetString(BestRelicNameKey, "None"),
            PlayerPrefs.GetInt(BestRelicPowerKey, 0));

        public static HavenProgressSnapshot RecordExtraction(IReadOnlyList<LootItem> loot)
        {
            if (loot.Count == 0)
            {
                return Current;
            }

            var snapshot = Current;
            var extractedRelics = snapshot.ExtractedRelics + loot.Count;
            var totalPower = snapshot.TotalRelicPower;
            var bestName = snapshot.BestRelicName;
            var bestPower = snapshot.BestRelicPower;

            for (var i = 0; i < loot.Count; i++)
            {
                var item = loot[i];
                totalPower += item.Power;

                if (item.Power > bestPower)
                {
                    bestPower = item.Power;
                    bestName = item.DisplayName;
                }
            }

            PlayerPrefs.SetInt(ExtractedRelicsKey, extractedRelics);
            PlayerPrefs.SetInt(TotalRelicPowerKey, totalPower);
            PlayerPrefs.SetString(BestRelicNameKey, bestName);
            PlayerPrefs.SetInt(BestRelicPowerKey, bestPower);
            PlayerPrefs.Save();

            return new HavenProgressSnapshot(extractedRelics, totalPower, bestName, bestPower);
        }

        public static void AddShardDust(int amount)
        {
            if (amount <= 0)
            {
                return;
            }

            PlayerPrefs.SetInt(ShardDustKey, ShardDust + amount);
            PlayerPrefs.Save();
        }

        public static bool TrySpendShardDust(int amount)
        {
            if (amount <= 0)
            {
                return true;
            }

            if (ShardDust < amount)
            {
                return false;
            }

            PlayerPrefs.SetInt(ShardDustKey, ShardDust - amount);
            PlayerPrefs.Save();
            return true;
        }

        public static void RecordContractCompleted()
        {
            PlayerPrefs.SetInt(ContractsCompletedKey, ContractsCompleted + 1);
            AddShardDust(15);
        }

        public static void Reset()
        {
            PlayerPrefs.DeleteKey(ExtractedRelicsKey);
            PlayerPrefs.DeleteKey(TotalRelicPowerKey);
            PlayerPrefs.DeleteKey(BestRelicNameKey);
            PlayerPrefs.DeleteKey(BestRelicPowerKey);
            PlayerPrefs.DeleteKey(ShardDustKey);
            PlayerPrefs.DeleteKey(ContractsCompletedKey);
            PlayerPrefs.Save();
        }
    }
}
