using Shardwake.Rendering;
using UnityEngine;

namespace Shardwake.Monsters
{
    public sealed class EnemyAttackTelegraph : MonoBehaviour
    {
        [SerializeField] private float lifetime = 0.5f;

        private Renderer telegraphRenderer;
        private float age;

        private void Awake()
        {
            telegraphRenderer = GetComponent<Renderer>();
        }

        private void Update()
        {
            age += Time.deltaTime;
            var progress = Mathf.Clamp01(age / lifetime);

            if (telegraphRenderer != null)
            {
                var color = GreyboxMaterial.GetColor(telegraphRenderer, Color.white);
                color.a = Mathf.Lerp(0.65f, 0.08f, progress);
                GreyboxMaterial.Apply(telegraphRenderer, color);
            }

            if (age >= lifetime)
            {
                Destroy(gameObject);
            }
        }

        public static void Spawn(Vector3 position, Quaternion rotation, float range, float windupSeconds)
        {
            Spawn(position, rotation, range, windupSeconds, 1.25f, MonsterAttackKind.MeleeCone);
        }

        public static void Spawn(Vector3 position, Quaternion rotation, float range, float windupSeconds, float width, MonsterAttackKind kind)
        {
            var telegraph = GameObject.CreatePrimitive(PrimitiveType.Cube);
            telegraph.name = $"Enemy Telegraph - {kind}";
            telegraph.transform.position = position + Vector3.up * 0.035f;
            telegraph.transform.rotation = rotation;

            if (kind == MonsterAttackKind.GroundZone)
            {
                telegraph.transform.localScale = new Vector3(width, 0.025f, width);
            }
            else
            {
                telegraph.transform.localScale = new Vector3(width, 0.025f, range);
            }

            var renderer = telegraph.GetComponent<Renderer>();
            GreyboxMaterial.Apply(renderer, kind == MonsterAttackKind.RangedShot
                ? new Color(1f, 0.55f, 0.1f, 0.65f)
                : new Color(1f, 0.1f, 0.08f, 0.65f));

            var collider = telegraph.GetComponent<Collider>();
            if (collider != null)
            {
                Destroy(collider);
            }

            var marker = telegraph.AddComponent<EnemyAttackTelegraph>();
            marker.lifetime = windupSeconds;
        }
    }
}
