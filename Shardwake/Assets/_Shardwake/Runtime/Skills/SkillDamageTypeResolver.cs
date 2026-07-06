using Shardwake.Combat;
using Shardwake.Weapons;

namespace Shardwake.Skills
{
    public static class SkillDamageTypeResolver
    {
        public static DamageType Resolve(WeaponSkillDefinition skill)
        {
            return skill.EffectType switch
            {
                SkillEffectType.PoisonHit => DamageType.Poison,
                SkillEffectType.ShieldPulse => DamageType.Magic,
                SkillEffectType.Projectile => DamageType.Magic,
                SkillEffectType.Trap => DamageType.Physical,
                SkillEffectType.Curse => DamageType.Shadow,
                SkillEffectType.Sustain => DamageType.Shadow,
                SkillEffectType.CorpseExplosion => DamageType.Shadow,
                SkillEffectType.Heal => DamageType.Holy,
                SkillEffectType.Shield => DamageType.Holy,
                _ => DamageType.Physical
            };
        }
    }
}
