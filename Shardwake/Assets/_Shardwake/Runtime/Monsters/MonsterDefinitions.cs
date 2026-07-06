using Shardwake.Loot;
using Shardwake.Map;
using UnityEngine;

namespace Shardwake.Monsters
{
    public static class MonsterDefinitions
    {
        public static MonsterDefinition Get(MonsterType type)
        {
            return type switch
            {
                MonsterType.Wolf => Normal(type, "Wolf", MonsterFaction.Beast, 36, 4.2f, 8.5f, Melee(7, 1.45f, 1.15f, 0.35f), new Color(0.55f, 0.58f, 0.5f)),
                MonsterType.ForestSpirit => Normal(type, "Forest Spirit", MonsterFaction.Elemental, 30, 2.6f, 9.5f, Ranged(6, 6.5f, 0.75f, 0.55f), new Color(0.3f, 0.95f, 0.55f)),
                MonsterType.LivingRoot => Normal(type, "Living Root", MonsterFaction.Elemental, 55, 1.8f, 7.5f, Zone(9, 3.8f, 0.55f, 0.75f), new Color(0.38f, 0.22f, 0.12f)),

                MonsterType.Bandit => Normal(type, "Bandit", MonsterFaction.Bandit, 42, 3.3f, 8.5f, Melee(8, 1.55f, 1.05f, 0.4f), new Color(0.72f, 0.42f, 0.24f)),
                MonsterType.BanditArcher => Normal(type, "Bandit Archer", MonsterFaction.Bandit, 32, 2.9f, 11f, Ranged(7, 7.5f, 0.85f, 0.5f), new Color(0.78f, 0.52f, 0.28f)),
                MonsterType.Scout => Normal(type, "Scout", MonsterFaction.Bandit, 34, 4.1f, 9f, Melee(6, 1.35f, 1.35f, 0.28f), new Color(0.58f, 0.45f, 0.28f)),

                MonsterType.Cultist => Normal(type, "Cultist", MonsterFaction.Cultist, 34, 2.8f, 9f, Ranged(7, 6.2f, 0.8f, 0.55f), new Color(0.62f, 0.2f, 0.75f)),
                MonsterType.TempleGuardian => Elite(type, "Temple Guardian", MonsterFaction.Cultist, 70, 2.3f, 8f, Melee(11, 1.8f, 0.75f, 0.65f), new Color(0.78f, 0.72f, 0.58f)),
                MonsterType.Fanatic => Normal(type, "Fanatic", MonsterFaction.Cultist, 28, 3.8f, 8.5f, Leap(10, 2.6f, 0.65f, 0.45f), new Color(0.86f, 0.32f, 0.42f)),

                MonsterType.IceGoblin => Normal(type, "Ice Goblin", MonsterFaction.Goblin, 36, 3.4f, 8.5f, Melee(7, 1.45f, 1f, 0.38f), new Color(0.5f, 0.82f, 0.95f)),
                MonsterType.FrostWolf => Normal(type, "Frost Wolf", MonsterFaction.Beast, 40, 4.4f, 9f, Leap(8, 2.5f, 0.8f, 0.38f), new Color(0.72f, 0.86f, 1f)),
                MonsterType.FrostShaman => Normal(type, "Frost Shaman", MonsterFaction.Goblin, 34, 2.4f, 10f, Zone(7, 4.2f, 0.65f, 0.65f), new Color(0.38f, 0.7f, 1f)),

                MonsterType.Goblin => Normal(type, "Goblin", MonsterFaction.Goblin, 32, 3.6f, 8f, Melee(6, 1.35f, 1.25f, 0.32f), new Color(0.45f, 0.78f, 0.25f)),
                MonsterType.SpearGoblin => Normal(type, "Spear Goblin", MonsterFaction.Goblin, 38, 3.1f, 8.5f, Melee(8, 2.05f, 0.9f, 0.45f), new Color(0.38f, 0.68f, 0.24f)),
                MonsterType.GoblinShaman => Normal(type, "Goblin Shaman", MonsterFaction.Goblin, 34, 2.4f, 10f, Ranged(6, 6.5f, 0.85f, 0.55f), new Color(0.3f, 0.84f, 0.42f)),

                MonsterType.Skeleton => Normal(type, "Skeleton", MonsterFaction.Undead, 34, 2.9f, 8f, Melee(7, 1.5f, 0.95f, 0.42f), new Color(0.78f, 0.75f, 0.65f)),
                MonsterType.Zombie => Normal(type, "Zombie", MonsterFaction.Undead, 65, 1.9f, 7f, Melee(9, 1.55f, 0.65f, 0.55f), new Color(0.42f, 0.55f, 0.36f)),
                MonsterType.Ghost => Normal(type, "Ghost", MonsterFaction.Undead, 28, 3.4f, 9.5f, Ranged(8, 5.8f, 0.8f, 0.45f), new Color(0.66f, 0.78f, 1f)),

                MonsterType.Slime => Normal(type, "Slime", MonsterFaction.Beast, 48, 2.1f, 7.5f, Melee(6, 1.5f, 0.85f, 0.45f), new Color(0.25f, 0.85f, 0.35f)),
                MonsterType.SwampCreature => Normal(type, "Swamp Creature", MonsterFaction.Beast, 58, 2.5f, 8f, Melee(10, 1.65f, 0.75f, 0.55f), new Color(0.23f, 0.46f, 0.36f)),
                MonsterType.PoisonFrog => Normal(type, "Poison Frog", MonsterFaction.Beast, 30, 3.5f, 9f, Leap(7, 2.4f, 0.85f, 0.35f), new Color(0.45f, 1f, 0.26f)),

                MonsterType.CrystalBeetle => Normal(type, "Crystal Beetle", MonsterFaction.Insect, 46, 2.8f, 8f, Melee(8, 1.35f, 0.95f, 0.4f), new Color(0.3f, 0.88f, 1f)),
                MonsterType.CrystalGolem => Elite(type, "Crystal Golem", MonsterFaction.Elemental, 85, 1.8f, 8f, Melee(14, 1.95f, 0.55f, 0.8f), new Color(0.24f, 0.62f, 0.88f)),
                MonsterType.CrystalBat => Normal(type, "Crystal Bat", MonsterFaction.Beast, 26, 4.6f, 10f, Leap(6, 2.4f, 1.1f, 0.25f), new Color(0.45f, 0.92f, 1f)),

                MonsterType.FireImp => Normal(type, "Fire Imp", MonsterFaction.Hellspawn, 32, 3.2f, 10f, Ranged(9, 6.8f, 0.9f, 0.45f), new Color(1f, 0.32f, 0.12f)),
                MonsterType.AshHound => Normal(type, "Ash Hound", MonsterFaction.Hellspawn, 44, 4.3f, 9f, Leap(9, 2.6f, 0.9f, 0.35f), new Color(0.85f, 0.2f, 0.12f)),

                MonsterType.Spider => Normal(type, "Spider", MonsterFaction.Insect, 30, 3.7f, 8f, Melee(6, 1.35f, 1.3f, 0.28f), new Color(0.32f, 0.22f, 0.36f)),
                MonsterType.VenomSpider => Normal(type, "Venom Spider", MonsterFaction.Insect, 34, 3.4f, 8.5f, Melee(7, 1.35f, 1.1f, 0.32f), new Color(0.45f, 0.2f, 0.58f)),
                MonsterType.CocoonHatchling => Normal(type, "Cocoon Hatchling", MonsterFaction.Insect, 24, 4.2f, 7.5f, Melee(5, 1.25f, 1.45f, 0.22f), new Color(0.72f, 0.62f, 0.75f)),

                MonsterType.CrossbowBandit => Normal(type, "Crossbow Bandit", MonsterFaction.Bandit, 36, 2.5f, 12f, Ranged(10, 8.5f, 0.65f, 0.65f), new Color(0.74f, 0.46f, 0.28f)),
                MonsterType.Harpy => Normal(type, "Harpy", MonsterFaction.Beast, 34, 4.5f, 10f, Leap(8, 2.7f, 0.95f, 0.32f), new Color(0.56f, 0.48f, 0.68f)),

                MonsterType.Drowned => Normal(type, "Drowned", MonsterFaction.Undead, 46, 2.4f, 8f, Melee(8, 1.5f, 0.85f, 0.5f), new Color(0.25f, 0.52f, 0.62f)),
                MonsterType.WaterSpirit => Normal(type, "Water Spirit", MonsterFaction.Elemental, 32, 3.1f, 10f, Ranged(7, 6.4f, 0.9f, 0.45f), new Color(0.25f, 0.72f, 1f)),
                MonsterType.FisherZombie => Normal(type, "Fisher Zombie", MonsterFaction.Undead, 42, 2.7f, 8.5f, Ranged(8, 5.2f, 0.75f, 0.55f), new Color(0.32f, 0.58f, 0.52f)),

                _ => Normal(MonsterType.Shardling, "Shardling", MonsterFaction.Corrupted, 45, 3.2f, 9f, Melee(8, 1.45f, 0.9f, 0.45f), new Color(0.9f, 0.28f, 0.22f))
            };
        }

