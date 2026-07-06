using System.Collections.Generic;
using Shardwake.Equipment;
using UnityEngine;

namespace Shardwake.Loot
{
    public static class LootGenerator
    {
        private static readonly string[] RelicNames =
        {
            "Old Kingdom Relic",
            "Sunken Crown Shard",
            "Violet Portal Core",
            "Ashen Saint Sigil",
            "Emerald Oathstone",
            "Seeker's Broken Compass",
            "Moonwell Ember"
        };

        private static readonly string[] WeaponNames =
        {
            "Iron Greatblade",
            "Templar Sword",
            "Twin Fang Daggers",
            "Pilgrim Battle Staff",
            "Ashwood Bow",
            "Runed Mage Staff",
            "Holy Reliquary",
            "Bonebound Tome"
        };

        private static readonly string[] ArmorNames =
        {
            "Worn Hood",
            "Brigandine Vest",
            "Travel Gloves",
            "Scout Legguards",
            "Iron Boots"
        };

        private static readonly string[] AccessoryNames =
        {
            "Copper Ring",
            "Shard Ring",
            "Old Amulet",
            "Moonlit Pendant"
        };

        private static readonly string[] MaterialNames =
        {
            "Shard Dust",
            "Broken Fang",
            "Goblin Scrap",
            "Crystal Splinter",
            "Ancient Cloth"
        };

        private static readonly string[] FlavorLines =
        {
            "Warm to the touch, as if it remembers sunrise.",
            "Its surface hums softly near unstable portals.",
            "A city scribe would pay well to study this.",
            "The old kingdoms left more questions than answers.",
            "A faint sigil flickers beneath the cracked metal."
        };

        public static LootItem Roll(LootSource source = LootSource.NormalEnemy)
        {
            var rarity = LootTables.Get(source).Roll();
            var rarityTier = (int)rarity + 1;
            var category = RollCategory(source, rarity);
            var power = Random.Range(8, 15) * rarityTier + Random.Range(0, 7);
            var rule = RarityRules.Get(rarity);
            var affixes = category == LootItemCategory.Material || category == LootItemCategory.Consumable || category == LootItemCategory.Gold
                ? System.Array.Empty<LootAffix>()
                : RollAffixes(rule);
            var flavor = rule.HasAiFlavor || rarity >= ItemRarity.Rare ? FlavorLines[Random.Range(0, FlavorLines.Length)] : string.Empty;
            var isUnstable = category == LootItemCategory.Relic && (rarity >= ItemRarity.Epic || (rarity == ItemRarity.Rare && Random.value > 0.65f));
            var slotSize = GetSlotSize(category, rarity);
            var equipmentSlot = RollEquipmentSlot(category);
            var armorType = RollArmorType(category);
            var name = RollName(category, equipmentSlot);

            return new LootItem(name, rarity, power, affixes, flavor, isUnstable, slotSize, category, equipmentSlot, armorType);
        }

        private static LootItemCategory RollCategory(LootSource source, ItemRarity rarity)
        {
            var roll = Random.value;

            if (source == LootSource.HellgateBoss || source == LootSource.AncientChest || rarity >= ItemRarity.Epic)
            {
                if (roll < 0.32f) return LootItemCategory.Relic;
                if (roll < 0.58f) return LootItemCategory.Weapon;
                if (roll < 0.82f) return LootItemCategory.Armor;
                if (roll < 0.92f) return LootItemCategory.Ring;
                return LootItemCategory.Amulet;
            }

            if (roll < 0.24f) return LootItemCategory.Material;
            if (roll < 0.38f) return LootItemCategory.Consumable;
            if (roll < 0.56f) return LootItemCategory.Relic;
            if (roll < 0.72f) return LootItemCategory.Armor;
            if (roll < 0.86f) return LootItemCategory.Weapon;
            if (roll < 0.95f) return LootItemCategory.Ring;
            return LootItemCategory.Amulet;
        }

        private static ItemSlotSize GetSlotSize(LootItemCategory category, ItemRarity rarity)
        {
            switch (category)
            {
                case LootItemCategory.Consumable:
                case LootItemCategory.Material:
                case LootItemCategory.Gold:
                case LootItemCategory.Ring:
                case LootItemCategory.Amulet:
                    return ItemSlotSize.Small;
                case LootItemCategory.Weapon:
                case LootItemCategory.Armor:
                    return ItemSlotSize.Medium;
                case LootItemCategory.Relic:
                    return rarity >= ItemRarity.Epic ? ItemSlotSize.Large : ItemSlotSize.Medium;
                default:
                    return ItemSlotSize.Small;
            }
        }

