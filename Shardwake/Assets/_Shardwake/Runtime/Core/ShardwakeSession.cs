using System.Collections.Generic;
using Shardwake.Death;
using Shardwake.Equipment;
using Shardwake.Insurance;
using Shardwake.Inventory;
using Shardwake.Loot;
using Shardwake.Map;
using Shardwake.Monsters;
using Shardwake.Player;
using Shardwake.UI;
using Shardwake.Stats;
using Shardwake.Weapons;
using UnityEngine;

namespace Shardwake.Core
{
    public sealed class ShardwakeSession : MonoBehaviour
    {
        [Header("Match Timing")]
        [SerializeField] private float shardDurationSeconds = 720f;
        [SerializeField] private float portalUnlockSeconds = 360f;
        [SerializeField] private float hellgateEntranceSeconds = 510f;
        [SerializeField] private float hellDurationSeconds = 180f;

        [Header("Weapon Swap")]
        [SerializeField] private float weaponSwapCooldown = 5f;

        private GreyboxHud hud;
        private PlayerController playerController;
        private PlayerCombat playerCombat;
        private PlayerDash playerDash;
        private EquipmentLoadout equipmentLoadout;
        private InventoryComponent inventory;
        private InsuranceLoadout insuranceLoadout;
        private Combat.Health playerHealth;
        private readonly List<LootItem> collectedLoot = new();
        private readonly List<DiscoveredMapMarker> discoveredMapMarkers = new();
        private readonly WeaponLoadout weaponLoadout = new(WeaponType.GreatWeapon, WeaponType.Bow);
        private float nextWeaponSwapTime;
        private bool started;
        private bool finished;
        private bool shardAlertActive;
        private bool queuedHellgate;
        private bool inHellgate;
        private string alertRelicName = string.Empty;
        private int enemiesKilled;
        private int miniBossesDefeated;
        private float elapsedSeconds;
        private float hellElapsedSeconds;

        public static ShardwakeSession Instance { get; private set; }

        public float RemainingSeconds => inHellgate ? Mathf.Max(0f, hellDurationSeconds - hellElapsedSeconds) : Mathf.Max(0f, shardDurationSeconds - elapsedSeconds);
        public int EnemiesKilled => enemiesKilled;
        public int MiniBossesDefeated => miniBossesDefeated;
        public WeaponType SelectedWeapon1 => weaponLoadout.PrimaryWeaponType;
        public WeaponType SelectedWeapon2 => weaponLoadout.SecondaryWeaponType;
        public WeaponType ActiveWeapon => weaponLoadout.ActiveWeaponType;
        public WeaponLoadout WeaponLoadout => weaponLoadout;
        public bool HasStarted => started;
        public bool IsPortalUnlocked => !inHellgate && elapsedSeconds >= portalUnlockSeconds;
        public bool IsHellgateEntranceAvailable => !inHellgate && elapsedSeconds >= hellgateEntranceSeconds && elapsedSeconds < shardDurationSeconds;
        public bool IsHellgateQueued => queuedHellgate;
        public bool IsInHellgate => inHellgate;
        public bool IsHellExtractionOpen => inHellgate && hellElapsedSeconds >= 45f;
        public bool IsFinished => finished;
        public bool IsShardAlertActive => shardAlertActive;
        public string AlertRelicName => alertRelicName;
        public float ShardAlertThreatMultiplier => shardAlertActive ? 1.35f : 1f;
        public CharacterStats PlayerStats => equipmentLoadout != null ? equipmentLoadout.Stats : CharacterStats.Baseline;
        public float SkillCooldownMultiplier => StatScaling.CooldownMultiplier(PlayerStats.Focus);
        public Transform PlayerTransform => playerController != null ? playerController.transform : null;
        public Combat.Health PlayerHealth => playerHealth;
        public PlayerSkills PlayerSkills => playerController != null ? playerController.GetComponent<PlayerSkills>() : null;
        public PlayerDash PlayerDash => playerDash;
        public IReadOnlyList<DiscoveredMapMarker> DiscoveredMapMarkers => discoveredMapMarkers;
        public float WeaponSwapCooldownRemaining => Mathf.Max(0f, nextWeaponSwapTime - Time.time);
        public float WeaponSwapCooldownRatio => Mathf.Clamp01(WeaponSwapCooldownRemaining / Mathf.Max(0.01f, weaponSwapCooldown));

