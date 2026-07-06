using Shardwake.Combat;
using Shardwake.Equipment;
using Shardwake.Stats;
using UnityEngine;

namespace Shardwake.Player
{
    [RequireComponent(typeof(Health))]
    public sealed class PlayerRegeneration : MonoBehaviour
    {
        [SerializeField] private float outOfCombatDelay = 5f;
        [SerializeField] private float tickInterval = 1f;

        private Health health;
        private EquipmentLoadout equipmentLoadout;
        private float nextTickTime;

        private void Awake()
        {
            health = GetComponent<Health>();
            equipmentLoadout = GetComponent<EquipmentLoadout>();
        }

        private void Update()
        {
            if (health == null || health.IsDead || Time.time < nextTickTime)
            {
                return;
            }

            nextTickTime = Time.time + tickInterval;

            if (Time.time - health.LastDamageTime < outOfCombatDelay)
            {
                return;
            }

            var stats = equipmentLoadout != null ? equipmentLoadout.Stats : CharacterStats.Baseline;
            var amount = StatScaling.HpRegenPerSecond(stats.Stamina) * tickInterval;
            if (amount > 0f)
            {
                health.Heal(amount, showText: false);
            }
        }
    }
}
