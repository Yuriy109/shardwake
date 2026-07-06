using UnityEngine;

namespace Shardwake.StatusEffects
{
    public sealed class StatusEffectInstance
    {
        private float nextTickTime;

        public StatusEffectInstance(StatusEffectDefinition definition)
        {
            Definition = definition;
            RemainingSeconds = definition.Duration;
            nextTickTime = Time.time + definition.TickInterval;
        }

        public StatusEffectDefinition Definition { get; }
        public float RemainingSeconds { get; private set; }
        public bool IsExpired => RemainingSeconds <= 0f;
        public bool ShouldTick => Time.time >= nextTickTime;

        public void TickScheduled()
        {
            nextTickTime = Time.time + Definition.TickInterval;
        }

        public void Advance(float deltaTime)
        {
            RemainingSeconds -= Mathf.Max(0f, deltaTime);
        }
    }
}
