using Shardwake.Core;
using Shardwake.StatusEffects;
using Shardwake.UI;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Shardwake.Player
{
    [RequireComponent(typeof(Rigidbody))]
    public sealed class PlayerDash : MonoBehaviour
    {
        [SerializeField] private float dashDistance = 3.2f;
        [SerializeField] private float dashCooldown = 1.2f;

        private float baseDashDistance;
        private float baseDashCooldown;

        private Rigidbody body;
        private float nextDashTime;
        private float lastDashCooldown;

        private void Awake()
        {
            body = GetComponent<Rigidbody>();
            baseDashDistance = dashDistance;
            baseDashCooldown = dashCooldown;
        }

        private void Update()
        {
            if (ShardwakeSession.Instance != null && (!ShardwakeSession.Instance.HasStarted || ShardwakeSession.Instance.IsFinished))
            {
                return;
            }

            if (!ShouldDash() || Time.time < nextDashTime)
            {
                return;
            }

            var statusEffects = GetComponent<StatusEffectController>();
            if (statusEffects != null && (statusEffects.IsRooted || statusEffects.IsStunned))
            {
                return;
            }

            lastDashCooldown = dashCooldown;
            nextDashTime = Time.time + dashCooldown;
            Dash();
        }


        public float GetCooldownRemaining()
        {
            return Mathf.Max(0f, nextDashTime - Time.time);
        }

        public float GetCooldownRatio()
        {
            var duration = Mathf.Max(0.01f, lastDashCooldown);
            return Mathf.Clamp01(GetCooldownRemaining() / duration);
        }

        private static bool ShouldDash()
        {
            if (MobileInput.ConsumeDash())
            {
                return true;
            }

            var keyboard = Keyboard.current;
            return keyboard != null && keyboard.leftShiftKey.wasPressedThisFrame;
        }

        public void Configure(float distanceMultiplier, float cooldownMultiplier)
        {
            dashDistance = Mathf.Max(0.2f, baseDashDistance * distanceMultiplier);
            dashCooldown = Mathf.Max(0.05f, baseDashCooldown * cooldownMultiplier);
        }

        private void Dash()
        {
            var direction = new Vector3(MobileInput.Movement.x, 0f, MobileInput.Movement.y);
            if (direction.sqrMagnitude < 0.01f)
            {
                direction = transform.forward;
            }

            body.MovePosition(body.position + direction.normalized * dashDistance);
            FloatingText.Spawn(transform.position + Vector3.up * 1.8f, "DASH", new Color(0.55f, 0.85f, 1f), 28);
        }
    }
}
