using Shardwake.Core;
using Shardwake.StatusEffects;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

namespace Shardwake.Player
{
    [RequireComponent(typeof(Rigidbody))]
    public sealed class PlayerController : MonoBehaviour
    {
        [SerializeField] private float moveSpeed = 7f;

        private Rigidbody body;
        private Vector2 moveInput;

        private void Awake()
        {
            body = GetComponent<Rigidbody>();
        }

        private void Update()
        {
            if (ShardwakeSession.Instance != null && (!ShardwakeSession.Instance.HasStarted || ShardwakeSession.Instance.IsFinished))
            {
                moveInput = Vector2.zero;
                return;
            }

            moveInput = ReadMoveInput();
        }

        private void FixedUpdate()
        {
            var movement = new Vector3(moveInput.x, 0f, moveInput.y);
            var statusEffects = GetComponent<StatusEffectController>();
            var speedMultiplier = statusEffects != null ? statusEffects.MoveSpeedMultiplier : 1f;
            body.MovePosition(body.position + movement * (moveSpeed * speedMultiplier * Time.fixedDeltaTime));

            if (movement.sqrMagnitude > 0.01f)
            {
                transform.rotation = Quaternion.LookRotation(movement.normalized, Vector3.up);
            }
        }

        public void SetMoveSpeed(float value)
        {
            moveSpeed = Mathf.Max(0.1f, value);
        }

        private Vector2 ReadMoveInput()
        {
            if (MobileInput.Movement.sqrMagnitude > 0.001f)
            {
                return MobileInput.Movement;
            }

            var keyboard = Keyboard.current;
            if (keyboard != null)
            {
                var keyboardInput = Vector2.zero;
                keyboardInput.x = ReadAxis(keyboard.aKey, keyboard.dKey, keyboard.leftArrowKey, keyboard.rightArrowKey);
                keyboardInput.y = ReadAxis(keyboard.sKey, keyboard.wKey, keyboard.downArrowKey, keyboard.upArrowKey);

                if (keyboardInput.sqrMagnitude > 0f)
                {
                    return Vector2.ClampMagnitude(keyboardInput, 1f);
                }
            }

            return Vector2.zero;
        }

        private static float ReadAxis(params ButtonControl[] buttons)
        {
            var negative = buttons[0].isPressed || buttons[2].isPressed;
            var positive = buttons[1].isPressed || buttons[3].isPressed;
            return (positive ? 1f : 0f) - (negative ? 1f : 0f);
        }
    }
}
