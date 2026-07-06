using Shardwake.Core;
using Shardwake.StatusEffects;
using Shardwake.UI;
using UnityEngine;

namespace Shardwake.Combat
{
    public sealed class Health : MonoBehaviour
    {
        [SerializeField] private float maxHealth = 100f;
        [SerializeField] private float armor;
        [SerializeField] private float magicResistance;

        public float MaxHealth => maxHealth;
        public float CurrentHealth { get; private set; }
        public float Normalized => maxHealth <= 0f ? 0f : CurrentHealth / maxHealth;
        public bool IsDead => CurrentHealth <= 0f;
        public float LastDamageTime { get; private set; } = -999f;
        public float Armor => armor;
        public float MagicResistance => magicResistance;

        private void Awake()
        {
            CurrentHealth = maxHealth;
        }

        public void SetMaxHealth(float value)
        {
            maxHealth = Mathf.Max(1f, value);
            CurrentHealth = maxHealth;
        }

        public void ConfigureDefenses(float physicalArmor, float resistance)
        {
            armor = Mathf.Max(0f, physicalArmor);
            magicResistance = Mathf.Max(0f, resistance);
        }

        public void Heal(float amount, bool showText = true)
        {
            if (IsDead || amount <= 0f || CurrentHealth >= maxHealth)
            {
                return;
            }

            var healed = Mathf.Min(amount, maxHealth - CurrentHealth);
            CurrentHealth += healed;

            if (showText && healed > 0f)
            {
                FloatingText.Spawn(transform.position + Vector3.up * 2.25f, $"+{Mathf.CeilToInt(healed)}", new Color(0.35f, 1f, 0.55f));
            }
        }

        public void TakeDamage(float amount)
        {
            TakeDamage(DamageRequest.Physical(amount));
        }

        public DamageResult TakeDamage(DamageRequest request)
        {
            if (IsDead)
            {
                return new DamageResult(request.Amount, 0f, 0f, request.DamageType);
            }

            var mitigated = DamageCalculator.Mitigate(request.Amount, request.DamageType, armor, magicResistance);
            var absorbed = 0f;

            if (TryGetComponent(out StatusEffectController statusEffects))
            {
                absorbed = statusEffects.AbsorbShield(mitigated);
            }

            var damage = Mathf.Max(0f, mitigated - absorbed);
            if (damage > 0f)
            {
                LastDamageTime = Time.time;
            }

            CurrentHealth = Mathf.Max(0f, CurrentHealth - damage);

            if (request.HasStatusEffect && !IsDead)
            {
                if (!TryGetComponent(out statusEffects))
                {
                    statusEffects = gameObject.AddComponent<StatusEffectController>();
                }

                statusEffects.Apply(request.StatusEffect);
            }

            if (absorbed > 0f)
            {
                FloatingText.Spawn(transform.position + Vector3.up * 2.55f, $"-{Mathf.CeilToInt(absorbed)} SHIELD", new Color(0.45f, 0.75f, 1f), 20);
            }

            if (damage > 0f)
            {
                GetComponent<HitFeedback>()?.Play();
                FloatingText.Spawn(transform.position + Vector3.up * 2.25f, $"-{Mathf.CeilToInt(damage)}", new Color(1f, 0.48f, 0.28f));
            }

            if (IsDead)
            {
                ShardwakeSession.Instance?.RecordDeath(gameObject);
                FloatingText.Spawn(transform.position + Vector3.up * 1.4f, "DOWN", new Color(1f, 0.85f, 0.35f), 28);
                Destroy(gameObject);
            }

            return new DamageResult(request.Amount, damage, absorbed, request.DamageType);
        }
    }
}
