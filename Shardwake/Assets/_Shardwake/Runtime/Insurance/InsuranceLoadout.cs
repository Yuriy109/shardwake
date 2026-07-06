using System.Collections.Generic;
using Shardwake.Equipment;
using Shardwake.Loot;
using UnityEngine;

namespace Shardwake.Insurance
{
    /// <summary>
    /// Greybox insurance state. The final game should let the player choose insured items in Haven.
    /// For now this can auto-insure up to two equipped non-starter items.
    /// </summary>
    public sealed class InsuranceLoadout : MonoBehaviour
    {
        private readonly List<LootItem> insuredItems = new();

        public IReadOnlyList<LootItem> InsuredItems => insuredItems;

        public void Clear()
        {
            insuredItems.Clear();
        }

        public void AutoInsureFromEquipment(EquipmentLoadout equipment)
        {
            insuredItems.Clear();
            if (equipment == null)
            {
                return;
            }

            var equipped = equipment.GetAllEquippedItems();
            for (var i = 0; i < equipped.Count && insuredItems.Count < InsuranceRules.MaxInsuredItemsPerRun; i++)
            {
                insuredItems.Add(equipped[i]);
            }
        }

        public bool IsInsured(LootItem item)
        {
            for (var i = 0; i < insuredItems.Count; i++)
            {
                if (insuredItems[i].DisplayName == item.DisplayName && insuredItems[i].Power == item.Power && insuredItems[i].Category == item.Category)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
