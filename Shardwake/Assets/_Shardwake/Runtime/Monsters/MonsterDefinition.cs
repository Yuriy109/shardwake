using Shardwake.Loot;
using UnityEngine;

namespace Shardwake.Monsters
{
    public readonly struct MonsterDefinition
    {
        public MonsterDefinition(
            MonsterType type,
            string displayName,
            MonsterFaction faction,
            MonsterDifficulty difficulty,
            MonsterBehaviorType behavior,
            float maxHealth,
            float moveSpeed,
            float aggroRange,
            MonsterAttackDefinition attack,
            LootSource lootSource,
            Color color)
        {
            Type = type;
            DisplayName = displayName;
            Faction = faction;
            Difficulty = difficulty;
            Behavior = behavior;
            MaxHealth = Mathf.Max(1f, maxHealth);
            MoveSpeed = Mathf.Max(0.1f, moveSpeed);
            AggroRange = Mathf.Max(attack.Range, aggroRange);
            Attack = attack;
            LootSource = lootSource;
            Color = color;
        }

        public MonsterType Type { get; }
        public string DisplayName { get; }
        public MonsterFaction Faction { get; }
        public MonsterDifficulty Difficulty { get; }
        public MonsterBehaviorType Behavior { get; }
        public float MaxHealth { get; }
        public float MoveSpeed { get; }
        public float AggroRange { get; }
        public MonsterAttackDefinition Attack { get; }
        public LootSource LootSource { get; }
        public Color Color { get; }
    }
}
