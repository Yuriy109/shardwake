namespace Shardwake.Weapons
{
    public readonly struct WeaponPassiveDefinition
    {
        public WeaponPassiveDefinition(string id, string displayName, string description)
        {
            Id = id;
            DisplayName = displayName;
            Description = description;
        }

        public string Id { get; }
        public string DisplayName { get; }
        public string Description { get; }
    }
}
