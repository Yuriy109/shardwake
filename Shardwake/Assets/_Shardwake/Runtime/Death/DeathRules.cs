using System.Collections.Generic;
using Shardwake.Equipment;
using Shardwake.Inventory;
using Shardwake.Loot;

namespace Shardwake.Death
{
    public static class DeathRules
    {
        public const bool EquippedItemsDrop = true;
        public const bool BackpackItemsDrop = true;
        public const bool StarterGearCanBeReclaimed = true;

        public static List<LootItem> CollectDroppedItems(EquipmentLoadout equipment, InventoryComponent inventory)
        {
            var dropped = new List<LootItem>();

            if (EquippedItemsDrop && equipment != null)
            {
                dropped.AddRange(equipment.GetAllEquippedItems());
            }

            if (BackpackItemsDrop && inventory != null)
            {
                dropped.AddRange(inventory.Items);
            }

            return dropped;
        }
    }
}
