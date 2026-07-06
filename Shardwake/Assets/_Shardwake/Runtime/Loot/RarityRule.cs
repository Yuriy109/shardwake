namespace Shardwake.Loot
{
    public readonly struct RarityRule
    {
        public RarityRule(int affixPoints, int guaranteedMinimumSingleStat, bool canRollSpecialAffix, bool hasUniqueEffect, bool hasAiFlavor)
        {
            AffixPoints = affixPoints;
            GuaranteedMinimumSingleStat = guaranteedMinimumSingleStat;
            CanRollSpecialAffix = canRollSpecialAffix;
            HasUniqueEffect = hasUniqueEffect;
            HasAiFlavor = hasAiFlavor;
        }

        public int AffixPoints { get; }
        public int GuaranteedMinimumSingleStat { get; }
        public bool CanRollSpecialAffix { get; }
        public bool HasUniqueEffect { get; }
        public bool HasAiFlavor { get; }
    }
}
