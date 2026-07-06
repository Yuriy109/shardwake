namespace Shardwake.Combat
{
    public readonly struct DamageResult
    {
        public DamageResult(float rawDamage, float finalDamage, float absorbedByShield, DamageType damageType)
        {
            RawDamage = rawDamage;
            FinalDamage = finalDamage;
            AbsorbedByShield = absorbedByShield;
            DamageType = damageType;
        }

        public float RawDamage { get; }
        public float FinalDamage { get; }
        public float AbsorbedByShield { get; }
        public DamageType DamageType { get; }
        public bool DidDamage => FinalDamage > 0f;
    }
}
