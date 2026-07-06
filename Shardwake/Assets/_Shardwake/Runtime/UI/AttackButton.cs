using Shardwake.Player;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Shardwake.UI
{
    public sealed class AttackButton : MonoBehaviour, IPointerDownHandler
    {
        [SerializeField] private float visualCooldown = 0.45f;

        private Image background;
        private Text label;
        private float cooldownRemaining;

        private void Awake()
        {
            background = GetComponent<Image>();
            label = GetComponentInChildren<Text>();
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            MobileInput.QueueAttack();
            cooldownRemaining = visualCooldown;

            if (label != null)
            {
                label.color = new Color(1f, 0.86f, 0.48f);
            }
        }

        private void LateUpdate()
        {
            cooldownRemaining = Mathf.Max(0f, cooldownRemaining - Time.deltaTime);

            if (background != null)
            {
                var ready = cooldownRemaining <= 0f;
                background.color = ready
                    ? new Color(0.95f, 0.18f, 0.16f, 0.74f)
                    : new Color(0.55f, 0.08f, 0.08f, 0.62f);
            }

            if (label != null)
            {
                label.color = cooldownRemaining > 0f ? new Color(1f, 0.86f, 0.48f) : Color.white;
            }
        }
    }
}
