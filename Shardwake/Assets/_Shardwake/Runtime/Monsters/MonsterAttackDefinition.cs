using UnityEngine;

namespace Shardwake.Monsters
{
    public readonly struct MonsterAttackDefinition
    {
        public MonsterAttackDefinition(MonsterAttackKind kind, float damage, float range, float attackSpeed, float windupSeconds, float telegraphWidth = 1.25f)
        {
            Kind = kind;
            Damage = Mathf.Max(0f, damage);
            Range = Mathf.Max(0.1f, range);
            AttackSpeed = Mathf.Max(0.05f, attackSpeed);
            WindupSeconds = Mathf.Max(0.05f, windupSeconds);
            TelegraphWidth = Mathf.Max(0.25f, telegraphWidth);
        }

        public MonsterAttackKind Kind { get; }
        public float Damage { get; }
        public float Range { get; }
        public float AttackSpeed { get; }
        public float WindupSeconds { get; }
        public float TelegraphWidth { get; }
        public float CooldownSeconds => 1f / AttackSpeed;
        public bool IsRanged => Kind == MonsterAttackKind.RangedShot || Kind == MonsterAttackKind.GroundZone;
    }
}
