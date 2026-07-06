using System;

namespace Shardwake.Weapons
{
    public sealed class WeaponSkillLoadout
    {
        public const int EquippedActiveSkillSlots = 3;

        private readonly int[] activeSkillIndexes = new int[EquippedActiveSkillSlots];

        public WeaponSkillLoadout(WeaponType weaponType)
            : this(weaponType, new[] { 0, 1, 2 }, 0)
        {
        }

        public WeaponSkillLoadout(WeaponType weaponType, int[] selectedActiveSkillIndexes, int selectedPassiveIndex)
        {
            WeaponType = weaponType;
            SetActiveSkills(selectedActiveSkillIndexes);
            SelectedPassiveIndex = Math.Max(0, selectedPassiveIndex);
        }

        public WeaponType WeaponType { get; }
        public int SelectedPassiveIndex { get; private set; }

        public int GetActiveSkillIndex(int equippedSlot)
        {
            if (equippedSlot < 0 || equippedSlot >= activeSkillIndexes.Length)
            {
                return -1;
            }

            return activeSkillIndexes[equippedSlot];
        }

        public WeaponSkillDefinition GetActiveSkill(int equippedSlot)
        {
            var definition = WeaponDefinitions.Get(WeaponType);
            var skillIndex = GetActiveSkillIndex(equippedSlot);
            if (skillIndex < 0 || skillIndex >= definition.ActiveSkills.Length)
            {
                return default;
            }

            return definition.ActiveSkills[skillIndex];
        }

        public WeaponPassiveDefinition GetPassive()
        {
            var definition = WeaponDefinitions.Get(WeaponType);
            if (definition.PassiveSkills.Length == 0)
            {
                return default;
            }

            var index = Math.Min(SelectedPassiveIndex, definition.PassiveSkills.Length - 1);
            return definition.PassiveSkills[index];
        }

        public void SetActiveSkills(int[] selectedActiveSkillIndexes)
        {
            if (selectedActiveSkillIndexes == null || selectedActiveSkillIndexes.Length == 0)
            {
                for (var i = 0; i < activeSkillIndexes.Length; i++)
                {
                    activeSkillIndexes[i] = i;
                }

                return;
            }

            for (var i = 0; i < activeSkillIndexes.Length; i++)
            {
                activeSkillIndexes[i] = i < selectedActiveSkillIndexes.Length ? Math.Max(0, selectedActiveSkillIndexes[i]) : i;
            }
        }

        public void SetPassive(int selectedPassiveIndex)
        {
            SelectedPassiveIndex = Math.Max(0, selectedPassiveIndex);
        }
    }
}
