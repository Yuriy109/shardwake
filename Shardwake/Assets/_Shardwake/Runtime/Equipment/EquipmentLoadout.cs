using System.Collections.Generic;
using Shardwake.Loot;
using Shardwake.Stats;
using UnityEngine;

namespace Shardwake.Equipment
{
    /// <summary>
    /// Runtime equipment state for the greybox player.
    /// Armor slots affect equip load. All equipped item affixes can contribute core stats.
    /// </summary>
    [DisallowMultipleComponent]
    public sealed class EquipmentLoadout : MonoBehaviour
    {
        [Header("Starter Armor Preset")]
        [SerializeField] private ArmorType head = ArmorType.Medium;
        [SerializeField] private ArmorType chest = ArmorType.Medium;
        [SerializeField] private ArmorType hands = ArmorType.Medium;
        [SerializeField] private ArmorType legs = ArmorType.Medium;
        [SerializeField] private ArmorType feet = ArmorType.Medium;

        [Header("Temporary Greybox Stat Bonuses")]
        [SerializeField, Min(0)] private int strength;
        [SerializeField, Min(0)] private int dexterity;
        [SerializeField, Min(0)] private int intelligence;
        [SerializeField, Min(0)] private int agility;
        [SerializeField, Min(0)] private int vitality;
        [SerializeField, Min(0)] private int focus;
        [SerializeField, Min(0)] private int stamina;

        private readonly Dictionary<EquipmentSlot, LootItem> equippedItems = new();

        public ArmorType Head => GetArmorType(EquipmentSlot.Head, head);
        public ArmorType Chest => GetArmorType(EquipmentSlot.Chest, chest);
        public ArmorType Hands => GetArmorType(EquipmentSlot.Hands, hands);
        public ArmorType Legs => GetArmorType(EquipmentSlot.Legs, legs);
        public ArmorType Feet => GetArmorType(EquipmentSlot.Feet, feet);
        public IReadOnlyDictionary<EquipmentSlot, LootItem> EquippedItems => equippedItems;

        public CharacterStats Stats => GetTotalStats();

        public List<LootItem> GetAllEquippedItems()
        {
            return new List<LootItem>(equippedItems.Values);
        }

        public void ClearEquippedItems()
        {
            equippedItems.Clear();
        }

        public float EquipLoad => EquipLoadCalculator.GetArmorWeight(Head, Chest, Hands, Legs, Feet);
        public LoadCategory LoadCategory => EquipLoadCalculator.GetLoadCategory(EquipLoad);
        public LoadEffects LoadEffects => LoadEffectRules.Get(LoadCategory);

        public bool TryEquip(LootItem item)
        {
            return TryEquip(item, out _, out _);
        }

        public bool TryEquip(LootItem item, out LootItem replacedItem, out bool hasReplacedItem)
        {
            replacedItem = default;
            hasReplacedItem = false;

            if (!item.IsEquipment)
            {
                Debug.Log($"Cannot equip non-equipment item: {item.DisplayName}");
                return false;
            }

            var slot = ResolveSlot(item);
            hasReplacedItem = equippedItems.TryGetValue(slot, out replacedItem);
            equippedItems[slot] = item;
            Debug.Log($"Equipped {item.DisplayName} in {slot}: {item.GetStatLine()}");
            return true;
        }

        public bool TryGetEquipped(EquipmentSlot slot, out LootItem item)
        {
            return equippedItems.TryGetValue(slot, out item);
        }

        public bool TryUnequip(EquipmentSlot slot, out LootItem item)
        {
            if (!equippedItems.TryGetValue(slot, out item))
            {
                return false;
            }

            equippedItems.Remove(slot);
            return true;
        }

        public void SetArmorPreset(ArmorType armorType)
        {
            head = armorType;
            chest = armorType;
            hands = armorType;
            legs = armorType;
            feet = armorType;
        }

        public void SetTemporaryStats(CharacterStats value)
        {
            strength = Mathf.Max(0, value.Strength);
            dexterity = Mathf.Max(0, value.Dexterity);
            intelligence = Mathf.Max(0, value.Intelligence);
            agility = Mathf.Max(0, value.Agility);
            vitality = Mathf.Max(0, value.Vitality);
            focus = Mathf.Max(0, value.Focus);
            stamina = Mathf.Max(0, value.Stamina);
        }

        private CharacterStats GetTotalStats()
        {
            var total = new CharacterStats(strength, dexterity, intelligence, agility, vitality, focus, stamina);

            foreach (var pair in equippedItems)
            {
                var item = pair.Value;
                for (var i = 0; i < item.Affixes.Count; i++)
                {
                    if (TryConvertCoreStat(item.Affixes[i].Stat, out var coreStat))
                    {
                        total = total.Add(coreStat, item.Affixes[i].Value);
                    }
                }
            }

            return total;
        }

        private ArmorType GetArmorType(EquipmentSlot slot, ArmorType fallback)
        {
            if (equippedItems.TryGetValue(slot, out var item) && item.IsArmor)
            {
                return item.ArmorType;
            }

            return fallback;
        }

        private EquipmentSlot ResolveSlot(LootItem item)
        {
            if (item.IsWeapon)
            {
                if (!equippedItems.ContainsKey(EquipmentSlot.Weapon1))
                {
                    return EquipmentSlot.Weapon1;
                }

                if (!equippedItems.ContainsKey(EquipmentSlot.Weapon2))
                {
                    return EquipmentSlot.Weapon2;
                }

                return EquipmentSlot.Weapon1;
            }

            if (item.Category == LootItemCategory.Ring)
            {
                if (!equippedItems.ContainsKey(EquipmentSlot.Ring1))
                {
                    return EquipmentSlot.Ring1;
                }

                if (!equippedItems.ContainsKey(EquipmentSlot.Ring2))
                {
                    return EquipmentSlot.Ring2;
                }

                return EquipmentSlot.Ring1;
            }

            return item.EquipmentSlot;
        }

        private static bool TryConvertCoreStat(LootStat stat, out CoreStat coreStat)
        {
            switch (stat)
            {
                case LootStat.Strength:
                    coreStat = CoreStat.Strength;
                    return true;
                case LootStat.Dexterity:
                    coreStat = CoreStat.Dexterity;
                    return true;
                case LootStat.Intelligence:
                    coreStat = CoreStat.Intelligence;
                    return true;
                case LootStat.Agility:
                    coreStat = CoreStat.Agility;
                    return true;
                case LootStat.Vitality:
                    coreStat = CoreStat.Vitality;
                    return true;
                case LootStat.Focus:
                    coreStat = CoreStat.Focus;
                    return true;
                case LootStat.Stamina:
                    coreStat = CoreStat.Stamina;
                    return true;
                default:
                    coreStat = CoreStat.Strength;
                    return false;
            }
        }
    }
}
