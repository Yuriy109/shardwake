using Shardwake.Rendering;
using UnityEngine;

namespace Shardwake.Combat
{
    public sealed class AttackArc : MonoBehaviour
    {
        [SerializeField] private float lifetime = 0.16f;
        [SerializeField] private float startScale = 0.45f;
        [SerializeField] private float endScale = 1.25f;

        private Renderer arcRenderer;
        private Vector3 baseScale;
        private float age;

        private void Awake()
        {
            arcRenderer = GetComponent<Renderer>();
            baseScale = transform.localScale;
        }

        private void Update()
        {
            age += Time.deltaTime;
            var progress = Mathf.Clamp01(age / lifetime);
            transform.localScale = Vector3.Lerp(baseScale * startScale, baseScale * endScale, progress);

            if (arcRenderer != null)
            {
                var color = GreyboxMaterial.GetColor(arcRenderer, Color.white);
                color.a = 1f - progress;
                GreyboxMaterial.Apply(arcRenderer, color);
            }

            if (age >= lifetime)
            {
                Destroy(gameObject);
            }
        }

        public static void Spawn(Vector3 position, Quaternion rotation)
        {
            var arc = GameObject.CreatePrimitive(PrimitiveType.Cube);
            arc.name = "Attack Arc";
            arc.transform.position = position + Vector3.up * 0.08f;
            arc.transform.rotation = rotation;
            arc.transform.localScale = new Vector3(1.25f, 0.04f, 0.28f);

            var renderer = arc.GetComponent<Renderer>();
            GreyboxMaterial.Apply(renderer, new Color(1f, 0.88f, 0.36f, 0.75f));

            var collider = arc.GetComponent<Collider>();
            if (collider != null)
            {
                Destroy(collider);
            }

            arc.AddComponent<AttackArc>();
        }
    }
}
