namespace Shardwake.Equipment
{
    public static class ArmorWeights
    {
        public static float GetWeight(EquipmentSlot slot, ArmorType armorType)
        {
            return slot switch
            {
                EquipmentSlot.Head => armorType switch { ArmorType.Light => 1.5f, ArmorType.Medium => 2.6f, _ => 4.7f },
                EquipmentSlot.Chest => armorType switch { ArmorType.Light => 3.5f, ArmorType.Medium => 6.2f, _ => 11.0f },
                EquipmentSlot.Hands => armorType switch { ArmorType.Light => 1.5f, ArmorType.Medium => 2.6f, _ => 4.7f },
                EquipmentSlot.Legs => armorType switch { ArmorType.Light => 2.0f, ArmorType.Medium => 3.5f, _ => 6.3f },
                EquipmentSlot.Feet => armorType switch { ArmorType.Light => 1.5f, ArmorType.Medium => 2.6f, _ => 4.7f },
                _ => 0f
            };
        }
    }
}
