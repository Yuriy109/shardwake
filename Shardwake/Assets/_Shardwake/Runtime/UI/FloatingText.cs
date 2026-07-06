using UnityEngine;
using UnityEngine.UI;

namespace Shardwake.UI
{
    public sealed class FloatingText : MonoBehaviour
    {
        [SerializeField] private float lifetime = 0.85f;
        [SerializeField] private Vector3 velocity = new(0f, 1.2f, 0f);

        private Text text;
        private float age;
        private Color baseColor;

        private void Awake()
        {
            text = GetComponentInChildren<Text>();
            baseColor = text != null ? text.color : Color.white;
        }

        private void Update()
        {
            age += Time.deltaTime;
            transform.position += velocity * Time.deltaTime;

            var mainCamera = UnityEngine.Camera.main;
            if (mainCamera != null)
            {
                transform.rotation = mainCamera.transform.rotation;
            }

            if (text != null)
            {
                var color = baseColor;
                color.a = Mathf.Clamp01(1f - age / lifetime);
                text.color = color;
            }

            if (age >= lifetime)
            {
                Destroy(gameObject);
            }
        }

        public static void Spawn(Vector3 worldPosition, string message, Color color, int fontSize = 34)
        {
            var root = new GameObject("Floating Text");
            root.transform.position = worldPosition;
            root.transform.localScale = Vector3.one * 0.01f;

            var canvas = root.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.WorldSpace;

            var canvasRect = root.GetComponent<RectTransform>();
            canvasRect.sizeDelta = new Vector2(260f, 80f);

            var textObject = new GameObject("Text");
            textObject.transform.SetParent(root.transform, false);

            var rect = textObject.AddComponent<RectTransform>();
            rect.anchorMin = Vector2.zero;
            rect.anchorMax = Vector2.one;
            rect.offsetMin = Vector2.zero;
            rect.offsetMax = Vector2.zero;

            var text = textObject.AddComponent<Text>();
            text.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            text.fontSize = fontSize;
            text.fontStyle = FontStyle.Bold;
            text.alignment = TextAnchor.MiddleCenter;
            text.color = color;
            text.raycastTarget = false;
            text.text = message;

            root.AddComponent<FloatingText>();
        }
    }
}
