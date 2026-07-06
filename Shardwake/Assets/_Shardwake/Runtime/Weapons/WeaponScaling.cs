using Shardwake.Stats;

namespace Shardwake.Weapons
{
    public static class WeaponScaling
    {
        public static float BasicDamageMultiplier(WeaponType weaponType, CharacterStats stats)
        {
            var scalingStat = weaponType switch
            {
                WeaponType.GreatWeapon => stats.Strength,
                WeaponType.SwordAndShield => stats.Strength,
                WeaponType.Daggers => (stats.Agility + stats.Strength) * 0.5f,
                WeaponType.MonkBattleStaff => (stats.Agility + stats.Strength) * 0.5f,
                WeaponType.Bow => stats.Dexterity,
                WeaponType.MageStaff => stats.Intelligence,
                WeaponType.HolyRelic => stats.Intelligence,
                WeaponType.NecromancerTome => stats.Intelligence,
                _ => 0f
            };

            return 1f + scalingStat * 0.025f;
        }

        public static float SkillDamageMultiplier(WeaponSkillDefinition skill, CharacterStats stats)
        {
            var scalingKey = string.IsNullOrWhiteSpace(skill.ScalingStat) ? string.Empty : skill.ScalingStat.ToLowerInvariant();
            var value = scalingKey switch
            {
                "strength" => stats.Strength,
                "dexterity" => stats.Dexterity,
                "intelligence" => stats.Intelligence,
                "agility" => stats.Agility,
                "vitality" => stats.Vitality,
                "focus" => stats.Focus,
                "stamina" => stats.Stamina,
                _ => 0
            };

            return 1f + value * 0.02f;
        }
    }
}
