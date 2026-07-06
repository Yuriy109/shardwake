using System.Collections.Generic;
using Shardwake.Loot;

namespace Shardwake.Core
{
    public readonly struct ExpeditionResult
    {
        public ExpeditionResult(
            bool survived,
            int enemiesKilled,
            int miniBossesDefeated,
            float durationSeconds,
            string loadoutName,
            bool shardAlertTriggered,
            string alertRelicName,
            IReadOnlyList<LootItem> savedLoot,
            IReadOnlyList<LootItem> droppedLoot,
            IReadOnlyList<LootItem> insuredReturned,
            IReadOnlyList<LootItem> insuredLost,
            HavenProgressSnapshot haven,
            string outcomeText,
            bool enteredHellgate)
        {
            Survived = survived;
            EnemiesKilled = enemiesKilled;
            MiniBossesDefeated = miniBossesDefeated;
            DurationSeconds = durationSeconds;
            LoadoutName = loadoutName;
            ShardAlertTriggered = shardAlertTriggered;
            AlertRelicName = alertRelicName;
            SavedLoot = savedLoot;
            DroppedLoot = droppedLoot;
            InsuredReturned = insuredReturned;
            InsuredLost = insuredLost;
            Haven = haven;
            OutcomeText = outcomeText;
            EnteredHellgate = enteredHellgate;
        }

        public bool Survived { get; }
        public int EnemiesKilled { get; }
        public int MiniBossesDefeated { get; }
        public float DurationSeconds { get; }
        public string LoadoutName { get; }
        public bool ShardAlertTriggered { get; }
        public string AlertRelicName { get; }
        public IReadOnlyList<LootItem> SavedLoot { get; }
        public IReadOnlyList<LootItem> DroppedLoot { get; }
        public IReadOnlyList<LootItem> InsuredReturned { get; }
        public IReadOnlyList<LootItem> InsuredLost { get; }
        public HavenProgressSnapshot Haven { get; }
        public string OutcomeText { get; }
        public bool EnteredHellgate { get; }
    }
}
