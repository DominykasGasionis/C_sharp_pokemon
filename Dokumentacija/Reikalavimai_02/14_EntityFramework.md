# 14. Duomenų bazė ir Entity Framework — 3 t.

## Reikalavimas

Naudojate savo projekte duomenų bazę ir Entity Framework.

## Naudojamos bibliotekos

- `Microsoft.EntityFrameworkCore.Sqlite` (v8.0.0)
- `Microsoft.EntityFrameworkCore.Design` (v8.0.0)
- Duomenų bazė: **SQLite** (`~/.pokemongame/save.db`)

## Duomenų modeliai

**Failas:** `PokemonGame/GameDbContext.cs`

### SavedPokemonEntity — Pokemon lentelė

```csharp
public class SavedPokemonEntity
{
    public int    Id         { get; set; } // pirminis raktas
    public string Name       { get; set; } = "";
    public int    MaxHp      { get; set; }
    public int    Hp         { get; set; }
    public int    Attack     { get; set; }
    public int    Defense    { get; set; }
    public int    Level      { get; set; }
    public int    Experience { get; set; }
    public int    XpReward   { get; set; }
    public int    SortOrder  { get; set; } // išsaugo originalią tvarką
}
```

### SavedGameState — žaidimo būsenos lentelė

```csharp
public class SavedGameState
{
    public int    Id              { get; set; }
    public int    PlayerX         { get; set; }
    public int    PlayerY         { get; set; }
    public int    EncounterChance { get; set; }
    public int    Pokeballs       { get; set; }
    public int    Potions         { get; set; }
    public string PartyIndices    { get; set; } = "0,-1,-1";
}
```

## DbContext

**Failas:** `PokemonGame/GameDbContext.cs`

```csharp
// Entity Framework DbContext – jungiasi prie SQLite duomenų bazės
public class GameDbContext : DbContext
{
    private static readonly string DbPath = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
        ".pokemongame", "save.db");

    public DbSet<SavedPokemonEntity> Pokemon    { get; set; } = null!;
    public DbSet<SavedGameState>     GameStates { get; set; } = null!;

    protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlite($"Data Source={DbPath}");
}
```

## SaveSystem — išsaugojimas ir įkėlimas

**Failas:** `PokemonGame/SaveSystem.cs`

### Išsaugojimas

```csharp
public static void Save(PokemonRoster roster, Player player, GameSettings settings, Inventory inventory)
{
    EnsureDb();
    using var db = new GameDbContext();

    db.Pokemon.RemoveRange(db.Pokemon);
    db.GameStates.RemoveRange(db.GameStates);
    db.SaveChanges();

    for (int i = 0; i < roster.All.Count; i++)
    {
        var p = roster.All[i];
        db.Pokemon.Add(new SavedPokemonEntity { Name = p.Name, /* ... */ SortOrder = i });
    }

    db.GameStates.Add(new SavedGameState { PlayerX = player.X, /* ... */ });
    db.SaveChanges();
}
```

### Įkėlimas (LINQ užklausa)

```csharp
var all = db.Pokemon
    .OrderBy(p => p.SortOrder)
    .Select(e => new Pokemon(e.Name, e.MaxHp, e.Attack, e.Defense,
                             e.Hp, e.Level, e.Experience, e.XpReward))
    .ToList();
```

## Duomenų bazės vieta

```
~/.pokemongame/save.db
```

Lentelės sukuriamos automatiškai paleidus žaidimą pirmą kartą (`EnsureCreated()`).
