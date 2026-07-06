using UnityEngine;

namespace Shardwake.Monsters
{
    public readonly struct MonsterBehaviorTuning
    {
        public MonsterBehaviorTuning(float preferredDistance, float retreatDistance, float strafeWeight, float aggression, float specialCooldown)
        {
            PreferredDistance = Mathf.Max(0f, preferredDistance);
            RetreatDistance = Mathf.Max(0f, retreatDistance);
            StrafeWeight = Mathf.Clamp01(strafeWeight);
            Aggression = Mathf.Clamp01(aggression);
            SpecialCooldown = Mathf.Max(0.1f, specialCooldown);
        }

        public float PreferredDistance { get; }
        public float RetreatDistance { get; }
        public float StrafeWeight { get; }
        public float Aggression { get; }
        public float SpecialCooldown { get; }
    }

    public static class MonsterBehaviorTunings
    {
        public static MonsterBehaviorTuning Get(MonsterBehaviorType behavior, MonsterAttackDefinition attack)
        {
            return behavior switch
            {
                MonsterBehaviorType.Swarm => new MonsterBehaviorTuning(attack.Range * 0.75f, 0f, 0.15f, 0.9f, 6f),
                MonsterBehaviorType.Charger => new MonsterBehaviorTuning(attack.Range * 0.9f, 0f, 0.05f, 0.85f, 5f),
                MonsterBehaviorType.RangedKiter => new MonsterBehaviorTuning(attack.Range * 0.82f, attack.Range * 0.45f, 0.55f, 0.65f, 7f),
                MonsterBehaviorType.Caster => new MonsterBehaviorTuning(attack.Range * 0.72f, attack.Range * 0.35f, 0.35f, 0.55f, 8f),
                MonsterBehaviorType.SupportHealer => new MonsterBehaviorTuning(attack.Range * 0.7f, attack.Range * 0.35f, 0.45f, 0.4f, 6f),
                MonsterBehaviorType.Tank => new MonsterBehaviorTuning(attack.Range * 0.85f, 0f, 0.05f, 0.7f, 8f),
                MonsterBehaviorType.Ambusher => new MonsterBehaviorTuning(attack.Range * 0.8f, 0f, 0.25f, 0.95f, 5f),
                _ => new MonsterBehaviorTuning(attack.Range * 0.85f, 0f, 0.1f, 0.75f, 7f)
            };
        }
    }
}