        public static MonsterType GetForBiome(BiomeType biome, int spawnIndex)
        {
            var pool = biome switch
            {
                BiomeType.AncientGrove => new[] { MonsterType.Wolf, MonsterType.ForestSpirit, MonsterType.LivingRoot },
                BiomeType.BanditRoad => new[] { MonsterType.Bandit, MonsterType.BanditArcher, MonsterType.Scout },
                BiomeType.BrokenShrine => new[] { MonsterType.Cultist, MonsterType.TempleGuardian, MonsterType.Fanatic },
                BiomeType.FrostCamp => new[] { MonsterType.IceGoblin, MonsterType.FrostWolf, MonsterType.FrostShaman },
                BiomeType.GoblinDen => new[] { MonsterType.Goblin, MonsterType.SpearGoblin, MonsterType.GoblinShaman },
                BiomeType.OldCemetery => new[] { MonsterType.Skeleton, MonsterType.Zombie, MonsterType.Ghost },
                BiomeType.SwampRuins => new[] { MonsterType.Slime, MonsterType.SwampCreature, MonsterType.PoisonFrog },
                BiomeType.CrystalHollow => new[] { MonsterType.CrystalBeetle, MonsterType.CrystalGolem, MonsterType.CrystalBat },
                BiomeType.EmberGate => new[] { MonsterType.FireImp, MonsterType.AshHound, MonsterType.FireImp },
                BiomeType.SpiderNest => new[] { MonsterType.Spider, MonsterType.VenomSpider, MonsterType.CocoonHatchling },
                BiomeType.FallenWatchtower => new[] { MonsterType.CrossbowBandit, MonsterType.Harpy, MonsterType.CrossbowBandit },
                BiomeType.SunkenVillage => new[] { MonsterType.Drowned, MonsterType.WaterSpirit, MonsterType.FisherZombie },
                _ => new[] { MonsterType.Shardling, MonsterType.Shardling, MonsterType.Shardling }
            };

            return pool[Mathf.Abs(spawnIndex) % pool.Length];
        }

