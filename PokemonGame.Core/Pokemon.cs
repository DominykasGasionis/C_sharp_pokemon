namespace PokemonGame;

[Flags]
public enum StatusEffect
{
    None      = 0,
    Poisoned  = 1 << 0, // 1
    Paralyzed = 1 << 1, // 2
}

public class Pokemon : Entity, IComparable<Pokemon>, IFormattable, IHealable
{
    public string Name { get; }
    public int MaxHp { get; private set; }
    public int Hp { get; private set; }
    public int Attack { get; private set; }
    public int Defense { get; private set; }
    public int Level { get; private set; }
    public int Experience { get; private set; }
    public int XpReward { get; }
    public IReadOnlyList<Move> Moves { get; }

    public StatusEffect Status { get; private set; } = StatusEffect.None;

    public bool IsAlive => Hp > 0;
    public int ExperienceToNextLevel => Level * 50;

    public void ApplyStatus(StatusEffect effect)  => Status |= effect;
    public void ClearStatus(StatusEffect effect)   => Status &= ~effect;
    public bool HasStatus(StatusEffect effect)     => (Status & effect) != 0;

    public Pokemon(string name, int maxHp, int attack, int defense, int? currentHp = null, int level = 5, int experience = 0, int xpReward = 0)
    {
        Name       = name;
        MaxHp      = maxHp;
        Hp         = currentHp ?? maxHp;
        Attack     = attack;
        Defense    = defense;
        Level      = level;
        Experience = experience;
        XpReward   = xpReward;

        Moves = SpeciesMoves.TryGetValue(name, out var moves)
            ? moves
            : new List<Move> { new Move("Tackle", 40) };
    }

    public static readonly Dictionary<string, List<Move>> SpeciesMoves = new()
    {
        ["Bulbasaur"]  = new() { new("Tackle", 40), new("Vine Whip", 45), new("Razor Leaf", 55), new("Solar Beam", 120) },
        ["Charmander"] = new() { new("Scratch", 40), new("Ember", 40), new("Fire Fang", 65), new("Flamethrower", 90) },
        ["Squirtle"]   = new() { new("Tackle", 40), new("Water Gun", 40), new("Bubble Beam", 65), new("Hydro Pump", 110) },
        ["Pikachu"]    = new() { new("Tackle", 40), new("Thunder Shock", 40), new("Quick Attack", 40), new("Thunderbolt", 90) },
        ["Eevee"]      = new() { new("Tackle", 40), new("Quick Attack", 40), new("Swift", 60), new("Last Resort", 140) },
        ["Gulpin"]     = new() { new("Pound", 40), new("Acid", 40), new("Sludge", 65), new("Sludge Bomb", 90) },
        ["Swalot"]     = new() { new("Pound", 40), new("Body Slam", 85), new("Sludge Bomb", 90), new("Gunk Shot", 120) },
        ["Slugma"]     = new() { new("Tackle", 40), new("Ember", 40), new("Rock Throw", 50), new("Flamethrower", 90) },
        ["Rattata"]    = new() { new("Tackle", 40), new("Quick Attack", 40), new("Bite", 60) },
        ["Pidgey"]     = new() { new("Tackle", 40), new("Gust", 40), new("Quick Attack", 40) },
        ["Caterpie"]   = new() { new("Tackle", 40), new("Bug Bite", 60) },
        ["Weedle"]     = new() { new("Poison Sting", 15), new("Bug Bite", 60) },
    };

    // Starteriai kuriuos galima pasirinkti
    public static readonly (string Name, int Hp, int Atk, int Def)[] StarterPool = new[]
    {
        ("Bulbasaur",  45, 49, 49),
        ("Charmander", 39, 52, 43),
        ("Squirtle",   44, 48, 65),
        ("Pikachu",    35, 55, 40),
        ("Eevee",      55, 45, 45),
        ("Gulpin",     40, 45, 35),
        ("Swalot",    100, 73, 83),
        ("Slugma",     70, 80, 50),
    };

    public override string GetDisplayName() => $"{Name} Lv{Level}";

    public int CompareTo(Pokemon? other) => other is null ? 1 : Level.CompareTo(other.Level);

    public string ToString(string? format, IFormatProvider? _ = null) => format switch
    {
        "S" => $"{Name} Lv{Level}",
        "L" => $"{Name} Lv{Level} HP:{Hp}/{MaxHp}{(Status != StatusEffect.None ? $" [{Status}]" : "")}",
        _   => Name,
    };

    public void Deconstruct(out string name, out int hp, out int maxHp)
    {
        name  = Name;
        hp    = Hp;
        maxHp = MaxHp;
    }

    public void HealFull() => Hp = MaxHp;

    public int Heal(int amount)
    {
        int actual = Math.Min(amount, MaxHp - Hp);
        Hp += actual;
        return actual;
    }

    public int TakeDamage(int rawDamage)
    {
        int damage = Math.Max(1, rawDamage - Defense);
        Hp = Math.Max(0, Hp - damage);
        return damage;
    }

    public string HpBar(int barWidth = 20)
    {
        int filled = (int)Math.Round((double)Hp / MaxHp * barWidth);
        return "[" + new string('█', filled) + new string('░', barWidth - filled) + "]";
    }

    /// <summary>
    /// Grants XP. Returns a level-up message if leveled up, otherwise null.
    /// </summary>
    public string? GainExperience(int amount)
    {
        Experience += amount;
        string? lastMsg = null;

        while (Experience >= ExperienceToNextLevel)
        {
            Experience -= ExperienceToNextLevel;
            Level++;
            MaxHp  += 5;
            Hp      = Math.Min(Hp + 5, MaxHp);
            Attack  += 2;
            Defense += 2;
            lastMsg = $"{Name} pasiekė {Level} lygį! ATK:{Attack} DEF:{Defense} HP:{MaxHp}";
        }

        return lastMsg;
    }

    // Laukiniai Pokemon kurie gali pasirodyti
    private static readonly (string Name, int Hp, int Atk, int Def, int XpReward)[] WildPool = new[]
    {
        ("Bulbasaur",  45, 49, 49, 64),
        ("Charmander", 39, 52, 43, 64),
        ("Squirtle",   44, 48, 65, 64),
        ("Pikachu",    35, 55, 40, 64),
        ("Rattata",    30, 56, 35, 50),
        ("Pidgey",     40, 45, 40, 55),
        ("Caterpie",   45, 30, 35, 39),
        ("Weedle",     35, 35, 30, 41),
    };

    public static Pokemon RandomWild(Random rng)
    {
        var (name, hp, atk, def, xpReward) = WildPool[rng.Next(WildPool.Length)];
        int level     = rng.Next(2, 9); // 2–8
        int scaledHp  = Math.Max(10, hp  + (level - 5) * 3);
        int scaledAtk = Math.Max(5,  atk + (level - 5) * 2);
        int scaledDef = Math.Max(5,  def + (level - 5) * 2);
        return new Pokemon(name, scaledHp, scaledAtk, scaledDef, xpReward: xpReward, level: level);
    }
}