        private static EquipmentSlot RollEquipmentSlot(LootItemCategory category)
        {
            if (category == LootItemCategory.Weapon)
            {
                return EquipmentSlot.Weapon1;
            }

            if (category == LootItemCategory.Ring)
            {
                return EquipmentSlot.Ring1;
            }

            if (category == LootItemCategory.Amulet)
            {
                return EquipmentSlot.Amulet;
            }

            if (category != LootItemCategory.Armor)
            {
                return EquipmentSlot.Chest;
            }

            return Random.Range(0, 5) switch
            {
                0 => EquipmentSlot.Head,
                1 => EquipmentSlot.Chest,
                2 => EquipmentSlot.Hands,
                3 => EquipmentSlot.Legs,
                _ => EquipmentSlot.Feet
            };
        }

        private static ArmorType RollArmorType(LootItemCategory category)
        {
            if (category != LootItemCategory.Armor)
            {
                return ArmorType.Medium;
            }

            return Random.Range(0, 3) switch
            {
                0 => ArmorType.Light,
                1 => ArmorType.Medium,
                _ => ArmorType.Heavy
            };
        }

        private static string RollName(LootItemCategory category, EquipmentSlot slot)
        {
            switch (category)
            {
                case LootItemCategory.Relic:
                    return RelicNames[Random.Range(0, RelicNames.Length)];
                case LootItemCategory.Weapon:
                    return WeaponNames[Random.Range(0, WeaponNames.Length)];
                case LootItemCategory.Armor:
                    return GetArmorName(slot);
                case LootItemCategory.Ring:
                case LootItemCategory.Amulet:
                    return AccessoryNames[Random.Range(0, AccessoryNames.Length)];
                case LootItemCategory.Material:
                    return MaterialNames[Random.Range(0, MaterialNames.Length)];
                case LootItemCategory.Consumable:
                    return "Healing Flask";
                case LootItemCategory.Gold:
                    return "Gold Pouch";
                default:
                    return RelicNames[Random.Range(0, RelicNames.Length)];
            }
        }

        private static string GetArmorName(EquipmentSlot slot)
        {
            return slot switch
            {
                EquipmentSlot.Head => ArmorNames[0],
                EquipmentSlot.Chest => ArmorNames[1],
                EquipmentSlot.Hands => ArmorNames[2],
                EquipmentSlot.Legs => ArmorNames[3],
                EquipmentSlot.Feet => ArmorNames[4],
                _ => ArmorNames[1]
            };
        }

        private static IReadOnlyList<LootAffix> RollAffixes(RarityRule rule)
        {
            if (rule.AffixPoints <= 0)
            {
                return System.Array.Empty<LootAffix>();
            }

            var affixes = new List<LootAffix>();
            var remaining = rule.AffixPoints;

            if (rule.GuaranteedMinimumSingleStat > 0)
            {
                affixes.Add(new LootAffix(RollStat(), rule.GuaranteedMinimumSingleStat));
                remaining -= rule.GuaranteedMinimumSingleStat;
            }

            while (remaining > 0)
            {
                var amount = Mathf.Min(remaining, Random.Range(1, 3));
                affixes.Add(new LootAffix(RollStat(), amount));
                remaining -= amount;
            }

            return MergeDuplicateAffixes(affixes);
        }

        private static LootStat RollStat()
        {
            return (LootStat)Random.Range(0, System.Enum.GetValues(typeof(LootStat)).Length);
        }

        private static IReadOnlyList<LootAffix> MergeDuplicateAffixes(List<LootAffix> affixes)
        {
            var merged = new Dictionary<LootStat, int>();
            for (var i = 0; i < affixes.Count; i++)
            {
                var affix = affixes[i];
                merged.TryGetValue(affix.Stat, out var current);
                merged[affix.Stat] = current + affix.Value;
            }

            var result = new List<LootAffix>();
            foreach (var pair in merged)
            {
                result.Add(new LootAffix(pair.Key, pair.Value));
            }

            return result;
        }
    }
}
