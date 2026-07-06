namespace Shardwake.Equipment
{
    public static class EquipLoadCalculator
    {
        public static float GetArmorWeight(ArmorType head, ArmorType chest, ArmorType hands, ArmorType legs, ArmorType feet)
        {
            return ArmorWeights.GetWeight(EquipmentSlot.Head, head)
                + ArmorWeights.GetWeight(EquipmentSlot.Chest, chest)
                + ArmorWeights.GetWeight(EquipmentSlot.Hands, hands)
                + ArmorWeights.GetWeight(EquipmentSlot.Legs, legs)
                + ArmorWeights.GetWeight(EquipmentSlot.Feet, feet);
        }

        public static LoadCategory GetLoadCategory(float equipLoad)
        {
            if (equipLoad <= 12.9f)
            {
                return LoadCategory.Light;
            }

            if (equipLoad <= 22.9f)
            {
                return LoadCategory.Medium;
            }

            if (equipLoad <= 30f)
            {
                return LoadCategory.Heavy;
            }

            return LoadCategory.Overloaded;
        }
    }
}
