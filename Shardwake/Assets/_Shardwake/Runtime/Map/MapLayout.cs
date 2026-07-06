using System.Collections.Generic;

namespace Shardwake.Map
{
    public sealed class MapLayout
    {
        public MapLayout(int seed, IReadOnlyList<PlacedMapModule> modules)
        {
            Seed = seed;
            Modules = modules;
        }

        public int Seed { get; }
        public IReadOnlyList<PlacedMapModule> Modules { get; }
    }
}
