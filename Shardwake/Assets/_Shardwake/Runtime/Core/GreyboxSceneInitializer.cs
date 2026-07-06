using Shardwake.Cameras;
using Shardwake.Equipment;
using Shardwake.Combat;
using Shardwake.Monsters;
using Shardwake.Inventory;
using Shardwake.Player;
using Shardwake.UI;
using UnityEngine;

namespace Shardwake.Core
{
    public sealed class GreyboxSceneInitializer : MonoBehaviour
    {
        [SerializeField] private ShardwakeSession session;
        [SerializeField] private GreyboxHud hud;
        [SerializeField] private InventoryComponent playerInventory;
        [SerializeField] private Transform player;
        [SerializeField] private CameraFollow cameraFollow;

        private void Awake()
        {
            session ??= Object.FindFirstObjectByType<ShardwakeSession>();
            hud ??= Object.FindFirstObjectByType<GreyboxHud>();
            playerInventory ??= Object.FindFirstObjectByType<InventoryComponent>();

            if (player == null && playerInventory != null)
            {
                player = playerInventory.transform;
            }

            cameraFollow ??= Object.FindFirstObjectByType<CameraFollow>();

            if (session != null && hud != null)
            {
                hud.Bind(session, playerInventory);
                session.RegisterHud(hud);
            }

            if (session != null && player != null)
            {
                EnsurePlayerComponents(player.gameObject);
                session.RegisterPlayer(player.GetComponent<PlayerController>(), player.GetComponent<PlayerCombat>(), player.GetComponent<Health>());
            }

            if (cameraFollow != null && player != null)
            {
                cameraFollow.SetTarget(player);
            }

            var enemies = Object.FindObjectsByType<EnemyController>(FindObjectsSortMode.None);
            foreach (var enemy in enemies)
            {
                enemy.SetTarget(player);
            }
        }

        private static void EnsurePlayerComponents(GameObject playerObject)
        {
            
            if (playerObject.GetComponent<EquipmentLoadout>() == null)
            {
                playerObject.AddComponent<EquipmentLoadout>();
            }

            if (playerObject.GetComponent<PlayerDash>() == null)
            {
                playerObject.AddComponent<PlayerDash>();
            }

            if (playerObject.GetComponent<PlayerSkills>() == null)
            {
                playerObject.AddComponent<PlayerSkills>();
            }

            if (playerObject.GetComponent<PlayerRegeneration>() == null)
            {
                playerObject.AddComponent<PlayerRegeneration>();
            }
        }
    }
}
