using Microsoft.EntityFrameworkCore;

namespace PokemonGame;

// EF Core lentelės modelis – saugo vieno Pokemon duomenis duomenų bazėje
public class SavedPokemonEntity
{
    public int    Id         { get; set; } // pirminis raktas (automatiškai generuojamas)
    public string Name       { get; set; } = "";
    public int    MaxHp      { get; set; }
    public int    Hp         { get; set; }
    public int    Attack     { get; set; }
    public int    Defense    { get; set; }
    public int    Level      { get; set; }
    public int    Experience { get; set; }
    public int    XpReward   { get; set; }
    public int    SortOrder  { get; set; } // naudojamas tvarkai atkurti po load
}

// EF Core lentelės modelis – saugo žaidimo būseną (viena eilutė = vienas išsaugojimas)
public class SavedGameState
{
    public int    Id              { get; set; }
    public int    PlayerX         { get; set; }
    public int    PlayerY         { get; set; }
    public int    EncounterChance { get; set; }
    public int    Pokeballs       { get; set; }
    public int    Potions         { get; set; }
    public string PartyIndices    { get; set; } = "0,-1,-1"; // indeksai atskiri kableliais
}

// Entity Framework DbContext – jungiasi prie SQLite duomenų bazės ~/.pokemongame/save.db
public class GameDbContext : DbContext
{
    private static readonly string DbPath = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
        ".pokemongame", "save.db");

    // Lentelės duomenų bazėje
    public DbSet<SavedPokemonEntity> Pokemon    { get; set; } = null!;
    public DbSet<SavedGameState>     GameStates { get; set; } = null!;

    // Nurodo EF Core naudoti SQLite su failo keliu
    protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlite($"Data Source={DbPath}");
}
