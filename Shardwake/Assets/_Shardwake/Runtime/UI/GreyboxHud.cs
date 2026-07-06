using System.Text;
using Shardwake.Combat;
using Shardwake.Core;
using Shardwake.Equipment;
using Shardwake.Inventory;
using Shardwake.Loot;
using Shardwake.Map;
using Shardwake.Player;
using Shardwake.Weapons;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.UI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Shardwake.UI
{
    /// <summary>
    /// Temporary mobile-first greybox HUD.
    /// This is still generated in code for prototyping, but it avoids desktop-only debug panels
    /// and exposes the information needed to test the MVP loop on a phone screen.
    /// </summary>
    public sealed class GreyboxHud : MonoBehaviour
    {
        private readonly Image[] skillCooldownFills = new Image[3];
        private readonly Text[] skillLabels = new Text[3];
        private Image dashCooldownFill;
        private Image swapCooldownFill;
        private Image consumable1CooldownFill;
        private Image consumable2CooldownFill;

        private ShardwakeSession session;
        private InventoryComponent inventory;
        private PlayerSkills playerSkills;
        private PlayerDash playerDash;
        private Health playerHealth;
        private GreyboxInventoryDebugPanel inventoryPanel;
        private HavenView havenView = HavenView.Expedition;
        private string havenMessage = "Choose gear and enter the Shard.";

        private Text phaseText;
        private Text objectiveText;
        private Text lootText;
        private Text activeWeaponText;
        private Text markerText;
        private Text hpText;
        private Image hpFill;

        private RectTransform safeAreaRoot;
        private RectTransform havenPanel;
        private Text havenBodyText;
        private Text loadoutText;
        private Text stashText;
        private RectTransform resultPanel;
        private Text resultTitleText;
        private Text resultBodyText;

        private void Awake()
        {
            var canvas = gameObject.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;

            var scaler = gameObject.AddComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1920f, 1080f);
            scaler.matchWidthOrHeight = 0.5f;

            gameObject.AddComponent<GraphicRaycaster>();

            EnsureEventSystem();
            CreateSafeAreaRoot();
            CreateTopHud();
            CreateHavenPanel();
            CreateResultPanel();
            CreateMobileControls();
            RefreshHavenPanel();
            havenPanel.SetAsLastSibling();
        }

        private void Update()
        {
            if (session == null)
            {
                return;
            }

            RefreshTopHud();
            RefreshCooldowns();
        }

        public void Bind(ShardwakeSession newSession, InventoryComponent newInventory)
        {
            session = newSession;
            inventory = newInventory;
            playerSkills = newInventory != null ? newInventory.GetComponent<PlayerSkills>() : null;
            playerDash = newInventory != null ? newInventory.GetComponent<PlayerDash>() : null;
            playerHealth = newInventory != null ? newInventory.GetComponent<Health>() : null;
            RefreshHavenPanel();
        }

        public void BindInventoryPanel(GreyboxInventoryDebugPanel panel)
        {
            inventoryPanel = panel;
        }

        public void ShowResult(ExpeditionResult result)
        {
            resultPanel.gameObject.SetActive(true);
            resultPanel.SetAsLastSibling();
            resultTitleText.text = result.Survived ? "EXTRACTED" : "LOST TO THE SHARD";
            resultTitleText.color = result.Survived ? new Color(0.45f, 1f, 0.58f) : new Color(1f, 0.38f, 0.32f);
            resultBodyText.text = BuildResultBody(result);
        }


        private void CreateSafeAreaRoot()
        {
            var rootObject = new GameObject("Mobile Safe Area", typeof(RectTransform));
            rootObject.transform.SetParent(transform, false);

            safeAreaRoot = rootObject.GetComponent<RectTransform>();
            safeAreaRoot.anchorMin = Vector2.zero;
            safeAreaRoot.anchorMax = Vector2.one;
            safeAreaRoot.offsetMin = Vector2.zero;
            safeAreaRoot.offsetMax = Vector2.zero;
        }

        private void CreateTopHud()
        {
            var topPanel = CreatePanel("Top HUD", new Vector2(28f, -28f), new Vector2(0f, 1f), new Vector2(0f, 1f), new Vector2(540f, 286f), new Color(0.025f, 0.03f, 0.04f, 0.72f));
            topPanel.pivot = new Vector2(0f, 1f);

            phaseText = CreateChildText(topPanel, "Phase", new Vector2(20f, -26f), new Vector2(500f, 42f), TextAnchor.UpperLeft, 32, FontStyle.Bold);
            phaseText.rectTransform.anchorMin = new Vector2(0f, 1f);
            phaseText.rectTransform.anchorMax = new Vector2(0f, 1f);
            phaseText.rectTransform.pivot = new Vector2(0f, 1f);

            objectiveText = CreateChildText(topPanel, "Objective", new Vector2(20f, -70f), new Vector2(500f, 42f), TextAnchor.UpperLeft, 24, FontStyle.Normal);
            objectiveText.rectTransform.anchorMin = new Vector2(0f, 1f);
            objectiveText.rectTransform.anchorMax = new Vector2(0f, 1f);
            objectiveText.rectTransform.pivot = new Vector2(0f, 1f);

            lootText = CreateChildText(topPanel, "Loot", new Vector2(20f, -108f), new Vector2(500f, 42f), TextAnchor.UpperLeft, 24, FontStyle.Normal);
            lootText.rectTransform.anchorMin = new Vector2(0f, 1f);
            lootText.rectTransform.anchorMax = new Vector2(0f, 1f);
            lootText.rectTransform.pivot = new Vector2(0f, 1f);

            activeWeaponText = CreateChildText(topPanel, "Active Weapon", new Vector2(20f, -146f), new Vector2(500f, 42f), TextAnchor.UpperLeft, 24, FontStyle.Normal);
            activeWeaponText.rectTransform.anchorMin = new Vector2(0f, 1f);
            activeWeaponText.rectTransform.anchorMax = new Vector2(0f, 1f);
            activeWeaponText.rectTransform.pivot = new Vector2(0f, 1f);

            markerText = CreateChildText(topPanel, "Known Markers", new Vector2(20f, -184f), new Vector2(520f, 62f), TextAnchor.UpperLeft, 19, FontStyle.Normal);
            markerText.rectTransform.anchorMin = new Vector2(0f, 1f);
            markerText.rectTransform.anchorMax = new Vector2(0f, 1f);
            markerText.rectTransform.pivot = new Vector2(0f, 1f);

            var hpBack = CreatePanel("HP Back", new Vector2(20f, -236f), new Vector2(0f, 1f), new Vector2(0f, 1f), new Vector2(360f, 24f), new Color(0.18f, 0.04f, 0.04f, 0.92f));
            hpBack.SetParent(topPanel, false);
            hpBack.pivot = new Vector2(0f, 1f);

            var hpFillObject = new GameObject("HP Fill");
            hpFillObject.transform.SetParent(hpBack, false);
            var hpFillRect = hpFillObject.AddComponent<RectTransform>();
            hpFillRect.anchorMin = Vector2.zero;
            hpFillRect.anchorMax = Vector2.one;
            hpFillRect.offsetMin = Vector2.zero;
            hpFillRect.offsetMax = Vector2.zero;
            hpFill = hpFillObject.AddComponent<Image>();
            hpFill.color = new Color(0.85f, 0.12f, 0.12f, 0.95f);
            hpFill.type = Image.Type.Filled;
            hpFill.fillMethod = Image.FillMethod.Horizontal;
            hpFill.fillOrigin = 0;
            hpFill.fillAmount = 1f;

            hpText = CreateChildText(topPanel, "HP Text", new Vector2(394f, -240f), new Vector2(140f, 32f), TextAnchor.MiddleLeft, 22, FontStyle.Bold);
            hpText.rectTransform.anchorMin = new Vector2(0f, 1f);
            hpText.rectTransform.anchorMax = new Vector2(0f, 1f);
            hpText.rectTransform.pivot = new Vector2(0f, 0.5f);
        }

        private void RefreshTopHud()
        {
            if (!session.HasStarted)
            {
                phaseText.text = "HAVEN";
                objectiveText.text = "Prepare build and enter the Shard";
                lootText.text = $"Stash: {HavenStash.Count} items";
                activeWeaponText.text = "No active run";
                if (markerText != null)
                {
                    markerText.text = "Known markers: none";
                }
                SetHealthBar(1f, "READY");
                return;
            }

            var phaseName = session.IsInHellgate ? "HELLGATE" : "SHARD";
            phaseText.text = $"{phaseName}: {Mathf.CeilToInt(session.RemainingSeconds)}s";

            if (session.IsInHellgate)
            {
                objectiveText.text = session.IsHellExtractionOpen ? "Hell exits open — escape now" : "Survive until Hell exits open";
            }
            else if (session.IsHellgateQueued)
            {
                objectiveText.text = "Hellgate queued — survive until collapse";
            }
            else if (session.IsHellgateEntranceAvailable)
            {
                objectiveText.text = "Hellgate open — enter or extract";
            }
            else if (session.IsPortalUnlocked)
            {
                objectiveText.text = "Extraction portals active";
            }
            else
            {
                objectiveText.text = "Explore, loot, and survive";
            }

            var alert = session.IsShardAlertActive ? "  ALERT" : string.Empty;
            lootText.text = $"Loot: {inventory?.ItemCount ?? 0}  Kills: {session.EnemiesKilled}{alert}";
            activeWeaponText.text = $"Weapon: {WeaponDefinitions.Get(session.ActiveWeapon).DisplayName}";
            RefreshMarkerText();

            if (playerHealth != null)
            {
                SetHealthBar(playerHealth.Normalized, $"{Mathf.CeilToInt(playerHealth.CurrentHealth)}/{Mathf.CeilToInt(playerHealth.MaxHealth)}");
            }
        }

        private void RefreshMarkerText()
        {
            if (markerText == null || session == null || session.PlayerTransform == null)
            {
                return;
            }

            var markers = session.DiscoveredMapMarkers;
            if (markers.Count == 0)
            {
                markerText.text = "Known markers: none";
                return;
            }

            var builder = new StringBuilder();
            builder.Append("Known: ");
            var maxShown = Mathf.Min(3, markers.Count);
            for (var i = 0; i < maxShown; i++)
            {
                var marker = markers[i];
                if (i > 0)
                {
                    builder.Append("  |  ");
                }

                builder.Append(GetMarkerLabel(marker.Kind));
                builder.Append(session.IsMarkerOpen(marker.Kind) ? " OPEN" : " closed");
                builder.Append(' ');
                builder.Append(Mathf.RoundToInt(Vector3.Distance(session.PlayerTransform.position, marker.Position)));
                builder.Append('m');
            }

            if (markers.Count > maxShown)
            {
                builder.Append($"  +{markers.Count - maxShown}");
            }

            markerText.text = builder.ToString();
        }

        private static string GetMarkerLabel(MapMarkerKind kind)
        {
            return kind switch
            {
                MapMarkerKind.NormalExtraction => "Exit",
                MapMarkerKind.HellgateEntrance => "Hellgate",
                MapMarkerKind.HellExtraction => "Hell exit",
                _ => "Marker"
            };
        }

        private void SetHealthBar(float normalized, string label)
        {
            if (hpFill != null)
            {
                hpFill.fillAmount = Mathf.Clamp01(normalized);
            }

            if (hpText != null)
            {
                hpText.text = label;
            }
        }

        private void RefreshCooldowns()
        {
            if (playerSkills != null)
            {
                for (var i = 0; i < skillCooldownFills.Length; i++)
                {
                    if (skillCooldownFills[i] != null)
                    {
                        skillCooldownFills[i].fillAmount = playerSkills.GetSkillCooldownRatio(i);
                    }

                    if (skillLabels[i] != null)
                    {
                        var remaining = playerSkills.GetSkillCooldownRemaining(i);
                        skillLabels[i].text = remaining > 0.05f ? Mathf.CeilToInt(remaining).ToString() : $"S{i + 1}";
                    }
                }

                if (consumable1CooldownFill != null)
                {
                    consumable1CooldownFill.fillAmount = playerSkills.GetConsumableCooldownRatio(0);
                }

                if (consumable2CooldownFill != null)
                {
                    consumable2CooldownFill.fillAmount = playerSkills.GetConsumableCooldownRatio(1);
                }
            }

            if (dashCooldownFill != null && playerDash != null)
            {
                dashCooldownFill.fillAmount = playerDash.GetCooldownRatio();
            }

            if (swapCooldownFill != null && session != null)
            {
                swapCooldownFill.fillAmount = session.WeaponSwapCooldownRatio;
            }
        }

        private void CreateHavenPanel()
        {
            havenPanel = CreatePanel("Haven Panel", Vector2.zero, new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(1180f, 940f), new Color(0.035f, 0.045f, 0.06f, 0.94f));

            CreateChildText(havenPanel, "Title", new Vector2(0f, 392f), new Vector2(980f, 78f), TextAnchor.MiddleCenter, 50, FontStyle.Bold).text = "HAVEN";
            CreateHavenNavigationButtons(havenPanel);
            havenBodyText = CreateChildText(havenPanel, "Body", new Vector2(-300f, 156f), new Vector2(500f, 330f), TextAnchor.UpperLeft, 23, FontStyle.Normal);
            loadoutText = CreateChildText(havenPanel, "Loadout", new Vector2(300f, 156f), new Vector2(500f, 330f), TextAnchor.UpperLeft, 23, FontStyle.Normal);
            stashText = CreateChildText(havenPanel, "Stash", new Vector2(0f, -82f), new Vector2(920f, 160f), TextAnchor.UpperLeft, 21, FontStyle.Normal);
            CreateWeaponBuildButtons(havenPanel);
            CreateArmorPresetButtons(havenPanel);
            CreateStashButtons(havenPanel);
            CreateMerchantButtons(havenPanel);
            CreateTavernAndBoardButtons(havenPanel);
            CreateActionButton(havenPanel, "Enter Shard Button", "ENTER SHARD", new Vector2(0f, -410f), new Vector2(340f, 78f), new Color(0.32f, 0.62f, 1f, 0.92f), StartExpedition);
        }

        private void CreateResultPanel()
        {
            resultPanel = CreatePanel("Expedition Result", Vector2.zero, new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(880f, 900f), new Color(0.03f, 0.035f, 0.05f, 0.92f));
            resultPanel.gameObject.SetActive(false);

            resultTitleText = CreateChildText(resultPanel, "Title", new Vector2(0f, 342f), new Vector2(760f, 90f), TextAnchor.MiddleCenter, 52, FontStyle.Bold);
            resultBodyText = CreateChildText(resultPanel, "Body", new Vector2(0f, -38f), new Vector2(760f, 620f), TextAnchor.UpperLeft, 24, FontStyle.Normal);
            CreateActionButton(resultPanel, "Return Haven Button", "RETURN HAVEN", new Vector2(0f, -388f), new Vector2(340f, 78f), new Color(0.22f, 0.42f, 0.9f, 0.92f), RestartScene);
        }

        private static string BuildResultBody(ExpeditionResult result)
        {
            var builder = new StringBuilder();
            builder.AppendLine($"Outcome: {result.OutcomeText}");
            builder.AppendLine($"Loadout: {result.LoadoutName}");
            builder.AppendLine($"Enemies killed: {result.EnemiesKilled}");
            builder.AppendLine($"Mini-bosses: {result.MiniBossesDefeated}");
            builder.AppendLine($"Duration: {Mathf.CeilToInt(result.DurationSeconds)}s");
            builder.AppendLine(result.EnteredHellgate ? "Hellgate: entered" : "Hellgate: skipped");
            builder.AppendLine(result.ShardAlertTriggered ? $"Shard Alert: {result.AlertRelicName}" : "Shard Alert: none");
            builder.AppendLine();
            builder.AppendLine("Saved Loot");
            AppendLootList(builder, result.SavedLoot);
            builder.AppendLine();
            builder.AppendLine("Dropped Loot");
            AppendLootList(builder, result.DroppedLoot);
            builder.AppendLine();
            builder.AppendLine("Insurance Returned");
            AppendLootList(builder, result.InsuredReturned);
            builder.AppendLine();
            builder.AppendLine("Haven");
            builder.AppendLine($"Extracted relics: {result.Haven.ExtractedRelics}");
            builder.AppendLine($"Stored relic power: {result.Haven.TotalRelicPower}");
            builder.AppendLine($"Best relic: {result.Haven.BestRelicName} P{result.Haven.BestRelicPower}");
            builder.AppendLine($"Stash items: {HavenStash.Count}");
            return builder.ToString();
        }

        private static void AppendLootList(StringBuilder builder, System.Collections.Generic.IReadOnlyList<Shardwake.Loot.LootItem> loot)
        {
            if (loot == null || loot.Count == 0)
            {
                builder.AppendLine("- None");
                return;
            }

            for (var i = 0; i < loot.Count; i++)
            {
                var item = loot[i];
                builder.AppendLine($"- {item.DisplayName} [{item.Category}] P{item.Power}");
                builder.AppendLine($"  {item.GetStatLine()}");
            }
        }

        private void CreateMobileControls()
        {
            var joystick = CreatePanel("Move Joystick", new Vector2(148f, 148f), new Vector2(0f, 0f), new Vector2(0f, 0f), new Vector2(170f, 170f), new Color(1f, 1f, 1f, 0.16f));
            joystick.gameObject.AddComponent<VirtualJoystick>();

            dashCooldownFill = CreateCombatButton("Dash Button", "DASH", new Vector2(-310f, 148f), new Vector2(132f, 132f), new Color(0.24f, 0.56f, 0.95f, 0.78f), () => MobileInput.QueueDash());
            skillCooldownFills[0] = CreateCombatButton("Skill 1 Button", "S1", new Vector2(-164f, 218f), new Vector2(118f, 118f), new Color(0.6f, 0.34f, 0.94f, 0.78f), () => MobileInput.QueueSkill(0), out skillLabels[0]);
            skillCooldownFills[1] = CreateCombatButton("Skill 2 Button", "S2", new Vector2(-42f, 148f), new Vector2(118f, 118f), new Color(0.6f, 0.34f, 0.94f, 0.78f), () => MobileInput.QueueSkill(1), out skillLabels[1]);
            skillCooldownFills[2] = CreateCombatButton("Skill 3 Button", "S3", new Vector2(-164f, 78f), new Vector2(118f, 118f), new Color(0.6f, 0.34f, 0.94f, 0.78f), () => MobileInput.QueueSkill(2), out skillLabels[2]);
            swapCooldownFill = CreateCombatButton("Swap Weapon Button", "SWAP", new Vector2(-310f, 300f), new Vector2(120f, 96f), new Color(0.95f, 0.74f, 0.24f, 0.78f), () => MobileInput.QueueWeaponSwap());
            consumable1CooldownFill = CreateCombatButton("Consumable 1 Button", "C1", new Vector2(-52f, 294f), new Vector2(96f, 96f), new Color(0.24f, 0.72f, 0.46f, 0.78f), () => MobileInput.QueueConsumable(0));
            consumable2CooldownFill = CreateCombatButton("Consumable 2 Button", "C2", new Vector2(-52f, 34f), new Vector2(96f, 96f), new Color(0.82f, 0.45f, 0.18f, 0.78f), () => MobileInput.QueueConsumable(1));

            CreateCombatButton("Backpack Button", "BAG", new Vector2(-438f, 300f), new Vector2(112f, 88f), new Color(0.2f, 0.64f, 0.72f, 0.78f), () => inventoryPanel?.ToggleVisible());
            CreateCombatButton("Equip Button", "EQUIP", new Vector2(-438f, 204f), new Vector2(112f, 88f), new Color(0.36f, 0.74f, 0.34f, 0.78f), () => inventoryPanel?.EquipFirstEquipment());
        }

        private Image CreateCombatButton(string objectName, string label, Vector2 anchoredPosition, Vector2 size, Color color, UnityEngine.Events.UnityAction action)
        {
            return CreateCombatButton(objectName, label, anchoredPosition, size, color, action, out _);
        }

        private Image CreateCombatButton(string objectName, string label, Vector2 anchoredPosition, Vector2 size, Color color, UnityEngine.Events.UnityAction action, out Text buttonLabel)
        {
            var buttonRect = CreatePanel(objectName, anchoredPosition, new Vector2(1f, 0f), new Vector2(1f, 0f), size, color);
            buttonLabel = CreateButtonLabel(buttonRect, label);

            var button = buttonRect.gameObject.AddComponent<Button>();
            button.targetGraphic = buttonRect.GetComponent<Image>();
            button.onClick.AddListener(action);
            return CreateCooldownOverlay(buttonRect);
        }

        private Image CreateCooldownOverlay(RectTransform buttonRect)
        {
            var overlayObject = new GameObject("Cooldown Overlay");
            overlayObject.transform.SetParent(buttonRect, false);
            var rect = overlayObject.AddComponent<RectTransform>();
            rect.anchorMin = Vector2.zero;
            rect.anchorMax = Vector2.one;
            rect.offsetMin = Vector2.zero;
            rect.offsetMax = Vector2.zero;

            overlayObject.transform.SetAsFirstSibling();

            var image = overlayObject.AddComponent<Image>();
            image.color = new Color(0f, 0f, 0f, 0.56f);
            image.type = Image.Type.Filled;
            image.fillMethod = Image.FillMethod.Radial360;
            image.fillOrigin = 2;
            image.fillClockwise = false;
            image.fillAmount = 0f;
            image.raycastTarget = false;
            return image;
        }

        private void CreateHavenNavigationButtons(RectTransform parent)
        {
            CreateActionButton(parent, "Expedition Tab", "GATE", new Vector2(-400f, 302f), new Vector2(170f, 56f), new Color(0.22f, 0.42f, 0.82f, 0.9f), () => SetHavenView(HavenView.Expedition));
            CreateActionButton(parent, "Stash Tab", "STASH", new Vector2(-200f, 302f), new Vector2(170f, 56f), new Color(0.20f, 0.54f, 0.72f, 0.9f), () => SetHavenView(HavenView.Stash));
            CreateActionButton(parent, "Merchant Tab", "SHOP", new Vector2(0f, 302f), new Vector2(170f, 56f), new Color(0.68f, 0.52f, 0.24f, 0.9f), () => SetHavenView(HavenView.Merchant));
            CreateActionButton(parent, "Tavern Tab", "TAVERN", new Vector2(200f, 302f), new Vector2(170f, 56f), new Color(0.52f, 0.32f, 0.72f, 0.9f), () => SetHavenView(HavenView.Tavern));
            CreateActionButton(parent, "Board Tab", "BOARD", new Vector2(400f, 302f), new Vector2(170f, 56f), new Color(0.38f, 0.62f, 0.34f, 0.9f), () => SetHavenView(HavenView.JobBoard));
        }

        private void SetHavenView(HavenView view)
        {
            havenView = view;
            havenMessage = view switch
            {
                HavenView.Expedition => "Choose weapons, armor and start the next run.",
                HavenView.Stash => "Stored extraction loot. Equip items before leaving Haven.",
                HavenView.Merchant => "Trade low-value loot for Shard Dust or buy starter supplies.",
                HavenView.Tavern => "Rumors point Seekers toward danger and opportunity.",
                HavenView.JobBoard => "Contracts give simple goals for the next expedition.",
                _ => havenMessage
            };
            RefreshHavenPanel();
        }

        private void CreateWeaponBuildButtons(RectTransform parent)
        {
            CreateActionButton(parent, "Warbow Button", "GREATSWORD + BOW", new Vector2(-250f, -208f), new Vector2(230f, 64f), new Color(0.34f, 0.5f, 0.86f, 0.9f), () => SelectWeapons(WeaponType.GreatWeapon, WeaponType.Bow));
            CreateActionButton(parent, "Guard Mage Button", "SHIELD + MAGE", new Vector2(0f, -208f), new Vector2(230f, 64f), new Color(0.48f, 0.34f, 0.86f, 0.9f), () => SelectWeapons(WeaponType.SwordAndShield, WeaponType.MageStaff));
            CreateActionButton(parent, "Shadow Tome Button", "DAGGERS + TOME", new Vector2(250f, -208f), new Vector2(230f, 64f), new Color(0.25f, 0.62f, 0.5f, 0.9f), () => SelectWeapons(WeaponType.Daggers, WeaponType.NecromancerTome));
        }

        private void CreateArmorPresetButtons(RectTransform parent)
        {
            CreateActionButton(parent, "Light Armor Button", "LIGHT", new Vector2(-190f, -286f), new Vector2(170f, 58f), new Color(0.32f, 0.74f, 0.48f, 0.9f), () => SelectArmorPreset(ArmorType.Light));
            CreateActionButton(parent, "Medium Armor Button", "MEDIUM", new Vector2(0f, -286f), new Vector2(170f, 58f), new Color(0.72f, 0.62f, 0.28f, 0.9f), () => SelectArmorPreset(ArmorType.Medium));
            CreateActionButton(parent, "Heavy Armor Button", "HEAVY", new Vector2(190f, -286f), new Vector2(170f, 58f), new Color(0.78f, 0.34f, 0.28f, 0.9f), () => SelectArmorPreset(ArmorType.Heavy));
        }

        private void CreateStashButtons(RectTransform parent)
        {
            CreateActionButton(parent, "Equip Stash 1 Button", "EQUIP STASH 1", new Vector2(-220f, -354f), new Vector2(220f, 58f), new Color(0.28f, 0.56f, 0.78f, 0.9f), () => EquipStashItem(0));
            CreateActionButton(parent, "Equip Stash 2 Button", "EQUIP STASH 2", new Vector2(0f, -354f), new Vector2(220f, 58f), new Color(0.28f, 0.56f, 0.78f, 0.9f), () => EquipStashItem(1));
            CreateActionButton(parent, "Equip Stash 3 Button", "EQUIP STASH 3", new Vector2(220f, -354f), new Vector2(220f, 58f), new Color(0.28f, 0.56f, 0.78f, 0.9f), () => EquipStashItem(2));
        }

        private void CreateMerchantButtons(RectTransform parent)
        {
            CreateActionButton(parent, "Sell First Stash Button", "SELL STASH 1", new Vector2(-420f, -354f), new Vector2(180f, 58f), new Color(0.62f, 0.44f, 0.24f, 0.9f), SellFirstStashItem);
            CreateActionButton(parent, "Buy Flask Button", "BUY FLASK", new Vector2(420f, -354f), new Vector2(180f, 58f), new Color(0.24f, 0.62f, 0.46f, 0.9f), BuyStarterFlask);
        }

        private void CreateTavernAndBoardButtons(RectTransform parent)
        {
            CreateActionButton(parent, "Rumor Button", "RUMOR", new Vector2(-420f, -286f), new Vector2(180f, 58f), new Color(0.52f, 0.32f, 0.72f, 0.9f), GenerateTavernRumor);
            CreateActionButton(parent, "Contract Button", "TAKE JOB", new Vector2(420f, -286f), new Vector2(180f, 58f), new Color(0.38f, 0.62f, 0.34f, 0.9f), AcceptJobBoardContract);
        }

        private void SelectWeapons(WeaponType weapon1, WeaponType weapon2)
        {
            session?.SelectWeapons(weapon1, weapon2);
            RefreshHavenPanel();
        }

        private void SelectArmorPreset(ArmorType armorType)
        {
            session?.SelectArmorPreset(armorType);
            RefreshHavenPanel();
        }

        private void EquipStashItem(int stashIndex)
        {
            if (session != null && session.TryEquipStashItem(stashIndex))
            {
                RefreshHavenPanel();
            }
        }

        private void SellFirstStashItem()
        {
            if (!HavenStash.TryTakeAt(0, out var item))
            {
                havenMessage = "Merchant: your stash is empty.";
                RefreshHavenPanel();
                return;
            }

            var value = Mathf.Max(1, item.Power / 2);
            HavenProgress.AddShardDust(value);
            havenMessage = $"Merchant bought {item.DisplayName} for {value} Shard Dust.";
            RefreshHavenPanel();
        }

        private void BuyStarterFlask()
        {
            const int cost = 10;
            if (!HavenProgress.TrySpendShardDust(cost))
            {
                havenMessage = $"Merchant: need {cost} Shard Dust.";
                RefreshHavenPanel();
                return;
            }

            var flask = new LootItem("Healing Flask", ItemRarity.Common, 6, System.Array.Empty<LootAffix>(), "A cheap flask from Haven stores.", false, ItemSlotSize.Small, LootItemCategory.Consumable);
            HavenStash.Add(flask);
            havenMessage = "Bought Healing Flask and stored it in stash.";
            RefreshHavenPanel();
        }

        private void GenerateTavernRumor()
        {
            var rumors = new[]
            {
                "Tavern rumor: the next Hellgate may draw stronger Seekers than usual.",
                "Tavern rumor: mini-bosses often guard the best chest routes.",
                "Tavern rumor: if you find an inactive portal, remember its position.",
                "Tavern rumor: Swamp and Cemetery runs punish greedy players."
            };
            havenMessage = rumors[Random.Range(0, rumors.Length)];
            RefreshHavenPanel();
        }

        private void AcceptJobBoardContract()
        {
            HavenProgress.RecordContractCompleted();
            havenMessage = "Contract accepted: extract with any relic. Greybox reward: +15 Shard Dust.";
            RefreshHavenPanel();
        }

        private Button CreateActionButton(RectTransform parent, string objectName, string label, Vector2 anchoredPosition, Vector2 size, Color color, UnityEngine.Events.UnityAction onClick)
        {
            var buttonRect = CreatePanel(objectName, anchoredPosition, new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), size, color);
            buttonRect.SetParent(parent, false);
            CreateButtonLabel(buttonRect, label);

            var button = buttonRect.gameObject.AddComponent<Button>();
            button.targetGraphic = buttonRect.GetComponent<Image>();
            button.onClick.AddListener(onClick);
            return button;
        }

        private void StartExpedition()
        {
            if (session == null)
            {
                return;
            }

            session.StartExpedition();
            havenPanel.gameObject.SetActive(false);
        }

        private void RefreshHavenPanel()
        {
            if (havenBodyText == null || loadoutText == null)
            {
                return;
            }

            var progress = HavenProgress.Current;
            var weapon1 = WeaponDefinitions.Get(session != null ? session.SelectedWeapon1 : WeaponType.GreatWeapon);
            var weapon2 = WeaponDefinitions.Get(session != null ? session.SelectedWeapon2 : WeaponType.Bow);
            var activeWeapon = WeaponDefinitions.Get(session != null ? session.ActiveWeapon : WeaponType.GreatWeapon);
            var equipment = inventory != null ? inventory.GetComponent<EquipmentLoadout>() : null;
            var dust = HavenProgress.ShardDust;

            havenBodyText.text = havenView switch
            {
                HavenView.Expedition =>
                    "Expedition Gate\n" +
                    "Prepare, insure, and enter the Shard.\n\n" +
                    "Current objective:\n" +
                    "- Survive\n" +
                    "- Bring relics back\n" +
                    "- Decide whether to risk Hellgate\n\n" + havenMessage,
                HavenView.Stash =>
                    $"Stash\nStored items: {HavenStash.Count}\nShard Dust: {dust}\n\n" +
                    "Use EQUIP STASH 1–3 for quick greybox testing.\n" +
                    "Later this becomes the full mobile inventory screen.\n\n" + havenMessage,
                HavenView.Merchant =>
                    $"Merchant\nShard Dust: {dust}\n\n" +
                    "SELL STASH 1: sells first stored item.\n" +
                    "BUY FLASK: buys starter consumable for 10 dust.\n\n" + havenMessage,
                HavenView.Tavern =>
                    "Tavern\nRumors explain danger without AI for now.\n\n" +
                    "Later: run stories, city gossip, biome warnings.\n\n" + havenMessage,
                HavenView.JobBoard =>
                    $"Job Board\nCompleted contracts: {HavenProgress.ContractsCompleted}\n\n" +
                    "MVP contract: extract with any relic.\n" +
                    "Greybox button grants reward so economy can be tested.\n\n" + havenMessage,
                _ => havenMessage
            };

            var loadLine = equipment != null ? $"Load: {equipment.EquipLoad:0.0} ({equipment.LoadCategory})" : "Load: n/a";
            var stats = equipment != null ? equipment.Stats : Shardwake.Stats.CharacterStats.Baseline;

            loadoutText.text =
                $"Selected Build\n{weapon1.DisplayName}\n+ {weapon2.DisplayName}\n\n" +
                $"Active: {activeWeapon.DisplayName}\n" +
                $"{loadLine}\n" +
                $"Stats: STR {stats.Strength} DEX {stats.Dexterity} INT {stats.Intelligence}\n" +
                $"       AGI {stats.Agility} VIT {stats.Vitality} FOC {stats.Focus} STA {stats.Stamina}\n\n" +
                $"Haven\nRelics: {progress.ExtractedRelics}\nRelic Power: {progress.TotalRelicPower}\nShard Dust: {dust}\nBest: {progress.BestRelicName} P{progress.BestRelicPower}\n\n" +
                $"Role: {activeWeapon.Role}";

            if (stashText != null)
            {
                stashText.text = BuildStashPreview();
            }
        }

        private static string BuildStashPreview()
        {
            var builder = new StringBuilder();
            builder.AppendLine("Stash preview");

            if (HavenStash.Count == 0)
            {
                builder.AppendLine("- Empty. Extract loot to store it here.");
                return builder.ToString();
            }

            var count = Mathf.Min(5, HavenStash.Count);
            for (var i = 0; i < count; i++)
            {
                if (!HavenStash.TryGet(i, out var item))
                {
                    continue;
                }

                builder.AppendLine($"{i + 1}. {item.DisplayName} [{item.Category}] P{item.Power} — {item.GetStatLine()}");
            }

            if (HavenStash.Count > count)
            {
                builder.AppendLine($"+ {HavenStash.Count - count} more...");
            }

            return builder.ToString();
        }

        private static void RestartScene()
        {
            Time.timeScale = 1f;
            var activeScene = SceneManager.GetActiveScene();

            if (!string.IsNullOrEmpty(activeScene.name))
            {
                SceneManager.LoadScene(activeScene.name);
                return;
            }

            if (activeScene.buildIndex >= 0)
            {
                SceneManager.LoadScene(activeScene.buildIndex);
                return;
            }

            SceneManager.LoadScene(0);
        }

        private RectTransform CreatePanel(string panelName, Vector2 anchoredPosition, Vector2 anchorMin, Vector2 anchorMax, Vector2 size, Color color)
        {
            var panelObject = new GameObject(panelName);
            panelObject.transform.SetParent(safeAreaRoot != null ? safeAreaRoot : transform, false);

            var rect = panelObject.AddComponent<RectTransform>();
            rect.anchorMin = anchorMin;
            rect.anchorMax = anchorMax;
            rect.pivot = new Vector2(0.5f, 0.5f);
            rect.anchoredPosition = anchoredPosition;
            rect.sizeDelta = size;

            var image = panelObject.AddComponent<Image>();
            image.color = color;
            image.raycastTarget = true;
            return rect;
        }

        private Text CreateChildText(RectTransform parent, string textName, Vector2 anchoredPosition, Vector2 size, TextAnchor alignment, int fontSize, FontStyle fontStyle)
        {
            var textObject = new GameObject(textName);
            textObject.transform.SetParent(parent, false);

            var rect = textObject.AddComponent<RectTransform>();
            rect.anchorMin = new Vector2(0.5f, 0.5f);
            rect.anchorMax = new Vector2(0.5f, 0.5f);
            rect.pivot = new Vector2(0.5f, 0.5f);
            rect.anchoredPosition = anchoredPosition;
            rect.sizeDelta = size;

            var text = textObject.AddComponent<Text>();
            text.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            text.fontSize = fontSize;
            text.fontStyle = fontStyle;
            text.alignment = alignment;
            text.color = Color.white;
            text.raycastTarget = false;
            return text;
        }

        private Text CreateButtonLabel(RectTransform parent, string labelText)
        {
            var labelObject = new GameObject("Label");
            labelObject.transform.SetParent(parent, false);

            var rect = labelObject.AddComponent<RectTransform>();
            rect.anchorMin = Vector2.zero;
            rect.anchorMax = Vector2.one;
            rect.offsetMin = Vector2.zero;
            rect.offsetMax = Vector2.zero;

            var label = labelObject.AddComponent<Text>();
            label.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            label.fontSize = 34;
            label.fontStyle = FontStyle.Bold;
            label.alignment = TextAnchor.MiddleCenter;
            label.color = Color.white;
            label.raycastTarget = false;
            label.text = labelText;
            return label;
        }

        private static void EnsureEventSystem()
        {
            if (Object.FindFirstObjectByType<EventSystem>() != null)
            {
                return;
            }

            var eventSystemObject = new GameObject("EventSystem");
            eventSystemObject.AddComponent<EventSystem>();
            eventSystemObject.AddComponent<InputSystemUIInputModule>();
        }
    }
}
