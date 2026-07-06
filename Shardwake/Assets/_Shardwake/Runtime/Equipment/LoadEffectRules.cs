namespace Shardwake.Equipment
{
    public static class LoadEffectRules
    {
        public static LoadEffects Get(LoadCategory category)
        {
            return category switch
            {
                LoadCategory.Light => new LoadEffects(1.18f, 0.9f, 1.15f, 1.04f, 0f),
                LoadCategory.Medium => new LoadEffects(1f, 1f, 1f, 1f, 0.1f),
                LoadCategory.Heavy => new LoadEffects(0.78f, 1.18f, 0.85f, 0.94f, 0.28f),
                LoadCategory.Overloaded => new LoadEffects(0.42f, 1.55f, 0.65f, 0.82f, 0.45f),
                _ => new LoadEffects(1f, 1f, 1f, 1f, 0f)
            };
        }
    }
}
