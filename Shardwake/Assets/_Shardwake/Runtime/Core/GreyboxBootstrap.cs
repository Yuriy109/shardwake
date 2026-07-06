using Shardwake.Cameras;
using Shardwake.Combat;
using Shardwake.Monsters;
using Shardwake.Extraction;
using Shardwake.Equipment;
using Shardwake.Inventory;
using Shardwake.Insurance;
using Shardwake.Loot;
using Shardwake.Player;
using Shardwake.Rendering;
using Shardwake.UI;
using Shardwake.Map;
using UnityEngine;

namespace Shardwake.Core
{
    public static class GreyboxBootstrap
    {
        private const string MarkerName = "Shardwake Greybox Runtime";
        private static Transform playerTarget;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void BuildIfNeeded()
        {
            if (Object.FindFirstObjectByType<ShardwakeSession>() != null)
            {
                return;
            }

            var root = new GameObject(MarkerName);
            var session = root.AddComponent<ShardwakeSession>();

            CreateLighting();
            var player = CreatePlayer();
            playerTarget = player.transform;
            session.RegisterPlayer(player.GetComponent<PlayerController>(), player.GetComponent<PlayerCombat>(), player.GetComponent<Health>());
            CreateCamera(player.transform);
            CreateDungeon();
            CreateHud(session, player.GetComponent<InventoryComponent>(), player.GetComponent<EquipmentLoadout>());
        }

        private static GameObject CreatePlayer()
        {
            var player = GameObject.CreatePrimitive(PrimitiveType.Capsule);
            player.name = "Seeker";
            player.transform.position = new Vector3(0f, 1f, -22f);
            player.transform.localScale = new Vector3(1f, 1.15f, 1f);
            GreyboxMaterial.Apply(player.GetComponent<Renderer>(), new Color(0.2f, 0.65f, 1f));

            var body = player.AddComponent<Rigidbody>();
            body.constraints = RigidbodyConstraints.FreezeRotation;
            body.interpolation = RigidbodyInterpolation.Interpolate;

            player.AddComponent<Health>();
            player.AddComponent<HitFeedback>();
            player.AddComponent<WorldHealthBar>().SetFillColor(new Color(0.25f, 0.95f, 0.42f));
            player.AddComponent<InventoryComponent>();
            player.AddComponent<EquipmentLoadout>();
            player.AddComponent<InsuranceLoadout>();
            player.AddComponent<PlayerController>();
            player.AddComponent<PlayerCombat>();
            player.AddComponent<PlayerDash>();
            player.AddComponent<PlayerSkills>();
            player.AddComponent<PlayerRegeneration>();
            return player;
        }

        private static void CreateCamera(Transform target)
        {
            var camera = UnityEngine.Camera.main ?? Object.FindFirstObjectByType<UnityEngine.Camera>();
            var cameraObject = camera != null ? camera.gameObject : new GameObject("Shardwake Camera");

            if (camera == null)
            {
                camera = cameraObject.AddComponent<UnityEngine.Camera>();
            }

            camera.orthographic = true;
            camera.orthographicSize = 23f;
            camera.clearFlags = CameraClearFlags.SolidColor;
            camera.backgroundColor = new Color(0.1f, 0.12f, 0.16f);
            cameraObject.tag = "MainCamera";

            var follow = cameraObject.GetComponent<CameraFollow>() ?? cameraObject.AddComponent<CameraFollow>();
            follow.SetTarget(target);
        }

        private static void CreateDungeon()
        {
            var floor = GameObject.CreatePrimitive(PrimitiveType.Cube);
            floor.name = "Shard Floor";
            floor.transform.position = new Vector3(0f, -0.08f, 0f);
            floor.transform.localScale = new Vector3(MapScale.OuterHalfExtent * 2.15f, 0.12f, MapScale.OuterHalfExtent * 2.15f);
            GreyboxMaterial.Apply(floor.GetComponent<Renderer>(), new Color(0.22f, 0.24f, 0.28f));

            CreateOuterBounds();
            CreateFixedCenterWalls();

            var layout = MapGenerator.Generate(System.Environment.TickCount);
            foreach (var module in layout.Modules)
            {
                CreateModuleFloor(module);
                CreateModuleObstacles(module);
                CreateModuleSpawns(module);
            }

            CreateHellgateEntrance(new Vector3(MapGenerator.ModuleSize * 1.25f, 0.25f, -MapGenerator.ModuleSize * 1.25f));
            CreateHellgateEntrance(new Vector3(-MapGenerator.ModuleSize * 1.25f, 0.25f, -MapGenerator.ModuleSize * 1.25f));
            CreatePortal(new Vector3(-MapGenerator.ModuleSize * 1.35f, 0.5f, MapGenerator.ModuleSize * 1.35f), PortalKind.HellExtraction);
            CreatePortal(new Vector3(MapGenerator.ModuleSize * 1.35f, 0.5f, MapGenerator.ModuleSize * 1.35f), PortalKind.HellExtraction);
        }

