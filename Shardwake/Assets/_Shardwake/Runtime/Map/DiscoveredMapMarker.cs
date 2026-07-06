using UnityEngine;

namespace Shardwake.Map
{
    public readonly struct DiscoveredMapMarker
    {
        public DiscoveredMapMarker(int id, MapMarkerKind kind, Vector3 position)
        {
            Id = id;
            Kind = kind;
            Position = position;
        }

        public int Id { get; }
        public MapMarkerKind Kind { get; }
        public Vector3 Position { get; }
    }
}
