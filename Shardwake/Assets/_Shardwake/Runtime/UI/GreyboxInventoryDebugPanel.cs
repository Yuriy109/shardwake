using System.Text;
using Shardwake.Equipment;
using Shardwake.Inventory;
using Shardwake.Loot;
using Shardwake.Stats;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Shardwake.UI
{
    /// <summary>
    /// Temporary greybox panel for testing backpack, equipment, stats and armor load.
    /// This is not final UI. It exists so the core loot/equipment loop can be tested quickly.
    /// </summary>
    public sealed class GreyboxInventoryDebugPanel : MonoBehaviour
    {
        private InventoryComponent inventory;
        private EquipmentLoadout equipment;
        private Text panelText;
        private GameObject panelRoot;
        private bool visible = false;

        public void Bind(InventoryComponent newInventory, EquipmentLoadout newEquipment)
        {
            inventory = newInventory;
            equipment = newEquipment;
            Refresh();
        }

        private void Awake()
        {
            var panelObject = new GameObject("Inventory Debug Panel");
            panelRoot = panelObject;
            panelObject.transform.SetParent(transform, false);

            var rect = panelObject.AddComponent<RectTransform>();
            rect.anchorMin = new Vector2(1f, 1f);
            rect.anchorMax = new Vector2(1f, 1f);
            rect.pivot = new Vector2(1f, 1f);
            rect.anchoredPosition = new Vector2(-28f, -28f);
            rect.sizeDelta = new Vector2(500f, 620f);

            var image = panelObject.AddComponent<Image>();
            image.color = new Color(0.025f, 0.03f, 0.04f, 0.86f);

            var textObject = new GameObject("Inventory Debug Text");
            textObject.transform.SetParent(panelObject.transform, false);

            var textRect = textObject.AddComponent<RectTransform>();
            textRect.anchorMin = Vector2.zero;
            textRect.anchorMax = Vector2.one;
            textRect.offsetMin = new Vector2(18f, 16f);
            textRect.offsetMax = new Vector2(-18f, -16f);

            panelText = textObject.AddComponent<Text>();
            panelText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            panelText.fontSize = 22;
            panelText.alignment = TextAnchor.UpperLeft;
            panelText.color = Color.white;
            panelText.raycastTarget = false;
            panelRoot.SetActive(visible);
        }

        private void Update()
        {
            var keyboard = Keyboard.current;
            if (keyboard != null)
            {
                if (keyboard.tabKey.wasPressedThisFrame)
                {
                    ToggleVisible();
                }

                if (keyboard.eKey.wasPressedThisFrame)
                {
                    EquipFirstEquipment();
                }
            }

            if (visible)
            {
                Refresh();
            }
        }


        public void ToggleVisible()
        {
            visible = !visible;
            if (panelRoot != null)
            {
                panelRoot.SetActive(visible);
            }
        }

        public void EquipFirstEquipment()
        {
            if (inventory != null && equipment != null)
            {
                inventory.TryEquipFirstEquipment(equipment);
                Refresh();
            }
        }

        private void Refresh()
        {
            if (panelText == null)
            {
                return;
            }

            if (inventory == null || equipment == null)
            {
                panelText.text = "Inventory debug panel not bound.";
                return;
            }

            var stats = equipment.Stats;
            var builder = new StringBuilder();
            builder.AppendLine("BACKPACK / EQUIPMENT");
            builder.AppendLine("BAG: toggle  |  EQUIP: equip first equipment");
            builder.AppendLine();
            builder.AppendLine($"Backpack: {inventory.UsedSlots}/{inventory.MaxSlots} slots");
            builder.AppendLine($"Load: {equipment.EquipLoad:0.0} ({equipment.LoadCategory})");
            builder.AppendLine($"Stats: STR {stats.Strength}  DEX {stats.Dexterity}  INT {stats.Intelligence}");
            builder.AppendLine($"       AGI {stats.Agility}  VIT {stats.Vitality}  FOC {stats.Focus}  STA {stats.Stamina}");
            builder.AppendLine();

            builder.AppendLine("Equipped:");
            AppendEquipped(builder, EquipmentSlot.Weapon1);
            AppendEquipped(builder, EquipmentSlot.Weapon2);
            AppendEquipped(builder, EquipmentSlot.Head);
            AppendEquipped(builder, EquipmentSlot.Chest);
            AppendEquipped(builder, EquipmentSlot.Hands);
            AppendEquipped(builder, EquipmentSlot.Legs);
            AppendEquipped(builder, EquipmentSlot.Feet);
            AppendEquipped(builder, EquipmentSlot.Ring1);
            AppendEquipped(builder, EquipmentSlot.Ring2);
            AppendEquipped(builder, EquipmentSlot.Amulet);

            builder.AppendLine();
            builder.AppendLine("Backpack:");
            var items = inventory.Items;
            if (items.Count == 0)
            {
                builder.AppendLine("- empty");
            }
            else
            {
                var max = Mathf.Min(items.Count, 8);
                for (var i = 0; i < max; i++)
                {
                    var item = items[i];
                    builder.AppendLine($"{i + 1}. {ShortItem(item)}");
                }

                if (items.Count > max)
                {
                    builder.AppendLine($"+ {items.Count - max} more...");
                }
            }

            panelText.text = builder.ToString();
        }

        private void AppendEquipped(StringBuilder builder, EquipmentSlot slot)
        {
            if (equipment.TryGetEquipped(slot, out var item))
            {
                builder.AppendLine($"- {slot}: {ShortItem(item)}");
            }
            else
            {
                builder.AppendLine($"- {slot}: empty");
            }
        }

        private static string ShortItem(LootItem item)
        {
            return $"{item.Rarity} {item.Category} {item.Name} [{item.GetStatLine()}]";
        }
    }
}
