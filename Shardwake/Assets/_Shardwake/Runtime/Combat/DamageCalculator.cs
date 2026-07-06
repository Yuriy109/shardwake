using UnityEngine;

namespace Shardwake.Combat
{
    public static class DamageCalculator
    {
        public static float Mitigate(float rawDamage, DamageType damageType, float armor, float magicResistance)
        {
            var damage = Mathf.Max(0f, rawDamage);
            if (damage <= 0f || damageType == DamageType.True)
            {
                return damage;
            }

            var resistance = UsesArmor(damageType) ? armor : magicResistance;
            return damage * (100f / (100f + Mathf.Max(0f, resistance)));
        }

        public static bool UsesArmor(DamageType damageType)
        {
            return damageType == DamageType.Physical || damageType == DamageType.Poison;
        }
    }
}
