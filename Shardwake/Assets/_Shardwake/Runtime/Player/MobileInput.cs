using UnityEngine;

namespace Shardwake.Player
{
    public static class MobileInput
    {
        private static bool attackQueued;
        private static bool dashQueued;
        private static bool weaponSwapQueued;
        private static readonly bool[] skillQueued = new bool[3];
        private static readonly bool[] consumableQueued = new bool[2];

        public static Vector2 Movement { get; private set; }

        public static void SetMovement(Vector2 movement)
        {
            Movement = Vector2.ClampMagnitude(movement, 1f);
        }

        public static void QueueAttack()
        {
            attackQueued = true;
        }

        public static bool ConsumeAttack()
        {
            if (!attackQueued)
            {
                return false;
            }

            attackQueued = false;
            return true;
        }

        public static void QueueDash()
        {
            dashQueued = true;
        }

        public static bool ConsumeDash()
        {
            if (!dashQueued)
            {
                return false;
            }

            dashQueued = false;
            return true;
        }

        public static void QueueWeaponSwap()
        {
            weaponSwapQueued = true;
        }

        public static bool ConsumeWeaponSwap()
        {
            if (!weaponSwapQueued)
            {
                return false;
            }

            weaponSwapQueued = false;
            return true;
        }

        public static void QueueSkill(int index)
        {
            if (index >= 0 && index < skillQueued.Length)
            {
                skillQueued[index] = true;
            }
        }

        public static bool ConsumeSkill(int index)
        {
            if (index < 0 || index >= skillQueued.Length || !skillQueued[index])
            {
                return false;
            }

            skillQueued[index] = false;
            return true;
        }

        public static void QueueConsumable(int index)
        {
            if (index >= 0 && index < consumableQueued.Length)
            {
                consumableQueued[index] = true;
            }
        }

        public static bool ConsumeConsumable(int index)
        {
            if (index < 0 || index >= consumableQueued.Length || !consumableQueued[index])
            {
                return false;
            }

            consumableQueued[index] = false;
            return true;
        }
    }
}
