using Shardwake.Weapons;
using UnityEngine;

namespace Shardwake.Skills
{
    public readonly struct SkillExecutionContext
    {
        public SkillExecutionContext(
            GameObject caster,
            Transform casterTransform,
            WeaponSkillDefinition skill,
            float damageMultiplier)
        {
            Caster = caster;
            CasterTransform = casterTransform;
            Skill = skill;
            DamageMultiplier = Mathf.Max(0f, damageMultiplier);
        }

        public GameObject Caster { get; }
        public Transform CasterTransform { get; }
        public WeaponSkillDefinition Skill { get; }
        public float DamageMultiplier { get; }
    }
}
