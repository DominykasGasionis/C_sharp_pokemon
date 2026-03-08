namespace PokemonGame;

public class Pokemon
{
    public string Name { get; }
    public int MaxHp { get; }
    public int Hp { get; private set; }
    public int Attack { get; }
    public int Defense { get; }

    public bool IsAlive => Hp > 0;

    public Pokemon(string name, int maxHp, int attack, int defense, int? currentHp = null)
    {
        Name = name;
        MaxHp = maxHp;
        Hp = currentHp ?? maxHp;
        Attack = attack;
        Defense = defense;
    }

    public static Pokemon FromSave(SaveData save) =>
        new(save.PokemonName, save.PokemonMaxHp, save.PokemonAttack, save.PokemonDefense, save.PokemonHp);

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


    public void HealFull() => Hp = MaxHp;

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

    // Laukiniai Pokemon kurie gali pasirodyti
    private static readonly (string Name, int Hp, int Atk, int Def)[] WildPool = new[]
    {
        ("Bulbasaur",  45, 49, 49),
        ("Charmander", 39, 52, 43),
        ("Squirtle",   44, 48, 65),
        ("Pikachu",    35, 55, 40),
        ("Rattata",    30, 56, 35),
        ("Pidgey",     40, 45, 40),
        ("Caterpie",   45, 30, 35),
        ("Weedle",     35, 35, 30),
    };

    public static Pokemon RandomWild(Random rng)
    {
        var (name, hp, atk, def) = WildPool[rng.Next(WildPool.Length)];
        return new Pokemon(name, hp, atk, def);
    }
}
