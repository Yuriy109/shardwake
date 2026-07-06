namespace Shardwake.Loot
{
    public static class RarityRules
    {
        public static RarityRule Get(ItemRarity rarity)
        {
            return rarity switch
            {
                ItemRarity.Uncommon => new RarityRule(2, 0, false, false, false),
                ItemRarity.Rare => new RarityRule(3, 2, false, false, false),
                ItemRarity.Epic => new RarityRule(4, 2, true, false, false),
                ItemRarity.Legendary => new RarityRule(5, 3, true, true, true),
                _ => new RarityRule(0, 0, false, false, false)
            };
        }
    }
}
