using Shardwake.Combat;
using Shardwake.Core;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Shardwake.Player
{
    public sealed class PlayerCombat : MonoBehaviour
    {
        [SerializeField] private float attackRadius = 1.8f;
        [SerializeField] private float attackDamage = 25f;
        [SerializeField] private float attackCooldown = 0.45f;
        [SerializeField] private bool autoAttack = true;

        private float nextAttackTime;

        private void Update()
        {
            if (ShardwakeSession.Instance != null && (!ShardwakeSession.Instance.HasStarted || ShardwakeSession.Instance.IsFinished))
            {
                return;
            }

            if ((!ShouldAttack() && !HasAutoAttackTarget()) || Time.time < nextAttackTime)
            {
                return;
            }

            nextAttackTime = Time.time + attackCooldown;
            Attack();
        }

        private static bool ShouldAttack()
        {
            if (MobileInput.ConsumeAttack())
            {
                return true;
            }

            var keyboard = Keyboard.current;
            if (keyboard != null && keyboard.spaceKey.wasPressedThisFrame)
            {
                return true;
            }

            var mouse = Mouse.current;
            if (mouse != null && mouse.leftButton.wasPressedThisFrame)
            {
                return true;
            }

            return false;
        }

        private bool HasAutoAttackTarget()
        {
            return autoAttack && FindAttackTarget() != null;
        }

        private void Attack()
        {
            var target = FindAttackTarget();
            if (target != null)
            {
                var direction = target.transform.position - transform.position;
                direction.y = 0f;
                if (direction.sqrMagnitude > 0.01f)
                {
                    transform.rotation = Quaternion.LookRotation(direction.normalized, Vector3.up);
                }
            }

            AttackArc.Spawn(transform.position + transform.forward * 1.1f, transform.rotation);

            var hits = Physics.OverlapSphere(transform.position, attackRadius);
            foreach (var hit in hits)
            {
                if (hit.attachedRigidbody != null && hit.attachedRigidbody.gameObject == gameObject)
                {
                    continue;
                }

                if (hit.TryGetComponent(out Health health))
                {
                    health.TakeDamage(new DamageRequest(attackDamage, DamageType.Physical, gameObject));
                }
            }
        }

        private Health FindAttackTarget()
        {
            Health bestTarget = null;
            var bestDistance = float.MaxValue;
            var hits = Physics.OverlapSphere(transform.position, attackRadius);

            foreach (var hit in hits)
            {
                if (hit.attachedRigidbody != null && hit.attachedRigidbody.gameObject == gameObject)
                {
                    continue;
                }

                if (!hit.TryGetComponent(out Health health) || health.IsDead)
                {
                    continue;
                }

                var distance = Vector3.SqrMagnitude(health.transform.position - transform.position);
                if (distance < bestDistance)
                {
                    bestDistance = distance;
                    bestTarget = health;
                }
            }

            return bestTarget;
        }

        public void Configure(float damage, float cooldown, float radius)
        {
            attackDamage = Mathf.Max(1f, damage);
            attackCooldown = Mathf.Max(0.05f, cooldown);
            attackRadius = Mathf.Max(0.25f, radius);
        }
    }
}
