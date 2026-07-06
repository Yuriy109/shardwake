namespace Shardwake.Weapons
{
    public readonly struct WeaponSkillDefinition
    {
        public WeaponSkillDefinition(string id, string displayName, string description, SkillEffectType effectType, float baseDamage, float radius, float cooldown, string scalingStat)
        {
            Id = id;
            DisplayName = displayName;
            Description = description;
            EffectType = effectType;
            BaseDamage = baseDamage;
            Radius = radius;
            Cooldown = cooldown;
            ScalingStat = scalingStat;
        }

        public string Id { get; }
        public string DisplayName { get; }
        public string Description { get; }
        public SkillEffectType EffectType { get; }
        public float BaseDamage { get; }
        public float Radius { get; }
        public float Cooldown { get; }
        public string ScalingStat { get; }
    }
}
