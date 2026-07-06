namespace Shardwake.Loot
{
    public static class LootTables
    {
        public static LootTable Get(LootSource source)
        {
            return source switch
            {
                LootSource.EliteEnemy => new LootTable(35, 45, 18, 2, 0),
                LootSource.MiniBoss => new LootTable(0, 35, 50, 14, 1),
                LootSource.SmallChest => new LootTable(50, 40, 10, 0, 0),
                LootSource.LockedChest => new LootTable(15, 45, 35, 5, 0),
                LootSource.AncientChest => new LootTable(0, 20, 55, 23, 2),
                LootSource.HellgateEnemy => new LootTable(10, 35, 40, 14, 1),
                LootSource.HellgateBoss => new LootTable(0, 5, 45, 42, 8),
                _ => new LootTable(70, 25, 5, 0, 0)
            };
        }
    }
}
