using System.Collections.Generic;
using Shardwake.Combat;
using UnityEngine;

namespace Shardwake.StatusEffects
{
    [DisallowMultipleComponent]
    public sealed class StatusEffectController : MonoBehaviour
    {
        private readonly List<StatusEffectInstance> activeEffects = new();

        public float ShieldValue { get; private set; }
        public bool IsRooted => HasEffect(StatusEffectType.Root) || HasEffect(StatusEffectType.Stun);
        public bool IsStunned => HasEffect(StatusEffectType.Stun);
        public float MoveSpeedMultiplier => IsRooted ? 0f : 1f - HighestMagnitude(StatusEffectType.Slow);
        public float DamageDealtMultiplier => 1f - HighestMagnitude(StatusEffectType.Weakness);

        private void Update()
        {
            if (activeEffects.Count == 0)
            {
                return;
            }

            for (var i = activeEffects.Count - 1; i >= 0; i--)
            {
                var effect = activeEffects[i];
                ApplyTick(effect);
                effect.Advance(Time.deltaTime);

                if (effect.IsExpired)
                {
                    if (effect.Definition.Type == StatusEffectType.Shield)
                    {
                        ShieldValue = Mathf.Max(0f, ShieldValue - effect.Definition.Magnitude);
                    }

                    activeEffects.RemoveAt(i);
                }
            }
        }

        public void Apply(StatusEffectDefinition definition)
        {
            if (!definition.IsValid)
            {
                return;
            }

            if (definition.Type == StatusEffectType.Shield)
            {
                ShieldValue += definition.Magnitude;
            }

            activeEffects.Add(new StatusEffectInstance(definition));
        }

        public float AbsorbShield(float damage)
        {
            if (damage <= 0f || ShieldValue <= 0f)
            {
                return 0f;
            }

            var absorbed = Mathf.Min(damage, ShieldValue);
            ShieldValue -= absorbed;
            return absorbed;
        }

        public bool HasEffect(StatusEffectType type)
        {
            foreach (var effect in activeEffects)
            {
                if (effect.Definition.Type == type && !effect.IsExpired)
                {
                    return true;
                }
            }

            return false;
        }

        private float HighestMagnitude(StatusEffectType type)
        {
            var result = 0f;
            foreach (var effect in activeEffects)
            {
                if (effect.Definition.Type == type && !effect.IsExpired)
                {
                    result = Mathf.Max(result, effect.Definition.Magnitude);
                }
            }

            return Mathf.Clamp01(result);
        }

        private void ApplyTick(StatusEffectInstance effect)
        {
            if (!effect.ShouldTick)
            {
                return;
            }

            switch (effect.Definition.Type)
            {
                case StatusEffectType.Burn:
                    GetComponent<Health>()?.TakeDamage(new DamageRequest(effect.Definition.Magnitude, DamageType.Fire, gameObject));
                    break;
                case StatusEffectType.Poison:
                    GetComponent<Health>()?.TakeDamage(new DamageRequest(effect.Definition.Magnitude, DamageType.Poison, gameObject));
                    break;
                case StatusEffectType.Bleed:
                    GetComponent<Health>()?.TakeDamage(new DamageRequest(effect.Definition.Magnitude, DamageType.Physical, gameObject));
                    break;
                case StatusEffectType.HealOverTime:
                    GetComponent<Health>()?.Heal(effect.Definition.Magnitude);
                    break;
            }

            effect.TickScheduled();
        }
    }
}
