using Shardwake.Combat;
using Shardwake.StatusEffects;
using Shardwake.Rendering;
using Shardwake.UI;
using Shardwake.Weapons;
using UnityEngine;

namespace Shardwake.Skills
{
    public static class SkillExecutor
    {
        public static void Execute(SkillExecutionContext context)
        {
            var skill = context.Skill;

            switch (skill.EffectType)
            {
                case SkillEffectType.Heal:
                    HealSelf(context);
                    return;

                case SkillEffectType.Shield:
                case SkillEffectType.GuardZone:
                case SkillEffectType.SelfBuff:
                case SkillEffectType.Smoke:
                    ApplySelfStatus(context);
                    return;

                case SkillEffectType.Cleanse:
                    FloatingText.Spawn(context.CasterTransform.position + Vector3.up * 2f, "CLEANSE", new Color(0.75f, 1f, 0.95f), 22);
                    return;

                case SkillEffectType.Backstep:
                    MoveCaster(context, -context.CasterTransform.forward, 2.4f, "BACKSTEP");
                    return;

                case SkillEffectType.Blink:
                    MoveCaster(context, context.CasterTransform.forward, 3.2f, "BLINK");
                    return;

                case SkillEffectType.DashHit:
                    MoveCaster(context, context.CasterTransform.forward, 2.6f, "DASH");
                    DealDamageToTargets(context);
                    return;

                case SkillEffectType.Pull:
                    PullNearestTarget(context);
                    DealDamageToTargets(context);
                    return;

                case SkillEffectType.Knockback:
                    DealDamageToTargets(context);
                    KnockbackTargets(context);
                    return;

                case SkillEffectType.Trap:
                    FloatingText.Spawn(context.CasterTransform.position + context.CasterTransform.forward * 2f + Vector3.up * 1.6f, "TRAP", new Color(0.55f, 0.9f, 0.55f), 22);
                    DealDamageToTargets(context);
                    return;

                case SkillEffectType.Curse:
                    DealDamageToTargets(context);
                    FloatingText.Spawn(context.CasterTransform.position + Vector3.up * 2f, "CURSE", new Color(0.7f, 0.45f, 1f), 22);
                    return;

                case SkillEffectType.Sustain:
                    DealDamageToTargets(context);
                    context.Caster.GetComponent<Health>()?.Heal(Mathf.Max(8f, context.Skill.BaseDamage * 0.45f), true);
                    return;

                case SkillEffectType.Summon:
                    FloatingText.Spawn(context.CasterTransform.position + Vector3.up * 2f, "SUMMON", new Color(0.7f, 0.55f, 1f), 22);
                    CreateTemporarySummon(context);
                    return;

                default:
                    DealDamageToTargets(context);
                    return;
            }
        }

        private static void CreateTemporarySummon(SkillExecutionContext context)
        {
            var summon = GameObject.CreatePrimitive(PrimitiveType.Capsule);
            summon.name = "Temporary Skeleton Ally";
            summon.transform.position = context.CasterTransform.position - context.CasterTransform.right * 1.4f;
            summon.transform.localScale = new Vector3(0.65f, 0.85f, 0.65f);
            GreyboxMaterial.Apply(summon.GetComponent<Renderer>(), new Color(0.78f, 0.78f, 0.68f));
            Object.Destroy(summon, 8f);
        }

        private static void HealSelf(SkillExecutionContext context)
        {
            var amount = Mathf.Max(15f, context.Skill.BaseDamage * Mathf.Max(1f, context.DamageMultiplier));
            context.Caster.GetComponent<Health>()?.Heal(amount);
        }

        private static void ApplySelfStatus(SkillExecutionContext context)
        {
            var statusEffect = StatusEffectLibrary.FromSkill(context.Skill);
            if (!statusEffect.IsValid)
            {
                FloatingText.Spawn(context.CasterTransform.position + Vector3.up * 2f, context.Skill.DisplayName, new Color(0.65f, 0.75f, 1f), 22);
                return;
            }

            if (!context.Caster.TryGetComponent(out StatusEffectController statusController))
            {
                statusController = context.Caster.AddComponent<StatusEffectController>();
            }

            statusController.Apply(statusEffect);
        }

        private static void DealDamageToTargets(SkillExecutionContext context)
        {
            var skill = context.Skill;
            var statusEffect = StatusEffectLibrary.FromSkill(skill);
            var damageType = SkillDamageTypeResolver.Resolve(skill);
            var damage = Mathf.Max(0f, skill.BaseDamage * Mathf.Max(1f, context.DamageMultiplier));

            if (damage <= 0f && !statusEffect.IsValid)
            {
                return;
            }

            foreach (var target in SkillTargeting.FindTargets(context))
            {
                target.TakeDamage(new DamageRequest(damage, damageType, context.Caster, statusEffect));
            }
        }

        private static void PullNearestTarget(SkillExecutionContext context)
        {
            var target = SkillTargeting.FindNearestTarget(context);
            if (target == null)
            {
                return;
            }

            var targetTransform = target.transform;
            var direction = context.CasterTransform.position - targetTransform.position;
            direction.y = 0f;

            if (direction.sqrMagnitude <= 0.01f)
            {
                return;
            }

            var pullDistance = Mathf.Min(2.8f, direction.magnitude - 0.9f);
            if (pullDistance <= 0f)
            {
                return;
            }

            var destination = targetTransform.position + direction.normalized * pullDistance;
            if (target.TryGetComponent(out Rigidbody body))
            {
                body.MovePosition(destination);
            }
            else
            {
                targetTransform.position = destination;
            }

            FloatingText.Spawn(targetTransform.position + Vector3.up * 2f, "PULLED", new Color(0.8f, 0.85f, 1f), 22);
        }

        private static void KnockbackTargets(SkillExecutionContext context)
        {
            foreach (var target in SkillTargeting.FindTargets(context))
            {
                var direction = target.transform.position - context.CasterTransform.position;
                direction.y = 0f;

                if (direction.sqrMagnitude <= 0.01f)
                {
                    direction = context.CasterTransform.forward;
                }

                var destination = target.transform.position + direction.normalized * 2.2f;
                if (target.TryGetComponent(out Rigidbody body))
                {
                    body.MovePosition(destination);
                }
                else
                {
                    target.transform.position = destination;
                }
            }
        }

        private static void MoveCaster(SkillExecutionContext context, Vector3 direction, float distance, string label)
        {
            direction.y = 0f;
            if (direction.sqrMagnitude <= 0.01f)
            {
                direction = context.CasterTransform.forward;
            }

            var destination = context.CasterTransform.position + direction.normalized * distance;
            if (context.Caster.TryGetComponent(out Rigidbody body))
            {
                body.MovePosition(destination);
            }
            else
            {
                context.CasterTransform.position = destination;
            }

            FloatingText.Spawn(context.CasterTransform.position + Vector3.up * 2f, label, new Color(0.55f, 0.85f, 1f), 22);
        }
    }
}
