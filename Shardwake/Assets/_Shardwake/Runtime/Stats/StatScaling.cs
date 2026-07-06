using UnityEngine;

namespace Shardwake.Stats
{
    public static class StatScaling
    {
        public static float MaxHealthMultiplier(int vitality)
        {
            return 1f + Mathf.Max(0, vitality) * 0.03f;
        }

        public static float BonusHealthFromStrength(int strength)
        {
            return Mathf.Max(0, strength) * 1.5f;
        }

        public static float CooldownMultiplier(int focus)
        {
            return 1f / (1f + Mathf.Max(0, focus) * 0.025f);
        }

        public static float SkillActivationSpeedMultiplier(int focus)
        {
            return 1f + Mathf.Max(0, focus) * 0.01f;
        }

        public static float MaxEnergyBonus(int stamina)
        {
            return Mathf.Max(0, stamina) * 4f;
        }

        public static float HpRegenPerSecond(int stamina)
        {
            return Mathf.Max(0, stamina) * 0.08f;
        }

        public static float MagicResistanceBonus(int intelligence)
        {
            return Mathf.Max(0, intelligence) * 0.5f;
        }

        public static float PhysicalResistanceBonus(int vitality)
        {
            return Mathf.Max(0, vitality) * 0.4f;
        }

        public static float MovementSpeedMultiplier(int agility)
        {
            return 1f + Mathf.Max(0, agility) * 0.003f;
        }

        public static float BasicAttackCooldownMultiplier(int agility)
        {
            return 1f / (1f + Mathf.Max(0, agility) * 0.015f);
        }

        public static float CritChanceBonus(int dexterity)
        {
            return Mathf.Max(0, dexterity) * 0.0025f;
        }
    }
}
