using Shardwake.Combat;
using Shardwake.Core;
using Shardwake.StatusEffects;
using Shardwake.Rendering;
using Shardwake.Player;
using UnityEngine;

namespace Shardwake.Monsters
{
    [RequireComponent(typeof(Rigidbody))]
    public sealed class EnemyController : MonoBehaviour
    {
        [SerializeField] private MonsterType monsterType = MonsterType.Shardling;
        [SerializeField] private Transform target;

        private Rigidbody body;
        private MonsterDefinition definition;
        private MonsterBehaviorTuning behaviorTuning;
        private StatusEffectController statusEffects;

        private float nextAttackTime;
        private float nextSpecialTime;
        private float nextStrafeFlipTime;
        private float strafeDirection = 1f;
        private bool isWindingUp;
        private float attackResolveTime;
        private Vector3 homePosition;
        private float nextTargetSearchTime;
        private float aggroMemoryUntil;

        public MonsterDefinition Definition => definition;
        public MonsterType MonsterType => monsterType;
        public float AttackRange => definition.Attack.Range;
        public float AttackSpeed => definition.Attack.AttackSpeed;

        private void Awake()
        {
            body = GetComponent<Rigidbody>();
            statusEffects = GetComponent<StatusEffectController>();
            homePosition = transform.position;
            ApplyDefinition(monsterType, true);
        }

        private void FixedUpdate()
        {
            if (ShardwakeSession.Instance != null && (!ShardwakeSession.Instance.HasStarted || ShardwakeSession.Instance.IsFinished))
            {
                return;
            }

            EnsureTarget();

            if (target == null)
            {
                return;
            }

            if (statusEffects != null && statusEffects.IsStunned)
            {
                return;
            }

            if (isWindingUp)
            {
                ResolveAttackIfReady();
                return;
            }

            var toTarget = target.position - transform.position;
            toTarget.y = 0f;
            var distance = toTarget.magnitude;

            if (!HasAggro(distance))
            {
                ReturnHome();
                return;
            }

            RunSpecialBehavior(distance);
            MoveByBehavior(toTarget, distance);

            if (CanAttack(distance) && Time.time >= nextAttackTime)
            {
                BeginAttackWindup();
            }
        }

        public void SetTarget(Transform newTarget)
        {
            target = newTarget;
        }

        private void EnsureTarget()
        {
            if (target != null || Time.time < nextTargetSearchTime)
            {
                return;
            }

            nextTargetSearchTime = Time.time + 0.5f;

            var player = Object.FindFirstObjectByType<PlayerController>();
            if (player != null)
            {
                target = player.transform;
            }
        }

        private bool HasAggro(float distance)
        {
            // Greybox MVP: make enemies responsive from farther away so the test scene
            // immediately proves that AI movement works. Later this can become
            // line-of-sight + sound + proximity based aggro.
            var greyboxAggroRange = Mathf.Max(definition.AggroRange, 28f);

            if (distance <= greyboxAggroRange)
            {
                aggroMemoryUntil = Time.time + 4f;
                return true;
            }

            return Time.time <= aggroMemoryUntil;
        }

        private void ReturnHome()
        {
            if (statusEffects != null && (statusEffects.IsRooted || statusEffects.IsStunned))
            {
                return;
            }

            var toHome = homePosition - transform.position;
            toHome.y = 0f;

            if (toHome.sqrMagnitude <= 0.15f)
            {
                return;
            }

            var speedMultiplier = statusEffects != null ? statusEffects.MoveSpeedMultiplier : 1f;
            var direction = toHome.normalized;
            body.MovePosition(body.position + direction * (definition.MoveSpeed * 0.65f * speedMultiplier * Time.fixedDeltaTime));
            FaceTarget(direction);
        }

        public void ApplyDefinition(MonsterType newType, bool resetHealth)
        {
            monsterType = newType;
            definition = MonsterDefinitions.Get(monsterType);
            behaviorTuning = MonsterBehaviorTunings.Get(definition.Behavior, definition.Attack);
            name = definition.DisplayName;

            if (TryGetComponent(out Renderer enemyRenderer))
            {
                GreyboxMaterial.Apply(enemyRenderer, definition.Color);
            }

            if (resetHealth && TryGetComponent(out Health health))
            {
                health.SetMaxHealth(definition.MaxHealth);
            }
        }