        private static void CreateOuterBounds()
        {
            var half = MapScale.OuterHalfExtent + MapScale.HalfModule + 2f;
            var width = half * 2f;
            CreateWall("North Outer Wall", new Vector3(0f, 1f, half), new Vector3(width, 2f, 2f));
            CreateWall("South Outer Wall", new Vector3(0f, 1f, -half), new Vector3(width, 2f, 2f));
            CreateWall("West Outer Wall", new Vector3(-half, 1f, 0f), new Vector3(2f, 2f, width));
            CreateWall("East Outer Wall", new Vector3(half, 1f, 0f), new Vector3(2f, 2f, width));
        }

        private static void CreateFixedCenterWalls()
        {
            var half = MapScale.HalfModule + 0.5f;
            var wallLength = 19f;
            CreateWall("Center North Gate Left", new Vector3(-15f, 1f, half), new Vector3(wallLength, 2f, 2f));
            CreateWall("Center North Gate Right", new Vector3(15f, 1f, half), new Vector3(wallLength, 2f, 2f));
            CreateWall("Center South Gate Left", new Vector3(-15f, 1f, -half), new Vector3(wallLength, 2f, 2f));
            CreateWall("Center South Gate Right", new Vector3(15f, 1f, -half), new Vector3(wallLength, 2f, 2f));
            CreateWall("Center West Gate Top", new Vector3(-half, 1f, 15f), new Vector3(2f, 2f, wallLength));
            CreateWall("Center West Gate Bottom", new Vector3(-half, 1f, -15f), new Vector3(2f, 2f, wallLength));
            CreateWall("Center East Gate Top", new Vector3(half, 1f, 15f), new Vector3(2f, 2f, wallLength));
            CreateWall("Center East Gate Bottom", new Vector3(half, 1f, -15f), new Vector3(2f, 2f, wallLength));
        }

        private static void CreateWall(string wallName, Vector3 position, Vector3 scale)
        {
            var wall = GameObject.CreatePrimitive(PrimitiveType.Cube);
            wall.name = wallName;
            wall.transform.position = position;
            wall.transform.localScale = scale;
            GreyboxMaterial.Apply(wall.GetComponent<Renderer>(), new Color(0.18f, 0.18f, 0.23f));
        }

        private static void CreateModuleFloor(PlacedMapModule module)
        {
            var floor = GameObject.CreatePrimitive(PrimitiveType.Cube);
            floor.name = $"Biome Floor - {module.Definition.DisplayName}";
            floor.transform.position = module.Origin + new Vector3(0f, -0.04f, 0f);
            floor.transform.localScale = new Vector3(MapGenerator.ModuleSize - 1.2f, 0.08f, MapGenerator.ModuleSize - 1.2f);
            GreyboxMaterial.Apply(floor.GetComponent<Renderer>(), module.Definition.Color);
        }

        private static void CreateModuleObstacles(PlacedMapModule module)
        {
            for (var i = 0; i < module.Definition.Obstacles.Length; i++)
            {
                var obstacle = GameObject.CreatePrimitive(PrimitiveType.Cube);
                obstacle.name = $"{module.Definition.DisplayName} Obstacle {i + 1}";
                obstacle.transform.position = module.TransformPoint(module.Definition.Obstacles[i]);
                obstacle.transform.localScale = new Vector3(4.5f + i % 2 * 2.0f, 2.3f + i % 3 * 0.55f, 4.0f + i % 3 * 1.2f);
                GreyboxMaterial.Apply(obstacle.GetComponent<Renderer>(), new Color(0.34f, 0.32f, 0.42f));
            }
        }

        private static void CreateModuleSpawns(PlacedMapModule module)
        {
            for (var i = 0; i < module.Definition.Enemies.Length; i++)
            {
                var monsterType = MonsterDefinitions.GetForBiome(module.Definition.Biome, i);
                CreateEnemy(module.TransformPoint(module.Definition.Enemies[i]), monsterType, false);
            }

            // MVP mini-boss slot: not every biome spawns one. This keeps runs less predictable.
            if (Random.value < 0.45f)
            {
                CreateEnemy(module.Origin + new Vector3(0f, 1f, 0f), MonsterDefinitions.GetMiniBossForBiome(module.Definition.Biome), true);
            }

            foreach (var chestPosition in module.Definition.Chests)
            {
                CreateChest(module.TransformPoint(chestPosition));
            }

            foreach (var portalPosition in module.Definition.Portals)
            {
                CreatePortal(module.TransformPoint(portalPosition), PortalKind.NormalExtraction);
            }
        }

