namespace Shardwake.Insurance
{
    public static class InsuranceRules
    {
        public const int MaxInsuredItemsPerRun = 2;

        public static bool ReturnsToOwner(bool anotherPlayerExtractedWithItem)
        {
            return !anotherPlayerExtractedWithItem;
        }
    }
}