        private void MoveByBehavior(Vector3 toTarget, float distance)
        {
            if (statusEffects != null && statusEffects.IsRooted)
            {
                return;
            }

            if (distance <= 0.01f)
            {
                return;
            }

            var directionToTarget = toTarget.normalized;
            var desiredDirection = ResolveMovementDirection(directionToTarget, distance);

            if (desiredDirection.sqrMagnitude <= 0.01f)
            {
                FaceTarget(directionToTarget);
                return;
            }

            var speedMultiplier = statusEffects != null ? statusEffects.MoveSpeedMultiplier : 1f;
            var behaviorSpeed = ResolveBehaviorSpeedMultiplier(distance);
            body.MovePosition(body.position + desiredDirection.normalized * (definition.MoveSpeed * behaviorSpeed * speedMultiplier * Time.fixedDeltaTime));
            FaceTarget(directionToTarget);
        }

        private Vector3 ResolveMovementDirection(Vector3 directionToTarget, float distance)
        {
            if (definition.Behavior == MonsterBehaviorType.RangedKiter || definition.Behavior == MonsterBehaviorType.Caster || definition.Behavior == MonsterBehaviorType.SupportHealer)
            {
                if (distance < behaviorTuning.RetreatDistance)
                {
                    return -directionToTarget;
                }

                if (distance > behaviorTuning.PreferredDistance)
                {
                    return directionToTarget;
                }

                return StrafeDirection(directionToTarget) * behaviorTuning.StrafeWeight;
            }

            if (definition.Behavior == MonsterBehaviorType.Tank)
            {
                return distance > definition.Attack.Range * 0.85f ? directionToTarget : Vector3.zero;
            }

            if (definition.Behavior == MonsterBehaviorType.Ambusher)
            {
                return distance > definition.Attack.Range * 0.65f ? directionToTarget : StrafeDirection(directionToTarget) * 0.35f;
            }

            return distance > definition.Attack.Range * 0.8f ? directionToTarget : Vector3.zero;
        }

        private Vector3 StrafeDirection(Vector3 directionToTarget)
        {
            if (Time.time >= nextStrafeFlipTime)
            {
                strafeDirection = Random.value < 0.5f ? -1f : 1f;
                nextStrafeFlipTime = Time.time + Random.Range(1.2f, 2.4f);
            }

            return Vector3.Cross(Vector3.up, directionToTarget).normalized * strafeDirection;
        }

        private float ResolveBehaviorSpeedMultiplier(float distance)
        {
            return definition.Behavior switch
            {
                MonsterBehaviorType.Swarm => 1.12f,
                MonsterBehaviorType.Charger => distance > definition.Attack.Range ? 1.18f : 1f,
                MonsterBehaviorType.Ambusher => 1.2f,
                MonsterBehaviorType.Tank => 0.86f,
                MonsterBehaviorType.SupportHealer => 0.92f,
                _ => 1f
            };
        }

        private bool CanAttack(float distance)
        {
            if (definition.Attack.Kind == MonsterAttackKind.GroundZone)
            {
                return distance <= definition.Attack.Range;
            }

            return distance <= definition.Attack.Range + 0.15f;
        }

        private void RunSpecialBehavior(float distance)
        {
            if (Time.time < nextSpecialTime)
            {
                return;
            }

            if (definition.Behavior == MonsterBehaviorType.SupportHealer && TryHealNearbyAlly())
            {
                nextSpecialTime = Time.time + behaviorTuning.SpecialCooldown;
                return;
            }

            if (definition.Behavior == MonsterBehaviorType.Tank && TryApplyTankShield(distance))
            {
                nextSpecialTime = Time.time + behaviorTuning.SpecialCooldown;
                return;
            }

            if (definition.Behavior == MonsterBehaviorType.Ambusher && distance <= definition.Attack.Range * 1.25f)
            {
                nextAttackTime = Mathf.Min(nextAttackTime, Time.time + 0.15f);
                nextSpecialTime = Time.time + behaviorTuning.SpecialCooldown;
            }
        }

        private bool TryHealNearbyAlly()
        {
            var hits = Physics.OverlapSphere(transform.position, 5f);
            Health bestTarget = null;
            var lowestNormalized = 1f;

            foreach (var hit in hits)
            {
                if (hit.gameObject == gameObject)
                {
                    continue;
                }

                if (!hit.TryGetComponent(out EnemyController ally))
                {
                    continue;
                }

                if (ally.Definition.Faction != definition.Faction)
                {
                    continue;
                }

                if (!hit.TryGetComponent(out Health allyHealth) || allyHealth.IsDead || allyHealth.Normalized >= 0.75f)
                {
                    continue;
                }

                if (allyHealth.Normalized < lowestNormalized)
                {
                    bestTarget = allyHealth;
                    lowestNormalized = allyHealth.Normalized;
                }
            }

            if (bestTarget == null)
            {
                return false;
            }

            bestTarget.Heal(Mathf.Max(6f, definition.Attack.Damage * 1.4f));
            return true;
        }