        private static void CreateEnemy(Vector3 position, MonsterType monsterType, bool miniBoss)
        {
            var definition = MonsterDefinitions.Get(monsterType);

            var enemy = GameObject.CreatePrimitive(PrimitiveType.Capsule);
            enemy.name = miniBoss ? $"Mini Boss - {definition.DisplayName}" : definition.DisplayName;
            enemy.transform.position = position;
            enemy.transform.localScale = miniBoss ? new Vector3(1.9f, 2.15f, 1.9f) : Vector3.one;
            GreyboxMaterial.Apply(enemy.GetComponent<Renderer>(), miniBoss ? Color.Lerp(definition.Color, new Color(1f, 0.72f, 0.18f), 0.45f) : definition.Color);

            var body = enemy.AddComponent<Rigidbody>();
            body.constraints = RigidbodyConstraints.FreezeRotation;
            body.interpolation = RigidbodyInterpolation.Interpolate;

            enemy.AddComponent<Health>().SetMaxHealth(miniBoss ? definition.MaxHealth * 2.35f : definition.MaxHealth);
            enemy.AddComponent<HitFeedback>();
            enemy.AddComponent<WorldHealthBar>().SetFillColor(miniBoss ? new Color(1f, 0.72f, 0.18f) : new Color(1f, 0.25f, 0.18f));

            if (miniBoss)
            {
                enemy.AddComponent<MiniBossController>().Configure(2, LootSource.MiniBoss);
            }

            var controller = enemy.AddComponent<EnemyController>();
            controller.ApplyDefinition(monsterType, false);
            controller.SetTarget(playerTarget);
        }

        private static void CreateChest(Vector3 position)
        {
            var chest = GameObject.CreatePrimitive(PrimitiveType.Cube);
            chest.name = "Relic Chest";
            chest.transform.position = position;
            chest.transform.localScale = new Vector3(1.35f, 1f, 1.1f);
            GreyboxMaterial.Apply(chest.GetComponent<Renderer>(), new Color(1f, 0.72f, 0.22f));
            chest.AddComponent<LootChest>();
        }

        private static void CreatePortal(Vector3 position, PortalKind kind)
        {
            var portal = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            portal.name = kind == PortalKind.HellExtraction ? "Hell Extraction Portal" : "Extraction Portal";
            portal.transform.position = position;
            portal.transform.localScale = new Vector3(2.4f, 0.16f, 2.4f);
            GreyboxMaterial.Apply(portal.GetComponent<Renderer>(), kind == PortalKind.HellExtraction ? new Color(1f, 0.18f, 0.08f) : new Color(0.45f, 0.2f, 1f));
            portal.AddComponent<ExtractionPortal>().Configure(kind);
        }

        private static void CreateHellgateEntrance(Vector3 position)
        {
            var gate = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            gate.name = "Hellgate Entrance";
            gate.transform.position = position;
            gate.transform.localScale = new Vector3(2.8f, 0.18f, 2.8f);
            GreyboxMaterial.Apply(gate.GetComponent<Renderer>(), new Color(0.28f, 0.08f, 0.05f));
            gate.AddComponent<HellgateEntrance>();
        }

        private static void CreateLighting()
        {
            if (Object.FindFirstObjectByType<Light>() != null)
            {
                return;
            }

            var lightObject = new GameObject("Shard Sun");
            var light = lightObject.AddComponent<Light>();
            light.type = LightType.Directional;
            light.intensity = 1.8f;
            lightObject.transform.rotation = Quaternion.Euler(55f, -35f, 0f);
        }

        private static void CreateHud(ShardwakeSession session, InventoryComponent inventory, EquipmentLoadout equipmentLoadout)
        {
            var hudObject = new GameObject("Greybox HUD");
            var hud = hudObject.AddComponent<GreyboxHud>();
            hud.Bind(session, inventory);
            session.RegisterHud(hud);

            var inventoryPanel = hudObject.AddComponent<GreyboxInventoryDebugPanel>();
            inventoryPanel.Bind(inventory, equipmentLoadout);
            hud.BindInventoryPanel(inventoryPanel);
        }
    }
}
