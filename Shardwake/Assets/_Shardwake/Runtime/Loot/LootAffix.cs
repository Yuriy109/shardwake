namespace Shardwake.Loot
{
    public readonly struct LootAffix
    {
        public LootAffix(LootStat stat, int value)
        {
            Stat = stat;
            Value = value;
        }

        public LootStat Stat { get; }
        public int Value { get; }

        public override string ToString()
        {
            return $"+{Value} {Stat}";
        }
    }
}