        private void Awake()
        {
            Instance = this;
        }

        private void Update()
        {
            if (!started || IsFinished)
            {
                return;
            }

            if (inHellgate)
            {
                hellElapsedSeconds += Time.deltaTime;
                if (RemainingSeconds <= 0f)
                {
                    Finish(false, "Hellgate collapsed");
                }
            }
            else
            {
                elapsedSeconds += Time.deltaTime;
                if (RemainingSeconds <= 0f)
                {
                    if (queuedHellgate)
                    {
                        StartHellgatePhase();
                    }
                    else
                    {
                        Finish(false, "Shard collapsed");
                    }
                }
            }

            if (MobileInput.ConsumeWeaponSwap())
            {
                TrySwapWeapon();
            }
        }


        public bool DiscoverMapMarker(int markerId, MapMarkerKind kind, Vector3 position)
        {
            for (var i = 0; i < discoveredMapMarkers.Count; i++)
            {
                if (discoveredMapMarkers[i].Id == markerId)
                {
                    return false;
                }
            }

            discoveredMapMarkers.Add(new DiscoveredMapMarker(markerId, kind, position));
            return true;
        }

        public bool IsMarkerOpen(MapMarkerKind kind)
        {
            return kind switch
            {
                MapMarkerKind.NormalExtraction => IsPortalUnlocked,
                MapMarkerKind.HellgateEntrance => IsHellgateEntranceAvailable,
                MapMarkerKind.HellExtraction => IsHellExtractionOpen,
                _ => false
            };
        }

        public void RegisterHud(GreyboxHud greyboxHud)
        {
            hud = greyboxHud;
        }

        public void RegisterPlayer(PlayerController controller, PlayerCombat combat, Combat.Health health)
        {
            playerController = controller;
            playerCombat = combat;
            playerHealth = health;
            playerDash = controller != null ? controller.GetComponent<PlayerDash>() : null;
            equipmentLoadout = controller != null ? controller.GetComponent<EquipmentLoadout>() : null;
            inventory = controller != null ? controller.GetComponent<InventoryComponent>() : null;
            insuranceLoadout = controller != null ? controller.GetComponent<InsuranceLoadout>() : null;
        }

        public void SelectWeapons(WeaponType weapon1, WeaponType weapon2)
        {
            if (started)
            {
                return;
            }

            if (!WeaponLoadoutRules.CanEquipTogether(weapon1, weapon2))
            {
                return;
            }

            weaponLoadout.ReplaceWeapons(weapon1, weapon2);
            RefreshPlayerBuild();
        }

        public void SelectArmorPreset(ArmorType armorType)
        {
            if (started || equipmentLoadout == null)
            {
                return;
            }

            equipmentLoadout.SetArmorPreset(armorType);
            RefreshPlayerBuild();
        }

        public bool TryEquipStashItem(int stashIndex)
        {
            if (started || equipmentLoadout == null)
            {
                return false;
            }

            if (!HavenStash.TryTakeAt(stashIndex, out var item))
            {
                return false;
            }

            if (!item.IsEquipment || !equipmentLoadout.TryEquip(item, out var replacedItem, out var hasReplacedItem))
            {
                HavenStash.Add(item);
                return false;
            }

            if (hasReplacedItem)
            {
                HavenStash.Add(replacedItem);
            }

            RefreshPlayerBuild();
            return true;
        }

        public void RefreshPlayerBuild()
        {
            if (playerController == null || playerCombat == null || playerHealth == null)
            {
                return;
            }

            ApplySelectedWeapons();
        }

