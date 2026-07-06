using UnityEngine;

namespace Shardwake.Map
{
    public readonly struct PlacedMapModule
    {
        public PlacedMapModule(MapModuleDefinition definition, Vector2Int slot, Vector3 origin, int rotationSteps)
        {
            Definition = definition;
            Slot = slot;
            Origin = origin;
            RotationSteps = rotationSteps;
        }

        public MapModuleDefinition Definition { get; }
        public Vector2Int Slot { get; }
        public Vector3 Origin { get; }
        public int RotationSteps { get; }

        public Vector3 TransformPoint(Vector3 localPoint)
        {
            Vector3 rotated;
            switch (RotationSteps)
            {
                case 1:
                    rotated = new Vector3(localPoint.z, localPoint.y, -localPoint.x);
                    break;
                case 2:
                    rotated = new Vector3(-localPoint.x, localPoint.y, -localPoint.z);
                    break;
                case 3:
                    rotated = new Vector3(-localPoint.z, localPoint.y, localPoint.x);
                    break;
                default:
                    rotated = localPoint;
                    break;
            }

            return Origin + rotated;
        }
    }
}
