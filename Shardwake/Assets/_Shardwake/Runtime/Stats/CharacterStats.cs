namespace Shardwake.Stats
{
    public readonly struct CharacterStats
    {
        public CharacterStats(int strength, int dexterity, int intelligence, int agility, int vitality, int focus, int stamina)
        {
            Strength = strength;
            Dexterity = dexterity;
            Intelligence = intelligence;
            Agility = agility;
            Vitality = vitality;
            Focus = focus;
            Stamina = stamina;
        }

        public int Strength { get; }
        public int Dexterity { get; }
        public int Intelligence { get; }
        public int Agility { get; }
        public int Vitality { get; }
        public int Focus { get; }
        public int Stamina { get; }

        public int Get(CoreStat stat)
        {
            return stat switch
            {
                CoreStat.Strength => Strength,
                CoreStat.Dexterity => Dexterity,
                CoreStat.Intelligence => Intelligence,
                CoreStat.Agility => Agility,
                CoreStat.Vitality => Vitality,
                CoreStat.Focus => Focus,
                CoreStat.Stamina => Stamina,
                _ => 0
            };
        }

        public CharacterStats Add(CoreStat stat, int value)
        {
            return stat switch
            {
                CoreStat.Strength => new CharacterStats(Strength + value, Dexterity, Intelligence, Agility, Vitality, Focus, Stamina),
                CoreStat.Dexterity => new CharacterStats(Strength, Dexterity + value, Intelligence, Agility, Vitality, Focus, Stamina),
                CoreStat.Intelligence => new CharacterStats(Strength, Dexterity, Intelligence + value, Agility, Vitality, Focus, Stamina),
                CoreStat.Agility => new CharacterStats(Strength, Dexterity, Intelligence, Agility + value, Vitality, Focus, Stamina),
                CoreStat.Vitality => new CharacterStats(Strength, Dexterity, Intelligence, Agility, Vitality + value, Focus, Stamina),
                CoreStat.Focus => new CharacterStats(Strength, Dexterity, Intelligence, Agility, Vitality, Focus + value, Stamina),
                CoreStat.Stamina => new CharacterStats(Strength, Dexterity, Intelligence, Agility, Vitality, Focus, Stamina + value),
                _ => this
            };
        }

        public CharacterStats Add(CharacterStats other)
        {
            return new CharacterStats(
                Strength + other.Strength,
                Dexterity + other.Dexterity,
                Intelligence + other.Intelligence,
                Agility + other.Agility,
                Vitality + other.Vitality,
                Focus + other.Focus,
                Stamina + other.Stamina);
        }

        public static CharacterStats Baseline => new CharacterStats(0, 0, 0, 0, 0, 0, 0);
    }
}
