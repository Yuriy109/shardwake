using Shardwake.StatusEffects;
using UnityEngine;

namespace Shardwake.Combat
{
    public readonly struct DamageRequest
    {
        public DamageRequest(float amount, DamageType damageType, GameObject source = null, StatusEffectDefinition statusEffect = default)
        {
            Amount = Mathf.Max(0f, amount);
            DamageType = damageType;
            Source = source;
            StatusEffect = statusEffect;
        }

        public float Amount { get; }
        public DamageType DamageType { get; }
        public GameObject Source { get; }
        public StatusEffectDefinition StatusEffect { get; }
        public bool HasStatusEffect => StatusEffect.IsValid;

        public static DamageRequest Physical(float amount, GameObject source = null)
        {
            return new DamageRequest(amount, DamageType.Physical, source);
        }

        public static DamageRequest Magic(float amount, GameObject source = null)
        {
            return new DamageRequest(amount, DamageType.Magic, source);
        }
    }
}
