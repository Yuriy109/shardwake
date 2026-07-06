using Shardwake.Player;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Shardwake.UI
{
    public sealed class VirtualJoystick : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
    {
        [SerializeField] private float radius = 92f;

        private RectTransform rectTransform;
        private RectTransform handle;

        private void Awake()
        {
            rectTransform = GetComponent<RectTransform>();
            handle = CreateCircle("Handle", new Color(1f, 1f, 1f, 0.65f), 82f);
            handle.anchoredPosition = Vector2.zero;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            UpdateInput(eventData);
        }

        public void OnDrag(PointerEventData eventData)
        {
            UpdateInput(eventData);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            handle.anchoredPosition = Vector2.zero;
            MobileInput.SetMovement(Vector2.zero);
        }

        private void UpdateInput(PointerEventData eventData)
        {
            if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, eventData.position, eventData.pressEventCamera, out var localPoint))
            {
                return;
            }

            var input = Vector2.ClampMagnitude(localPoint / radius, 1f);
            handle.anchoredPosition = input * radius;
            MobileInput.SetMovement(input);
        }

        private RectTransform CreateCircle(string objectName, Color color, float size)
        {
            var circle = new GameObject(objectName);
            circle.transform.SetParent(transform, false);

            var rect = circle.AddComponent<RectTransform>();
            rect.anchorMin = new Vector2(0.5f, 0.5f);
            rect.anchorMax = new Vector2(0.5f, 0.5f);
            rect.pivot = new Vector2(0.5f, 0.5f);
            rect.sizeDelta = new Vector2(size, size);

            var image = circle.AddComponent<Image>();
            image.color = color;
            image.raycastTarget = false;
            return rect;
        }
    }
}
