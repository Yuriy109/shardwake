using System.Collections.Generic;
using Shardwake.Core;
using Shardwake.Equipment;
using Shardwake.Loot;
using UnityEngine;

namespace Shardwake.Inventory
{
    public sealed class InventoryComponent : MonoBehaviour
    {
        [SerializeField] private int maxSlots = 16;

        private readonly List<LootItem> items = new();

        public int MaxSlots => maxSlots;
        public int UsedSlots { get; private set; }
        public int FreeSlots => Mathf.Max(0, maxSlots - UsedSlots);
        public int ItemCount => items.Count;
        public IReadOnlyList<LootItem> Items => items;

        public bool CanAdd(LootItem item)
        {
            return UsedSlots + item.Slots <= maxSlots;
        }

        public bool TryAdd(LootItem item)
        {
            if (!CanAdd(item))
            {
                Debug.Log($"Backpack full: cannot pick up {item.DisplayName}");
                return false;
            }

            Add(item);
            return true;
        }

        public void Add(LootItem item)
        {
            items.Add(item);
            UsedSlots += item.Slots;
            Debug.Log($"Loot acquired: {item.DisplayName} [{item.Category}] P{item.Power} {item.GetStatLine()} ({item.Slots} slots)");
        }

        public List<LootItem> TakeAll()
        {
            var result = new List<LootItem>(items);
            items.Clear();
            UsedSlots = 0;
            return result;
        }

        public bool TryRemoveAt(int index, out LootItem item)
        {
            item = default;
            if (index < 0 || index >= items.Count)
            {
                return false;
            }

            item = items[index];
            items.RemoveAt(index);
            UsedSlots = Mathf.Max(0, UsedSlots - item.Slots);
            return true;
        }

        public bool TryEquipFromBackpack(int index, EquipmentLoadout loadout)
        {
            if (loadout == null || !TryRemoveAt(index, out var item))
            {
                return false;
            }

            if (!loadout.TryEquip(item, out var replacedItem, out var hasReplacedItem))
            {
                Add(item);
                return false;
            }

            if (hasReplacedItem)
            {
                Add(replacedItem);
            }

            ShardwakeSession.Instance?.RefreshPlayerBuild();
            return true;
        }

        public bool TryEquipFirstEquipment(EquipmentLoadout loadout)
        {
            for (var i = 0; i < items.Count; i++)
            {
                if (items[i].IsEquipment)
                {
                    return TryEquipFromBackpack(i, loadout);
                }
            }

            Debug.Log("No equipment item found in backpack.");
            return false;
        }
    }
}