        public void StartExpedition()
        {
            if (started || finished)
            {
                return;
            }

            insuranceLoadout?.AutoInsureFromEquipment(equipmentLoadout);
            ApplySelectedWeapons();
            started = true;
        }

        public bool QueueHellgateEntry()
        {
            if (!started || finished || inHellgate || !IsHellgateEntranceAvailable)
            {
                return false;
            }

            queuedHellgate = true;
            return true;
        }

        public void Extract(InventoryComponent extractingInventory, bool fromHellgate)
        {
            if (!started || finished)
            {
                return;
            }

            if (fromHellgate)
            {
                if (!IsHellExtractionOpen)
                {
                    return;
                }
            }
            else if (!IsPortalUnlocked)
            {
                return;
            }

            Finish(true, fromHellgate ? "Escaped Hellgate" : "Extracted");
        }

        public void RecordLoot(LootItem item)
        {
            if (finished)
            {
                return;
            }

            collectedLoot.Add(item);

            if (item.IsUnstable)
            {
                ActivateShardAlert(item);
            }
        }

        public void RecordDeath(GameObject victim)
        {
            if (finished || victim == null)
            {
                return;
            }

            if (victim.TryGetComponent<PlayerController>(out _))
            {
                Finish(false, "Lost to the Shard");
                return;
            }

            if (victim.TryGetComponent<EnemyController>(out _))
            {
                enemiesKilled++;
            }

            if (victim.TryGetComponent<MiniBossController>(out var miniBoss))
            {
                miniBossesDefeated++;
                miniBoss.GrantRewardsToNearestPlayer();
                FloatingText.Spawn(victim.transform.position + Vector3.up * 3.1f, "MINI-BOSS DOWN", new Color(1f, 0.72f, 0.18f), 34);
            }
        }

        public void TrySwapWeapon()
        {
            if (!started || finished || Time.time < nextWeaponSwapTime)
            {
                return;
            }

            weaponLoadout.Swap();
            nextWeaponSwapTime = Time.time + weaponSwapCooldown;
            ApplyActiveWeaponCombat();

            if (playerController != null)
            {
                FloatingText.Spawn(playerController.transform.position + Vector3.up * 2.4f, WeaponDefinitions.Get(weaponLoadout.ActiveWeaponType).DisplayName, new Color(0.65f, 0.86f, 1f), 30);
            }
        }

        private void StartHellgatePhase()
        {
            inHellgate = true;
            hellElapsedSeconds = 0f;
            FloatingText.Spawn(PlayerTransform != null ? PlayerTransform.position + Vector3.up * 2.6f : Vector3.up * 2f, "HELLGATE BEGINS", new Color(1f, 0.22f, 0.08f), 34);
        }

        private void Finish(bool survived, string outcomeText)
        {
            if (finished)
            {
                return;
            }

            finished = true;

            var savedLoot = new List<LootItem>();
            var droppedLoot = new List<LootItem>();
            var insuredReturned = new List<LootItem>();
            var insuredLost = new List<LootItem>();

            if (survived)
            {
                if (inventory != null)
                {
                    savedLoot.AddRange(inventory.Items);
                    inventory.TakeAll();
                }

                HavenStash.AddRange(savedLoot);
                HavenProgress.RecordExtraction(savedLoot);
            }
            else
            {
                droppedLoot = DeathRules.CollectDroppedItems(equipmentLoadout, inventory);
                if (inventory != null)
                {
                    inventory.TakeAll();
                }

                if (equipmentLoadout != null)
                {
                    equipmentLoadout.ClearEquippedItems();
                }

                ResolveInsurance(droppedLoot, insuredReturned, insuredLost);
                HavenStash.AddRange(insuredReturned);
            }

            var haven = HavenProgress.Current;
            var result = new ExpeditionResult(
                survived,
                enemiesKilled,
                miniBossesDefeated,
                inHellgate ? shardDurationSeconds + hellElapsedSeconds : elapsedSeconds,
                GetBuildName(),
                shardAlertActive,
                alertRelicName,
                savedLoot,
                droppedLoot,
                insuredReturned,
                insuredLost,
                haven,
                outcomeText,
                inHellgate || queuedHellgate);

            hud?.ShowResult(result);
        }

