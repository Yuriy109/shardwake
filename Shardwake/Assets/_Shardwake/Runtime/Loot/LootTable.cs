using UnityEngine;

namespace Shardwake.Loot
{
    public readonly struct LootTable
    {
        public LootTable(int common, int uncommon, int rare, int epic, int legendary)
        {
            Common = common;
            Uncommon = uncommon;
            Rare = rare;
            Epic = epic;
            Legendary = legendary;
        }

        public int Common { get; }
        public int Uncommon { get; }
        public int Rare { get; }
        public int Epic { get; }
        public int Legendary { get; }

        public ItemRarity Roll()
        {
            var total = Mathf.Max(1, Common + Uncommon + Rare + Epic + Legendary);
            var roll = Random.Range(0, total);

            if (roll < Common)
            {
                return ItemRarity.Common;
            }

            roll -= Common;
            if (roll < Uncommon)
            {
                return ItemRarity.Uncommon;
            }

            roll -= Uncommon;
            if (roll < Rare)
            {
                return ItemRarity.Rare;
            }

            roll -= Rare;
            if (roll < Epic)
            {
                return ItemRarity.Epic;
            }

            return ItemRarity.Legendary;
        }
    }
}
