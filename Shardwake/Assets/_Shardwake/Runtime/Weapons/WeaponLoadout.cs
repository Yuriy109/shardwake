namespace Shardwake.Weapons
{
    public sealed class WeaponLoadout
    {
        public WeaponLoadout(WeaponType weapon1, WeaponType weapon2)
        {
            Primary = new WeaponSkillLoadout(weapon1);
            Secondary = new WeaponSkillLoadout(weapon2);
            ActiveSlot = WeaponSlot.Primary;
        }

        public WeaponSkillLoadout Primary { get; private set; }
        public WeaponSkillLoadout Secondary { get; private set; }
        public WeaponSlot ActiveSlot { get; private set; }
        public WeaponType ActiveWeaponType => ActiveSlot == WeaponSlot.Primary ? Primary.WeaponType : Secondary.WeaponType;
        public WeaponType PrimaryWeaponType => Primary.WeaponType;
        public WeaponType SecondaryWeaponType => Secondary.WeaponType;

        public WeaponSkillLoadout ActiveSkillLoadout => ActiveSlot == WeaponSlot.Primary ? Primary : Secondary;

        public void ReplaceWeapons(WeaponType weapon1, WeaponType weapon2)
        {
            Primary = new WeaponSkillLoadout(weapon1);
            Secondary = new WeaponSkillLoadout(weapon2);
            ActiveSlot = WeaponSlot.Primary;
        }

        public void Swap()
        {
            ActiveSlot = ActiveSlot == WeaponSlot.Primary ? WeaponSlot.Secondary : WeaponSlot.Primary;
        }
    }
}