        public static MonsterType GetMiniBossForBiome(BiomeType biome)
        {
            return biome switch
            {
                BiomeType.AncientGrove => MonsterType.LivingRoot,
                BiomeType.BanditRoad => MonsterType.BanditArcher,
                BiomeType.BrokenShrine => MonsterType.TempleGuardian,
                BiomeType.FrostCamp => MonsterType.FrostShaman,
                BiomeType.GoblinDen => MonsterType.GoblinShaman,
                BiomeType.OldCemetery => MonsterType.Zombie,
                BiomeType.SwampRuins => MonsterType.SwampCreature,
                BiomeType.CrystalHollow => MonsterType.CrystalGolem,
                BiomeType.EmberGate => MonsterType.AshHound,
                BiomeType.SpiderNest => MonsterType.VenomSpider,
                BiomeType.FallenWatchtower => MonsterType.CrossbowBandit,
                BiomeType.SunkenVillage => MonsterType.FisherZombie,
                _ => MonsterType.Shardling
            };
        }

        private static MonsterDefinition Normal(MonsterType type, string displayName, MonsterFaction faction, float hp, float moveSpeed, float aggroRange, MonsterAttackDefinition attack, Color color)
        {
            return new MonsterDefinition(type, displayName, faction, MonsterDifficulty.Normal, ResolveBehavior(type, attack), hp, moveSpeed, aggroRange, attack, LootSource.NormalEnemy, color);
        }

