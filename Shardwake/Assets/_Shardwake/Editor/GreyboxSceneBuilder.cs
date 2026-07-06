using System.Collections.Generic;
using Shardwake.Cameras;
using Shardwake.Combat;
using Shardwake.Core;
using Shardwake.Monsters;
using Shardwake.Extraction;
using Shardwake.Inventory;
using Shardwake.Loot;
using Shardwake.Player;
using Shardwake.UI;
using Shardwake.Map;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Shardwake.EditorTools
{
    public static class GreyboxSceneBuilder
    {
        private const string RootPath = "Assets/_Shardwake";
        private const string PrefabPath = RootPath + "/Prefabs";
        private const string ScenePath = RootPath + "/Scenes";
        private const string MaterialPath = RootPath + "/Art/Materials";
        private const string GreyboxScenePath = ScenePath + "/GreyboxShard.unity";

        [InitializeOnLoadMethod]
        private static void AutoBuildOnce()
        {
            EditorApplication.delayCall += AutoBuildWhenReady;
        }

        private static void AutoBuildWhenReady()
        {
            if (EditorApplication.isPlayingOrWillChangePlaymode)
            {
                EditorApplication.delayCall += AutoBuildWhenReady;
                return;
            }

            if (AssetDatabase.LoadAssetAtPath<SceneAsset>(GreyboxScenePath) != null)
            {
                return;
            }

            BuildAll(overwriteExisting: false);
        }

        [MenuItem("Shardwake/Build Greybox Scene")]
        public static void BuildFromMenu()
        {
            BuildAll(overwriteExisting: true);
            EditorUtility.DisplayDialog("Shardwake", "Greybox prefabs and scene are ready.", "OK");
        }

        public static void BuildForBatchmode()
        {
            BuildAll(overwriteExisting: true);
        }

        [MenuItem("Shardwake/Reset Haven Progress")]
        public static void ResetHavenProgress()
        {
            HavenProgress.Reset();
            EditorUtility.DisplayDialog("Shardwake", "Haven prototype progress has been reset.", "OK");
        }

        private static void BuildAll(bool overwriteExisting)
        {
            EnsureFolders();
            var materials = CreateMaterials(overwriteExisting);
            CreatePrefabs(materials, overwriteExisting);
            CreateScene(materials, overwriteExisting);
            AddSceneToBuildSettings();
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        private static void EnsureFolders()
        {
            EnsureFolder(RootPath, "Prefabs");
            EnsureFolder(RootPath, "Scenes");
            EnsureFolder(RootPath, "Art");
            EnsureFolder(RootPath + "/Art", "Materials");
        }

        private static void EnsureFolder(string parent, string child)
        {
            var path = parent + "/" + child;
            if (!AssetDatabase.IsValidFolder(path))
            {
                AssetDatabase.CreateFolder(parent, child);
            }
        }

        private static Dictionary<string, Material> CreateMaterials(bool overwriteExisting)
        {
            var materials = new Dictionary<string, Material>
            {
                ["Seeker"] = CreateMaterial("Seeker_Blue", new Color(0.2f, 0.65f, 1f), overwriteExisting),
                ["Shardling"] = CreateMaterial("Shardling_Red", new Color(0.9f, 0.28f, 0.22f), overwriteExisting),
                ["Chest"] = CreateMaterial("RelicChest_Gold", new Color(1f, 0.72f, 0.22f), overwriteExisting),
                ["Portal"] = CreateMaterial("Portal_Violet", new Color(0.45f, 0.2f, 1f), overwriteExisting),
                ["Floor"] = CreateMaterial("ShardFloor_Grey", new Color(0.22f, 0.24f, 0.28f), overwriteExisting),
                ["Ruin"] = CreateMaterial("AncientRuin_Mauve", new Color(0.34f, 0.32f, 0.42f), overwriteExisting),
                ["Wall"] = CreateMaterial("DungeonWall_Stone", new Color(0.18f, 0.18f, 0.23f), overwriteExisting)
            };

            return materials;
        }

        private static Material CreateMaterial(string materialName, Color color, bool overwriteExisting)
        {
            var path = $"{MaterialPath}/{materialName}.mat";
            var existing = AssetDatabase.LoadAssetAtPath<Material>(path);
            if (existing != null && !overwriteExisting)
            {
                return existing;
            }

            var shader = Shader.Find("Universal Render Pipeline/Lit") ?? Shader.Find("Standard");
            var material = existing != null ? existing : new Material(shader);
            material.color = color;

            if (existing == null)
            {
                AssetDatabase.CreateAsset(material, path);
            }
            else
            {
                EditorUtility.SetDirty(material);
            }

            return material;
        }

        private static void CreatePrefabs(Dictionary<string, Material> materials, bool overwriteExisting)
        {
            SavePrefab("Seeker.prefab", CreateSeeker(materials["Seeker"]), overwriteExisting);
            SavePrefab("Shardling.prefab", CreateShardling(materials["Shardling"]), overwriteExisting);
            SavePrefab("RelicChest.prefab", CreateChest(materials["Chest"]), overwriteExisting);
            SavePrefab("ExtractionPortal.prefab", CreatePortal(materials["Portal"]), overwriteExisting);
        }

        private static GameObject CreateSeeker(Material material)
        {
            var seeker = GameObject.CreatePrimitive(PrimitiveType.Capsule);
            seeker.name = "Seeker";
            seeker.transform.localScale = new Vector3(1f, 1.15f, 1f);
            seeker.GetComponent<Renderer>().sharedMaterial = material;

            var body = seeker.AddComponent<Rigidbody>();
            body.constraints = RigidbodyConstraints.FreezeRotation;
            body.interpolation = RigidbodyInterpolation.Interpolate;

            seeker.AddComponent<Health>();
            seeker.AddComponent<HitFeedback>();
            seeker.AddComponent<WorldHealthBar>().SetFillColor(new Color(0.25f, 0.95f, 0.42f));
            seeker.AddComponent<InventoryComponent>();
            seeker.AddComponent<PlayerController>();
            seeker.AddComponent<PlayerCombat>();
            seeker.AddComponent<PlayerDash>();
            seeker.AddComponent<PlayerSkills>();
            return seeker;
        }

        private static GameObject CreateShardling(Material material)
        {
            var enemy = GameObject.CreatePrimitive(PrimitiveType.Capsule);
            enemy.name = "Shardling";
            enemy.GetComponent<Renderer>().sharedMaterial = material;

            var body = enemy.AddComponent<Rigidbody>();
            body.constraints = RigidbodyConstraints.FreezeRotation;
            body.interpolation = RigidbodyInterpolation.Interpolate;

            enemy.AddComponent<Health>().SetMaxHealth(45f);
            enemy.AddComponent<HitFeedback>();
            enemy.AddComponent<WorldHealthBar>().SetFillColor(new Color(1f, 0.25f, 0.18f));
            enemy.AddComponent<EnemyController>();
            return enemy;
        }

        private static GameObject CreateChest(Material material)
        {
            var chest = GameObject.CreatePrimitive(PrimitiveType.Cube);
            chest.name = "RelicChest";
            chest.transform.localScale = new Vector3(1.35f, 1f, 1.1f);
            chest.GetComponent<Renderer>().sharedMaterial = material;
            chest.AddComponent<LootChest>();
            return chest;
        }

        private static GameObject CreatePortal(Material material)
        {
            var portal = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            portal.name = "ExtractionPortal";
            portal.transform.localScale = new Vector3(2.4f, 0.16f, 2.4f);
            portal.GetComponent<Renderer>().sharedMaterial = material;
            portal.AddComponent<ExtractionPortal>();
            return portal;
        }

        private static void SavePrefab(string fileName, GameObject instance, bool overwriteExisting)
        {
            var path = $"{PrefabPath}/{fileName}";
            if (AssetDatabase.LoadAssetAtPath<GameObject>(path) != null && !overwriteExisting)
            {
                Object.DestroyImmediate(instance);
                return;
            }

            PrefabUtility.SaveAsPrefabAsset(instance, path);
            Object.DestroyImmediate(instance);
        }

        private static void CreateScene(Dictionary<string, Material> materials, bool overwriteExisting)
        {
            if (AssetDatabase.LoadAssetAtPath<SceneAsset>(GreyboxScenePath) != null && !overwriteExisting)
            {
                return;
            }

            var previousActiveScene = SceneManager.GetActiveScene();
            var scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Additive);
            EditorSceneManager.SetActiveScene(scene);
            scene.name = "GreyboxShard";

            var sessionObject = new GameObject("GameSession");
            sessionObject.AddComponent<ShardwakeSession>();
            sessionObject.AddComponent<GreyboxSceneInitializer>();

            CreateSceneLighting();
            var seeker = InstantiatePrefab("Seeker.prefab", scene, new Vector3(0f, 1f, -8f), Quaternion.identity);
            CreateSceneCamera(seeker.transform);
            var layout = MapGenerator.Generate(System.Environment.TickCount);
            CreateSceneDungeon(materials, layout, scene);
            new GameObject("Greybox HUD").AddComponent<GreyboxHud>();

            EditorSceneManager.SaveScene(scene, GreyboxScenePath);
            if (previousActiveScene.IsValid())
            {
                EditorSceneManager.SetActiveScene(previousActiveScene);
            }

            EditorSceneManager.CloseScene(scene, removeScene: true);
        }

        private static void CreateSceneLighting()
        {
            var lightObject = new GameObject("Shard Sun");
            var light = lightObject.AddComponent<Light>();
            light.type = LightType.Directional;
            light.intensity = 1.8f;
            lightObject.transform.rotation = Quaternion.Euler(55f, -35f, 0f);
        }

        private static void CreateSceneCamera(Transform target)
        {
            var cameraObject = new GameObject("Shardwake Camera");
            cameraObject.tag = "MainCamera";
            var camera = cameraObject.AddComponent<Camera>();
            camera.orthographic = true;
            camera.orthographicSize = 9f;
            camera.clearFlags = CameraClearFlags.SolidColor;
            camera.backgroundColor = new Color(0.1f, 0.12f, 0.16f);

            var follow = cameraObject.AddComponent<CameraFollow>();
            follow.SetTarget(target);
        }

        private static void CreateSceneDungeon(Dictionary<string, Material> materials, MapLayout layout, Scene scene)
        {
            var floor = GameObject.CreatePrimitive(PrimitiveType.Cube);
            floor.name = "Shard Floor";
            floor.transform.position = new Vector3(0f, -0.08f, 0f);
            floor.transform.localScale = new Vector3(38f, 0.12f, 38f);
            floor.GetComponent<Renderer>().sharedMaterial = materials["Floor"];

            CreateOuterBounds(materials["Wall"]);
            CreateFixedCenterWalls(materials["Wall"]);

            foreach (var module in layout.Modules)
            {
                CreateModuleFloor(module);
                CreateModuleObstacles(module, materials["Ruin"]);
                CreateModuleSpawns(module, scene);
            }
        }

        private static void CreateOuterBounds(Material wallMaterial)
        {
            CreateWall("North Outer Wall", new Vector3(0f, 1f, 18.5f), new Vector3(38f, 2f, 1f), wallMaterial);
            CreateWall("South Outer Wall", new Vector3(0f, 1f, -18.5f), new Vector3(38f, 2f, 1f), wallMaterial);
            CreateWall("West Outer Wall", new Vector3(-18.5f, 1f, 0f), new Vector3(1f, 2f, 38f), wallMaterial);
            CreateWall("East Outer Wall", new Vector3(18.5f, 1f, 0f), new Vector3(1f, 2f, 38f), wallMaterial);
        }

        private static void CreateFixedCenterWalls(Material wallMaterial)
        {
            CreateWall("Center North Gate Left", new Vector3(-3.5f, 1f, 6.1f), new Vector3(5f, 2f, 1f), wallMaterial);
            CreateWall("Center North Gate Right", new Vector3(3.5f, 1f, 6.1f), new Vector3(5f, 2f, 1f), wallMaterial);
            CreateWall("Center South Gate Left", new Vector3(-3.5f, 1f, -6.1f), new Vector3(5f, 2f, 1f), wallMaterial);
            CreateWall("Center South Gate Right", new Vector3(3.5f, 1f, -6.1f), new Vector3(5f, 2f, 1f), wallMaterial);
            CreateWall("Center West Gate Top", new Vector3(-6.1f, 1f, 3.5f), new Vector3(1f, 2f, 5f), wallMaterial);
            CreateWall("Center West Gate Bottom", new Vector3(-6.1f, 1f, -3.5f), new Vector3(1f, 2f, 5f), wallMaterial);
            CreateWall("Center East Gate Top", new Vector3(6.1f, 1f, 3.5f), new Vector3(1f, 2f, 5f), wallMaterial);
            CreateWall("Center East Gate Bottom", new Vector3(6.1f, 1f, -3.5f), new Vector3(1f, 2f, 5f), wallMaterial);
        }

        private static void CreateModuleFloor(PlacedMapModule module)
        {
            var floor = GameObject.CreatePrimitive(PrimitiveType.Cube);
            floor.name = $"Biome Floor - {module.Definition.DisplayName}";
            floor.transform.position = module.Origin + new Vector3(0f, -0.04f, 0f);
            floor.transform.localScale = new Vector3(MapGenerator.ModuleSize - 0.35f, 0.08f, MapGenerator.ModuleSize - 0.35f);

            var renderer = floor.GetComponent<Renderer>();
            renderer.material.color = module.Definition.Color;
        }

        private static void CreateModuleObstacles(PlacedMapModule module, Material material)
        {
            for (var i = 0; i < module.Definition.Obstacles.Length; i++)
            {
                var obstacle = GameObject.CreatePrimitive(PrimitiveType.Cube);
                obstacle.name = $"{module.Definition.DisplayName} Obstacle {i + 1}";
                obstacle.transform.position = module.TransformPoint(module.Definition.Obstacles[i]);
                obstacle.transform.localScale = new Vector3(1.4f + i % 2 * 0.4f, 1.1f + i % 3 * 0.35f, 1.25f);
                obstacle.GetComponent<Renderer>().sharedMaterial = material;
            }
        }

        private static void CreateModuleSpawns(PlacedMapModule module, Scene scene)
        {
            foreach (var enemy in module.Definition.Enemies)
            {
                InstantiatePrefab("Shardling.prefab", scene, module.TransformPoint(enemy), Quaternion.identity);
            }

            foreach (var chest in module.Definition.Chests)
            {
                InstantiatePrefab("RelicChest.prefab", scene, module.TransformPoint(chest), Quaternion.identity);
            }

            foreach (var portal in module.Definition.Portals)
            {
                InstantiatePrefab("ExtractionPortal.prefab", scene, module.TransformPoint(portal), Quaternion.identity);
            }
        }

        private static void CreateWall(string wallName, Vector3 position, Vector3 scale, Material material)
        {
            var wall = GameObject.CreatePrimitive(PrimitiveType.Cube);
            wall.name = wallName;
            wall.transform.position = position;
            wall.transform.localScale = scale;
            wall.GetComponent<Renderer>().sharedMaterial = material;
        }

        private static GameObject InstantiatePrefab(string fileName, Scene destinationScene, Vector3 position, Quaternion rotation)
        {
            var prefab = AssetDatabase.LoadAssetAtPath<GameObject>($"{PrefabPath}/{fileName}");
            var instance = (GameObject)PrefabUtility.InstantiatePrefab(prefab, destinationScene);
            instance.transform.SetPositionAndRotation(position, rotation);
            return instance;
        }

        private static void AddSceneToBuildSettings()
        {
            var scenes = new List<EditorBuildSettingsScene>(EditorBuildSettings.scenes);
            foreach (var scene in scenes)
            {
                if (scene.path == GreyboxScenePath)
                {
                    scene.enabled = true;
                    EditorBuildSettings.scenes = scenes.ToArray();
                    return;
                }
            }

            scenes.Add(new EditorBuildSettingsScene(GreyboxScenePath, true));
            EditorBuildSettings.scenes = scenes.ToArray();
        }
    }
}
