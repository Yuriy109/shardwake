using Shardwake.Core;
using Shardwake.Inventory;
using Shardwake.Loot;
using Shardwake.UI;
using UnityEngine;

namespace Shardwake.Monsters
{
    /// <summary>
    /// Greybox mini-boss marker and reward hook.
    /// Keeps mini-boss reward logic out of generic EnemyController.
    /// </summary>
    public sealed class MiniBossController : MonoBehaviour
    {
        [SerializeField] private int rewardRolls = 2;
        [SerializeField] private LootSource rewardSource = LootSource.MiniBoss;

        public int RewardRolls => Mathf.Max(1, rewardRolls);
        public LootSource RewardSource => rewardSource;

        public void Configure(int rolls, LootSource source)
        {
            rewardRolls = Mathf.Max(1, rolls);
            rewardSource = source;
        }

        public void GrantRewardsToNearestPlayer()
        {
            var inventory = Object.FindFirstObjectByType<InventoryComponent>();
            if (inventory == null)
            {
                return;
            }

            var granted = 0;
            for (var i = 0; i < RewardRolls; i++)
            {
                var item = LootGenerator.Roll(rewardSource);
                if (!inventory.TryAdd(item))
                {
                    FloatingText.Spawn(transform.position + Vector3.up * 2.4f, "BOSS LOOT FULL", Color.white, 24);
                    continue;
                }

                ShardwakeSession.Instance?.RecordLoot(item);
                granted++;
            }

            if (granted > 0)
            {
                FloatingText.Spawn(transform.position + Vector3.up * 2.8f, $"BOSS LOOT x{granted}", new Color(1f, 0.75f, 0.18f), 30);
            }
        }
    }
}
