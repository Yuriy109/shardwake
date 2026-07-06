using Shardwake.Core;
using Shardwake.Inventory;
using Shardwake.Rendering;
using Shardwake.UI;
using UnityEngine;

namespace Shardwake.Loot
{
    public sealed class LootChest : MonoBehaviour
    {
        [SerializeField] private float openRadius = 2.2f;

        private bool opened;

        private void Update()
        {
            if (ShardwakeSession.Instance != null && !ShardwakeSession.Instance.HasStarted)
            {
                return;
            }

            if (opened)
            {
                return;
            }

            var player = Object.FindFirstObjectByType<InventoryComponent>();
            if (player == null)
            {
                return;
            }

            var distance = Vector3.Distance(transform.position, player.transform.position);
            if (distance > openRadius)
            {
                return;
            }

            opened = true;
            var item = LootGenerator.Roll();
            if (!player.TryAdd(item))
            {
                opened = false;
                FloatingText.Spawn(transform.position + Vector3.up * 1.6f, "BACKPACK FULL", Color.white, 28);
                return;
            }

            ShardwakeSession.Instance?.RecordLoot(item);
            var prefix = item.IsUnstable ? "+ UNSTABLE" : $"+ {item.Rarity}";
            FloatingText.Spawn(transform.position + Vector3.up * 1.6f, $"{prefix} P{item.Power}", RarityColor(item.Rarity), 28);
            GreyboxMaterial.Apply(GetComponent<Renderer>(), new Color(0.35f, 0.26f, 0.15f));
        }

        private static Color RarityColor(ItemRarity rarity)
        {
            return rarity switch
            {
                ItemRarity.Uncommon => new Color(0.42f, 1f, 0.48f),
                ItemRarity.Rare => new Color(0.32f, 0.68f, 1f),
                ItemRarity.Epic => new Color(0.78f, 0.42f, 1f),
                ItemRarity.Legendary => new Color(1f, 0.58f, 0.16f),
                _ => Color.white
            };
        }
    }
}
