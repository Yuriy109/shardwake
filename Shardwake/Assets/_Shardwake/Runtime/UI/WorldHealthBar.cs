using Shardwake.Combat;
using UnityEngine;
using UnityEngine.UI;

namespace Shardwake.UI
{
    [RequireComponent(typeof(Health))]
    public sealed class WorldHealthBar : MonoBehaviour
    {
        [SerializeField] private Vector3 offset = new(0f, 1.65f, 0f);
        [SerializeField] private Color fillColor = new(0.25f, 0.95f, 0.42f);

        private Health health;
        private RectTransform fillRect;
        private Canvas canvas;

        private void Awake()
        {
            health = GetComponent<Health>();
            CreateBar();
        }

        private void LateUpdate()
        {
            if (canvas == null)
            {
                return;
            }

            canvas.transform.position = transform.position + offset;

            var mainCamera = UnityEngine.Camera.main;
            if (mainCamera != null)
            {
                canvas.transform.rotation = mainCamera.transform.rotation;
            }

            fillRect.localScale = new Vector3(Mathf.Clamp01(health.Normalized), 1f, 1f);
        }

        public void SetFillColor(Color color)
        {
            fillColor = color;

            if (fillRect != null && fillRect.TryGetComponent(out Image image))
            {
                image.color = fillColor;
            }
        }

        private void CreateBar()
        {
            var canvasObject = new GameObject("Health Bar");
            canvasObject.transform.SetParent(transform, false);
            canvasObject.transform.localPosition = offset;

            canvas = canvasObject.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.WorldSpace;

            var canvasRect = canvasObject.GetComponent<RectTransform>();
            canvasRect.sizeDelta = new Vector2(1.25f, 0.16f);

            CreateImage(canvasObject.transform, "Background", new Color(0.05f, 0.05f, 0.07f, 0.85f), Vector2.one);
            fillRect = CreateImage(canvasObject.transform, "Fill", fillColor, Vector2.one);
            fillRect.anchorMin = new Vector2(0f, 0.5f);
            fillRect.anchorMax = new Vector2(0f, 0.5f);
            fillRect.pivot = new Vector2(0f, 0.5f);
            fillRect.anchoredPosition = Vector2.zero;
        }

        private static RectTransform CreateImage(Transform parent, string objectName, Color color, Vector2 size)
        {
            var imageObject = new GameObject(objectName);
            imageObject.transform.SetParent(parent, false);

            var rect = imageObject.AddComponent<RectTransform>();
            rect.anchorMin = new Vector2(0.5f, 0.5f);
            rect.anchorMax = new Vector2(0.5f, 0.5f);
            rect.pivot = new Vector2(0.5f, 0.5f);
            rect.sizeDelta = size;

            var image = imageObject.AddComponent<Image>();
            image.color = color;
            image.raycastTarget = false;
            return rect;
        }
    }
}
