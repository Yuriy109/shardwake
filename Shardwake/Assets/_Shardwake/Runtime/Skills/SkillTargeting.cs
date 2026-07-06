using System.Collections.Generic;
using Shardwake.Combat;
using Shardwake.Weapons;
using UnityEngine;

namespace Shardwake.Skills
{
    public static class SkillTargeting
    {
        public static List<Health> FindTargets(SkillExecutionContext context)
        {
            var targets = new List<Health>();
            var skill = context.Skill;
            if (skill.Radius <= 0f)
            {
                return targets;
            }

            var hits = Physics.OverlapSphere(context.CasterTransform.position, skill.Radius);
            foreach (var hit in hits)
            {
                if (hit.attachedRigidbody != null && hit.attachedRigidbody.gameObject == context.Caster)
                {
                    continue;
                }

                if (!hit.TryGetComponent(out Health health) || health.IsDead)
                {
                    continue;
                }

                if (!IsValidForShape(context, health.transform.position))
                {
                    continue;
                }

                targets.Add(health);
            }

            return targets;
        }

        public static Health FindNearestTarget(SkillExecutionContext context)
        {
            Health best = null;
            var bestDistance = float.MaxValue;

            foreach (var target in FindTargets(context))
            {
                var distance = Vector3.SqrMagnitude(target.transform.position - context.CasterTransform.position);
                if (distance < bestDistance)
                {
                    bestDistance = distance;
                    best = target;
                }
            }

            return best;
        }

        private static bool IsValidForShape(SkillExecutionContext context, Vector3 targetPosition)
        {
            var effectType = context.Skill.EffectType;
            if (effectType == SkillEffectType.AreaHit ||
                effectType == SkillEffectType.GroundSlam ||
                effectType == SkillEffectType.ShieldPulse ||
                effectType == SkillEffectType.GuardZone ||
                effectType == SkillEffectType.CorpseExplosion)
            {
                return true;
            }

            var toTarget = targetPosition - context.CasterTransform.position;
            toTarget.y = 0f;

            if (toTarget.sqrMagnitude <= 0.01f)
            {
                return true;
            }

            var angle = Vector3.Angle(context.CasterTransform.forward, toTarget.normalized);
            var coneAngle = IsNarrowSkill(effectType) ? 35f : 70f;
            return angle <= coneAngle;
        }

        private static bool IsNarrowSkill(SkillEffectType effectType)
        {
            return effectType == SkillEffectType.RangedHit ||
                   effectType == SkillEffectType.Projectile ||
                   effectType == SkillEffectType.Pull ||
                   effectType == SkillEffectType.Root ||
                   effectType == SkillEffectType.Trap;
        }
    }
}
