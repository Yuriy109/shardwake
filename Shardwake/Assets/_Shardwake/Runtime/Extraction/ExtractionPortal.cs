using Shardwake.Combat;
using Shardwake.Core;
using Shardwake.Inventory;
using Shardwake.Map;
using Shardwake.Rendering;
using Shardwake.UI;
using UnityEngine;

namespace Shardwake.Extraction
{
    public sealed class ExtractionPortal : MonoBehaviour
    {
        [SerializeField] private float extractionRadius = 3f;
        [SerializeField] private float discoveryRadius = 10f;
        [SerializeField] private float channelSeconds = 3f;
        [SerializeField] private PortalKind portalKind = PortalKind.NormalExtraction;

        private Renderer portalRenderer;
        private float channelProgress;
        private float lastKnownDamageTime;
        private float nextChannelTextTime;
        private bool discovered;

        private void Awake()
        {
            portalRenderer = GetComponent<Renderer>();
        }

        public void Configure(PortalKind kind)
        {
            portalKind = kind;
        }

        private void Update()
        {
            var session = ShardwakeSession.Instance;
            if (session == null)
            {
                return;
            }

            var inventory = Object.FindFirstObjectByType<InventoryComponent>();
            TryDiscover(session, inventory);

            var active = IsActive(session);
            if (portalRenderer != null)
            {
                GreyboxMaterial.Apply(portalRenderer, active ? ActiveColor() : InactiveColor());
            }

            if (!session.HasStarted || !active || session.IsFinished)
            {
                channelProgress = 0f;
                return;
            }

            if (inventory == null)
            {
                channelProgress = 0f;
                return;
            }

            var playerHealth = inventory.GetComponent<Health>();
            if (playerHealth != null && playerHealth.LastDamageTime > lastKnownDamageTime)
            {
                lastKnownDamageTime = playerHealth.LastDamageTime;
                if (channelProgress > 0f)
                {
                    FloatingText.Spawn(inventory.transform.position + Vector3.up * 2.5f, "EXTRACTION INTERRUPTED", new Color(1f, 0.3f, 0.22f), 26);
                }

                channelProgress = 0f;
            }

            if (Vector3.Distance(transform.position, inventory.transform.position) > extractionRadius)
            {
                channelProgress = 0f;
                return;
            }

            channelProgress += Time.deltaTime;
            if (channelProgress >= channelSeconds)
            {
                session.Extract(inventory, portalKind == PortalKind.HellExtraction);
                gameObject.SetActive(false);
            }
            else if (Time.time >= nextChannelTextTime)
            {
                nextChannelTextTime = Time.time + 0.8f;
                var remaining = Mathf.CeilToInt(channelSeconds - channelProgress);
                FloatingText.Spawn(transform.position + Vector3.up * 1.4f, $"Extracting {remaining}", Color.white, 20);
            }
        }

        private void TryDiscover(ShardwakeSession session, InventoryComponent inventory)
        {
            if (discovered || session == null || inventory == null)
            {
                return;
            }

            if (Vector3.Distance(transform.position, inventory.transform.position) > discoveryRadius)
            {
                return;
            }

            var kind = portalKind == PortalKind.HellExtraction ? MapMarkerKind.HellExtraction : MapMarkerKind.NormalExtraction;
            discovered = session.DiscoverMapMarker(GetInstanceID(), kind, transform.position);
            if (discovered)
            {
                var label = portalKind == PortalKind.HellExtraction ? "Hell exit discovered" : "Portal location discovered";
                FloatingText.Spawn(transform.position + Vector3.up * 1.8f, label.ToUpperInvariant(), new Color(0.62f, 0.85f, 1f), 24);
            }
        }

        private bool IsActive(ShardwakeSession session)
        {
            return portalKind == PortalKind.HellExtraction
                ? session.IsHellExtractionOpen
                : session.IsPortalUnlocked;
        }

        private Color ActiveColor()
        {
            return portalKind == PortalKind.HellExtraction
                ? new Color(1f, 0.25f, 0.08f)
                : new Color(0.55f, 0.25f, 1f);
        }

        private Color InactiveColor()
        {
            return portalKind == PortalKind.HellExtraction
                ? new Color(0.24f, 0.06f, 0.03f)
                : new Color(0.18f, 0.12f, 0.3f);
        }
    }
}
