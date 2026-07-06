using Shardwake.Core;
using Shardwake.Map;
using Shardwake.Rendering;
using Shardwake.UI;
using UnityEngine;

namespace Shardwake.Extraction
{
    public sealed class HellgateEntrance : MonoBehaviour
    {
        [SerializeField] private float interactRadius = 3.2f;
        [SerializeField] private float discoveryRadius = 10f;

        private Renderer entranceRenderer;
        private bool usedByPlayer;
        private bool discovered;

        private void Awake()
        {
            entranceRenderer = GetComponent<Renderer>();
        }

        private void Update()
        {
            var session = ShardwakeSession.Instance;
            if (session == null || session.IsFinished)
            {
                return;
            }

            TryDiscover(session);

            if (entranceRenderer != null)
            {
                GreyboxMaterial.Apply(entranceRenderer, session.IsHellgateEntranceAvailable
                    ? new Color(1f, 0.18f, 0.08f)
                    : new Color(0.28f, 0.08f, 0.05f));
            }

            if (!session.IsHellgateEntranceAvailable || usedByPlayer)
            {
                return;
            }

            var player = session.PlayerTransform;
            if (player == null)
            {
                return;
            }

            if (Vector3.Distance(transform.position, player.position) <= interactRadius)
            {
                usedByPlayer = session.QueueHellgateEntry();
                if (usedByPlayer)
                {
                    FloatingText.Spawn(transform.position + Vector3.up * 1.8f, "HELLGATE QUEUED", new Color(1f, 0.32f, 0.15f), 30);
                }
            }
        }
        private void TryDiscover(ShardwakeSession session)
        {
            if (discovered || session == null || session.PlayerTransform == null)
            {
                return;
            }

            if (Vector3.Distance(transform.position, session.PlayerTransform.position) > discoveryRadius)
            {
                return;
            }

            discovered = session.DiscoverMapMarker(GetInstanceID(), MapMarkerKind.HellgateEntrance, transform.position);
            if (discovered)
            {
                FloatingText.Spawn(transform.position + Vector3.up * 1.9f, "HELLGATE LOCATION DISCOVERED", new Color(1f, 0.38f, 0.16f), 24);
            }
        }

    }
}
