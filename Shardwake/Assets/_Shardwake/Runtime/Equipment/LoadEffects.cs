namespace Shardwake.Equipment
{
    public readonly struct LoadEffects
    {
        public LoadEffects(float rollDistanceMultiplier, float rollCooldownMultiplier, float energyRecoveryMultiplier, float movementSpeedMultiplier, float staggerResistance)
        {
            RollDistanceMultiplier = rollDistanceMultiplier;
            RollCooldownMultiplier = rollCooldownMultiplier;
            EnergyRecoveryMultiplier = energyRecoveryMultiplier;
            MovementSpeedMultiplier = movementSpeedMultiplier;
            StaggerResistance = staggerResistance;
        }

        public float RollDistanceMultiplier { get; }
        public float RollCooldownMultiplier { get; }
        public float EnergyRecoveryMultiplier { get; }
        public float MovementSpeedMultiplier { get; }
        public float StaggerResistance { get; }
    }
}
