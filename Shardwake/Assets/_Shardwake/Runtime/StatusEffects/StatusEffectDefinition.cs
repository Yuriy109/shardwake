using UnityEngine;

namespace Shardwake.StatusEffects
{
    public readonly struct StatusEffectDefinition
    {
        public StatusEffectDefinition(StatusEffectType type, float duration, float magnitude, float tickInterval = 1f)
        {
            Type = type;
            Duration = Mathf.Max(0f, duration);
            Magnitude = Mathf.Max(0f, magnitude);
            TickInterval = Mathf.Max(0.05f, tickInterval);
        }

        public StatusEffectType Type { get; }
        public float Duration { get; }
        public float Magnitude { get; }
        public float TickInterval { get; }
        public bool IsValid => Type != StatusEffectType.None && Duration > 0f;
    }
}