        private static MonsterDefinition Elite(MonsterType type, string displayName, MonsterFaction faction, float hp, float moveSpeed, float aggroRange, MonsterAttackDefinition attack, Color color)
        {
            return new MonsterDefinition(type, displayName, faction, MonsterDifficulty.Elite, ResolveBehavior(type, attack), hp, moveSpeed, aggroRange, attack, LootSource.EliteEnemy, color);
        }

        private static MonsterBehaviorType ResolveBehavior(MonsterType type, MonsterAttackDefinition attack)
        {
            return type switch
            {
                MonsterType.Wolf or MonsterType.FrostWolf or MonsterType.AshHound or MonsterType.PoisonFrog or MonsterType.CrystalBat or MonsterType.Harpy => MonsterBehaviorType.Charger,
                MonsterType.BanditArcher or MonsterType.CrossbowBandit or MonsterType.FireImp or MonsterType.WaterSpirit or MonsterType.FisherZombie or MonsterType.Ghost => MonsterBehaviorType.RangedKiter,
                MonsterType.ForestSpirit or MonsterType.GoblinShaman => MonsterBehaviorType.SupportHealer,
                MonsterType.Cultist or MonsterType.FrostShaman => MonsterBehaviorType.Caster,
                MonsterType.TempleGuardian or MonsterType.CrystalGolem or MonsterType.Zombie or MonsterType.SwampCreature => MonsterBehaviorType.Tank,
                MonsterType.Scout or MonsterType.Fanatic => MonsterBehaviorType.Ambusher,
                MonsterType.Spider or MonsterType.CocoonHatchling or MonsterType.Goblin => MonsterBehaviorType.Swarm,
                _ => attack.IsRanged ? MonsterBehaviorType.RangedKiter : MonsterBehaviorType.Bruiser
            };
        }

        private static MonsterAttackDefinition Melee(float damage, float range, float attackSpeed, float windup)
        {
            return new MonsterAttackDefinition(MonsterAttackKind.MeleeCone, damage, range, attackSpeed, windup);
        }

        private static MonsterAttackDefinition Ranged(float damage, float range, float attackSpeed, float windup)
        {
            return new MonsterAttackDefinition(MonsterAttackKind.RangedShot, damage, range, attackSpeed, windup, 0.8f);
        }

        private static MonsterAttackDefinition Leap(float damage, float range, float attackSpeed, float windup)
        {
            return new MonsterAttackDefinition(MonsterAttackKind.Leap, damage, range, attackSpeed, windup, 1.1f);
        }

        private static MonsterAttackDefinition Zone(float damage, float range, float attackSpeed, float windup)
        {
            return new MonsterAttackDefinition(MonsterAttackKind.GroundZone, damage, range, attackSpeed, windup, 2.2f);
        }
    }
}
