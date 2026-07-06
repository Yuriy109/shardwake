namespace Shardwake.Weapons
{
    public static class WeaponLoadoutRules
    {
        public const int MaxWeaponsPerRun = 2;
        public const int ActiveSkillsPerWeapon = 3;
        public const int PassiveSkillsPerWeapon = 1;
        public const float DefaultWeaponSwapCooldown = 5f;

        public static bool CanEquipTogether(WeaponType weapon1, WeaponType weapon2)
        {
            return weapon1 != weapon2;
        }
    }
}
