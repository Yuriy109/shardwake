using Shardwake.Core;
using Shardwake.Skills;
using Shardwake.UI;
using Shardwake.Weapons;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Shardwake.Player
{
    public sealed class PlayerSkills : MonoBehaviour
    {
        [SerializeField] private float consumableCooldown = 12f;

        private readonly float[] nextSkillTimes = new float[3];
        private readonly float[] skillCooldownDurations = new float[3];
        private readonly float[] nextConsumableTimes = new float[2];
        private readonly float[] consumableCooldownDurations = new float[2];

        private void Update()
        {
            if (ShardwakeSession.Instance != null && (!ShardwakeSession.Instance.HasStarted || ShardwakeSession.Instance.IsFinished))
            {
                return;
            }

            TryKeyboardInput();

            for (var i = 0; i < 3; i++)
            {
                TrySkill(i);
            }

            TryConsumable(0, "POTION");
            TryConsumable(1, "BOMB");
        }

        private void TryKeyboardInput()
        {
            var keyboard = Keyboard.current;
            if (keyboard == null)
            {
                return;
            }

            if (keyboard.digit1Key.wasPressedThisFrame)
            {
                MobileInput.QueueSkill(0);
            }

            if (keyboard.digit2Key.wasPressedThisFrame)
            {
                MobileInput.QueueSkill(1);
            }

            if (keyboard.digit3Key.wasPressedThisFrame)
            {
                MobileInput.QueueSkill(2);
            }

            if (keyboard.qKey.wasPressedThisFrame)
            {
                MobileInput.QueueWeaponSwap();
            }
        }

        private void TrySkill(int index)
        {
            if (!MobileInput.ConsumeSkill(index))
            {
                return;
            }

            var session = ShardwakeSession.Instance;
            var skill = session != null
                ? session.WeaponLoadout.ActiveSkillLoadout.GetActiveSkill(index)
                : WeaponDefinitions.Get(WeaponType.GreatWeapon).ActiveSkills[index];

            if (string.IsNullOrWhiteSpace(skill.Id))
            {
                return;
            }

            if (Time.time < nextSkillTimes[index])
            {
                return;
            }

            var cooldownMultiplier = session != null ? session.SkillCooldownMultiplier : 1f;
            var finalCooldown = Mathf.Max(0.05f, skill.Cooldown * cooldownMultiplier);
            skillCooldownDurations[index] = finalCooldown;
            nextSkillTimes[index] = Time.time + finalCooldown;

            FloatingText.Spawn(transform.position + Vector3.up * 2.2f, skill.DisplayName.ToUpperInvariant(), new Color(0.72f, 0.55f, 1f), 26);

            var damageMultiplier = session != null ? session.GetSkillDamageMultiplier(skill) : 1f;
            SkillExecutor.Execute(new SkillExecutionContext(gameObject, transform, skill, damageMultiplier));
        }


        public float GetSkillCooldownRemaining(int index)
        {
            if (index < 0 || index >= nextSkillTimes.Length)
            {
                return 0f;
            }

            return Mathf.Max(0f, nextSkillTimes[index] - Time.time);
        }

        public float GetSkillCooldownRatio(int index)
        {
            if (index < 0 || index >= nextSkillTimes.Length)
            {
                return 0f;
            }

            var duration = Mathf.Max(0.01f, skillCooldownDurations[index]);
            return Mathf.Clamp01(GetSkillCooldownRemaining(index) / duration);
        }

        public string GetSkillDisplayName(int index)
        {
            var session = ShardwakeSession.Instance;
            var skill = session != null
                ? session.WeaponLoadout.ActiveSkillLoadout.GetActiveSkill(index)
                : WeaponDefinitions.Get(WeaponType.GreatWeapon).ActiveSkills[Mathf.Clamp(index, 0, 2)];

            return string.IsNullOrWhiteSpace(skill.DisplayName) ? $"Skill {index + 1}" : skill.DisplayName;
        }

        public float GetConsumableCooldownRatio(int index)
        {
            if (index < 0 || index >= nextConsumableTimes.Length)
            {
                return 0f;
            }

            var duration = Mathf.Max(0.01f, consumableCooldownDurations[index]);
            return Mathf.Clamp01(Mathf.Max(0f, nextConsumableTimes[index] - Time.time) / duration);
        }

        private void TryConsumable(int index, string label)
        {
            if (!MobileInput.ConsumeConsumable(index) || Time.time < nextConsumableTimes[index])
            {
                return;
            }

            consumableCooldownDurations[index] = consumableCooldown;
            nextConsumableTimes[index] = Time.time + consumableCooldown;
            FloatingText.Spawn(transform.position + Vector3.up * 2f, label, new Color(0.42f, 1f, 0.65f), 28);
        }
    }
}
