using Shardwake.Weapons;

namespace Shardwake.StatusEffects
{
    public static class StatusEffectLibrary
    {
        public static StatusEffectDefinition FromSkill(WeaponSkillDefinition skill)
        {
            return skill.EffectType switch
            {
                SkillEffectType.PoisonHit => new StatusEffectDefinition(StatusEffectType.Poison, 5f, 4f, 1f),
                SkillEffectType.Root => new StatusEffectDefinition(StatusEffectType.Root, 1.2f, 1f, 1f),
                SkillEffectType.Trap => new StatusEffectDefinition(StatusEffectType.Root, 1.5f, 1f, 1f),
                SkillEffectType.Curse => new StatusEffectDefinition(StatusEffectType.Weakness, 5f, 0.2f, 1f),
                SkillEffectType.Shield => new StatusEffectDefinition(StatusEffectType.Shield, 5f, 30f, 1f),
                SkillEffectType.GuardZone => new StatusEffectDefinition(StatusEffectType.Shield, 4f, 18f, 1f),
                _ => default
            };
        }
    }
}
