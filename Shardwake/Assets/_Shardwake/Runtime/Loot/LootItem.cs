using System.Collections.Generic;
using System.Text;
using Shardwake.Equipment;

namespace Shardwake.Loot
{
    public readonly struct LootItem
    {
        public LootItem(
            string name,
            ItemRarity rarity,
            int power,
            IReadOnlyList<LootAffix> affixes,
            string flavor,
            bool isUnstable,
            ItemSlotSize slotSize = ItemSlotSize.Small,
            LootItemCategory category = LootItemCategory.Relic,
            EquipmentSlot equipmentSlot = EquipmentSlot.Chest,
            ArmorType armorType = ArmorType.Medium)
        {
            Name = name;
            Rarity = rarity;
            Power = power;
            Affixes = affixes ?? System.Array.Empty<LootAffix>();
            Flavor = flavor;
            IsUnstable = isUnstable;
            SlotSize = slotSize;
            Category = category;
            EquipmentSlot = equipmentSlot;
            ArmorType = armorType;
        }

        public string Name { get; }
        public ItemRarity Rarity { get; }
        public int Power { get; }
        public IReadOnlyList<LootAffix> Affixes { get; }
        public string Flavor { get; }
        public bool IsUnstable { get; }
        public ItemSlotSize SlotSize { get; }
        public LootItemCategory Category { get; }
        public EquipmentSlot EquipmentSlot { get; }
        public ArmorType ArmorType { get; }
        public int Slots => (int)SlotSize;

        public bool IsEquipment => Category == LootItemCategory.Weapon || Category == LootItemCategory.Armor || Category == LootItemCategory.Ring || Category == LootItemCategory.Amulet;
        public bool IsArmor => Category == LootItemCategory.Armor;
        public bool IsWeapon => Category == LootItemCategory.Weapon;
        public bool IsAccessory => Category == LootItemCategory.Ring || Category == LootItemCategory.Amulet;

        public string DisplayName => IsUnstable ? $"{Rarity} Unstable {Name}" : $"{Rarity} {Name}";

        public string GetStatLine()
        {
            if (Affixes.Count == 0)
            {
                return "No affixes";
            }

            var builder = new StringBuilder();
            for (var i = 0; i < Affixes.Count; i++)
            {
                if (i > 0)
                {
                    builder.Append(", ");
                }

                builder.Append(Affixes[i]);
            }

            return builder.ToString();
        }
    }
}
