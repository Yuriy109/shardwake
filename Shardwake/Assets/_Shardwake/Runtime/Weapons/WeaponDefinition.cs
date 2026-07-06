namespace Shardwake.Weapons
{
    public sealed class WeaponDefinition
    {
        public WeaponDefinition(WeaponType type, string displayName, string role, string mainScaling, string secondaryScaling, float moveSpeed, float maxHealth, float basicDamage, float basicCooldown, float basicRadius, WeaponSkillDefinition[] activeSkills, WeaponPassiveDefinition[] passiveSkills)
        {
            Type = type;
            DisplayName = displayName;
            Role = role;
            MainScaling = mainScaling;
            SecondaryScaling = secondaryScaling;
            MoveSpeed = moveSpeed;
            MaxHealth = maxHealth;
            BasicDamage = basicDamage;
            BasicCooldown = basicCooldown;
            BasicRadius = basicRadius;
            ActiveSkills = activeSkills;
            PassiveSkills = passiveSkills;
        }

        public WeaponType Type { get; }
        public string DisplayName { get; }
        public string Role { get; }
        public string MainScaling { get; }
        public string SecondaryScaling { get; }
        public float MoveSpeed { get; }
        public float MaxHealth { get; }
        public float BasicDamage { get; }
        public float BasicCooldown { get; }
        public float BasicRadius { get; }
        public WeaponSkillDefinition[] ActiveSkills { get; }
        public WeaponPassiveDefinition[] PassiveSkills { get; }
    }
}