        private bool TryApplyTankShield(float distance)
        {
            if (distance > definition.Attack.Range * 1.3f)
            {
                return false;
            }

            if (!TryGetComponent(out StatusEffectController effects))
            {
                effects = gameObject.AddComponent<StatusEffectController>();
                statusEffects = effects;
            }

            effects.Apply(new StatusEffectDefinition(StatusEffectType.Shield, 3.5f, Mathf.Max(8f, definition.MaxHealth * 0.12f)));
            return true;
        }

        private void FaceTarget(Vector3 direction)
        {
            direction.y = 0f;
            if (direction.sqrMagnitude > 0.01f)
            {
                transform.rotation = Quaternion.LookRotation(direction.normalized, Vector3.up);
            }
        }

        private void BeginAttackWindup()
        {
            if (target == null)
            {
                return;
            }

            var direction = target.position - transform.position;
            direction.y = 0f;
            FaceTarget(direction);

            isWindingUp = true;
            attackResolveTime = Time.time + definition.Attack.WindupSeconds;

            EnemyAttackTelegraph.Spawn(
                GetTelegraphPosition(),
                transform.rotation,
                definition.Attack.Range,
                definition.Attack.WindupSeconds,
                definition.Attack.TelegraphWidth,
                definition.Attack.Kind);
        }

        private Vector3 GetTelegraphPosition()
        {
            if (definition.Attack.Kind == MonsterAttackKind.GroundZone && target != null)
            {
                return target.position;
            }

            return transform.position + transform.forward * (definition.Attack.Range * 0.5f);
        }

        private void ResolveAttackIfReady()
        {
            if (Time.time < attackResolveTime)
            {
                return;
            }

            isWindingUp = false;
            nextAttackTime = Time.time + definition.Attack.CooldownSeconds;

            if (target == null || !IsTargetInAttackRange())
            {
                return;
            }

            if (target.TryGetComponent(out Health health))
            {
                var threatMultiplier = ShardwakeSession.Instance != null ? ShardwakeSession.Instance.ShardAlertThreatMultiplier : 1f;
                var damageType = GetDamageType(definition.Attack.Kind);
                var statusEffect = GetAttackStatusEffect(definition);
                health.TakeDamage(new DamageRequest(definition.Attack.Damage * threatMultiplier, damageType, gameObject, statusEffect));
            }
        }

        private static DamageType GetDamageType(MonsterAttackKind attackKind)
        {
            return attackKind switch
            {
                MonsterAttackKind.RangedShot => DamageType.Physical,
                MonsterAttackKind.GroundZone => DamageType.Magic,
                _ => DamageType.Physical
            };
        }

        private static StatusEffectDefinition GetAttackStatusEffect(MonsterDefinition monster)
        {
            if (monster.Type == MonsterType.VenomSpider || monster.Type == MonsterType.PoisonFrog || monster.Type == MonsterType.Slime)
            {
                return new StatusEffectDefinition(StatusEffectType.Poison, 3f, 1.2f, 1f);
            }

            if (monster.Type == MonsterType.FireImp || monster.Type == MonsterType.AshHound)
            {
                return new StatusEffectDefinition(StatusEffectType.Burn, 2.5f, 1.4f, 1f);
            }

            if (monster.Type == MonsterType.FrostWolf || monster.Type == MonsterType.FrostShaman || monster.Type == MonsterType.IceGoblin)
            {
                return new StatusEffectDefinition(StatusEffectType.Slow, 2.2f, 0.3f);
            }

            return monster.Attack.Kind switch
            {
                MonsterAttackKind.Leap => new StatusEffectDefinition(StatusEffectType.Slow, 1.5f, 0.25f),
                MonsterAttackKind.GroundZone => new StatusEffectDefinition(StatusEffectType.Slow, 2.5f, 0.35f),
                _ => default
            };
        }

        private bool IsTargetInAttackRange()
        {
            var toTarget = target.position - transform.position;
            toTarget.y = 0f;
            var distance = toTarget.magnitude;

            if (distance > definition.Attack.Range + 0.25f)
            {
                return false;
            }

            if (definition.Attack.Kind == MonsterAttackKind.RangedShot || definition.Attack.Kind == MonsterAttackKind.GroundZone)
            {
                return true;
            }

            if (distance <= 0.01f)
            {
                return true;
            }

            var angle = Vector3.Angle(transform.forward, toTarget.normalized);
            return angle <= 65f;
        }
    }
}
