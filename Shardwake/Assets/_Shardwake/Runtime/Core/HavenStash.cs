using System.Collections.Generic;
using Shardwake.Loot;

namespace Shardwake.Core
{
    /// <summary>
    /// Temporary in-memory stash for the greybox MVP.
    /// Later this becomes saved account inventory on the server/backend.
    /// </summary>
    public static class HavenStash
    {
        private static readonly List<LootItem> items = new();

        public static IReadOnlyList<LootItem> Items => items;
        public static int Count => items.Count;

        public static void Add(LootItem item)
        {
            items.Add(item);
        }

        public static void AddRange(IEnumerable<LootItem> newItems)
        {
            if (newItems == null)
            {
                return;
            }

            foreach (var item in newItems)
            {
                items.Add(item);
            }
        }

        public static bool TryGet(int index, out LootItem item)
        {
            item = default;
            if (index < 0 || index >= items.Count)
            {
                return false;
            }

            item = items[index];
            return true;
        }

        public static bool TryTakeAt(int index, out LootItem item)
        {
            item = default;
            if (index < 0 || index >= items.Count)
            {
                return false;
            }

            item = items[index];
            items.RemoveAt(index);
            return true;
        }

        public static void Clear()
        {
            items.Clear();
        }
    }
}