        private void ResolveInsurance(IReadOnlyList<LootItem> droppedLoot, List<LootItem> insuredReturned, List<LootItem> insuredLost)
        {
            if (insuranceLoadout == null)
            {
                return;
            }

            for (var i = 0; i < droppedLoot.Count; i++)
            {
                var item = droppedLoot[i];
                if (!insuranceLoadout.IsInsured(item))
                {
                    continue;
                }

                // Single-player greybox assumption: nobody else extracts with the item.
                // Multiplayer backend will replace this with real item ownership tracking.
                if (InsuranceRules.ReturnsToOwner(false))
                {
                    insuredReturned.Add(item);
                }
                else
                {
                    insuredLost.Add(item);
                }
            }
        }

        private void ApplySelectedWeapons()
        {
            var primary = WeaponDefinitions.Get(weaponLoadout.PrimaryWeaponType);
            var secondary = WeaponDefinitions.Get(weaponLoadout.SecondaryWeaponType);
            var stats = PlayerStats;
            var loadEffects = equipmentLoadout != null ? equipmentLoadout.LoadEffects : LoadEffectRules.Get(LoadCategory.Medium);

            var baseMoveSpeed = Mathf.Min(primary.MoveSpeed, secondary.MoveSpeed);
            var moveSpeed = baseMoveSpeed * loadEffects.MovementSpeedMultiplier * StatScaling.MovementSpeedMultiplier(stats.Agility);
            playerController?.SetMoveSpeed(moveSpeed);

            var baseHealth = Mathf.Max(primary.MaxHealth, secondary.MaxHealth);
            var maxHealth = baseHealth * StatScaling.MaxHealthMultiplier(stats.Vitality) + StatScaling.BonusHealthFromStrength(stats.Strength);
            playerHealth?.SetMaxHealth(maxHealth);
            playerHealth?.ConfigureDefenses(StatScaling.PhysicalResistanceBonus(stats.Vitality), StatScaling.MagicResistanceBonus(stats.Intelligence));

            playerDash?.Configure(loadEffects.RollDistanceMultiplier, loadEffects.RollCooldownMultiplier);
            ApplyActiveWeaponCombat();

            if (playerController != null)
            {
                var loadLabel = equipmentLoadout != null ? equipmentLoadout.LoadCategory.ToString() : "Medium";
                FloatingText.Spawn(playerController.transform.position + Vector3.up * 2.4f, $"{GetBuildName()} / {loadLabel} Load", new Color(0.65f, 0.86f, 1f), 30);
            }
        }

        private void ApplyActiveWeaponCombat()
        {
            var active = WeaponDefinitions.Get(weaponLoadout.ActiveWeaponType);
            var stats = PlayerStats;
            var damage = active.BasicDamage * WeaponScaling.BasicDamageMultiplier(weaponLoadout.ActiveWeaponType, stats);
            var cooldown = active.BasicCooldown * StatScaling.BasicAttackCooldownMultiplier(stats.Agility);
            playerCombat?.Configure(damage, cooldown, active.BasicRadius);
        }

        public float GetSkillDamageMultiplier(WeaponSkillDefinition skill)
        {
            return WeaponScaling.SkillDamageMultiplier(skill, PlayerStats);
        }

        private string GetBuildName()
        {
            return $"{WeaponDefinitions.Get(weaponLoadout.PrimaryWeaponType).DisplayName} + {WeaponDefinitions.Get(weaponLoadout.SecondaryWeaponType).DisplayName}";
        }

        private void ActivateShardAlert(LootItem item)
        {
            if (shardAlertActive)
            {
                return;
            }

            shardAlertActive = true;
            alertRelicName = item.DisplayName;

            if (playerController != null)
            {
                FloatingText.Spawn(playerController.transform.position + Vector3.up * 2.8f, "SHARD ALERT", new Color(1f, 0.34f, 0.28f), 34);
            }
        }
    }
}
