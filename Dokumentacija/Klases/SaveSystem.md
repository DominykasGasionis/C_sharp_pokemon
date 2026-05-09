# SaveSystem, SaveData, PokemonSaveEntry

**Projektas:** `PokemonGame`
**Failas:** `SaveSystem.cs`

---

## PokemonSaveEntry

Duomenų perdavimo objektas (DTO) – vieno Pokemon duomenys JSON faile.

```csharp
public class PokemonSaveEntry
{
    public string Name       { get; set; } = "";
    public int    MaxHp      { get; set; }
    public int    Hp         { get; set; }
    public int    Attack     { get; set; }
    public int    Defense    { get; set; }
    public int    Level      { get; set; } = 5;
    public int    Experience { get; set; } = 0;
}
```

**Pastaba:** `XpReward` ir `Moves` **nesaugomi** – jie atkuriami iš `Pokemon.SpeciesMoves` automatiškai kuriant `Pokemon` objektą.

---

## SaveData

Visa žaidimo būsena viename JSON objekte.

```csharp
public class SaveData
{
    public List<PokemonSaveEntry> AllPokemon   { get; set; } = new();
    public int[]                  PartyIndices { get; set; } = new[] { 0, -1, -1 };
    public int PlayerX          { get; set; }
    public int PlayerY          { get; set; }
    public int EncounterChance  { get; set; } = 25;
    public int Pokeballs        { get; set; } = 5;
    public int Potions          { get; set; } = 3;
}
```

| Laukas | Aprašas |
|---|---|
| `AllPokemon` | Visi žaidėjo Pokemon (partija + dėžė) |
| `PartyIndices` | Indeksai į `AllPokemon` sąrašą (kuri vieta – kuris Pokemon); `-1` = tuščias lizas |
| `PlayerX`, `PlayerY` | Žaidėjo pozicija žemėlapyje |
| `EncounterChance` | Pokemon susidūrimų dažnis (%) |
| `Pokeballs`, `Potions` | Inventoriaus kiekiai |

---

## SaveSystem (statinė klasė)

Valdo išsaugojimo failo kūrimą, skaitymą ir trynimą.

### Išsaugojimo vieta

```csharp
private static readonly string SavePath = Path.Combine(
    Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
    ".pokemongame", "save.json");
// Pavyzdys Linux: ~/.pokemongame/save.json
// Pavyzdys Windows: C:\Users\vardas\.pokemongame\save.json
```

### Metodai

| Metodas | Grąžina | Aprašas |
|---|---|---|
| `SaveExists()` | `bool` | Tikrina ar `save.json` egzistuoja |
| `Save(roster, player, settings, inventory)` | `void` | Serializuoja žaidimo būseną į JSON (su `WriteIndented = true`) |
| `Load()` | `SaveData?` | Deserializuoja JSON; jei klaida – grąžina `null` |
| `RosterFromSave(SaveData)` | `PokemonRoster` | Atkuria `PokemonRoster` iš `SaveData` duomenų |
| `Delete()` | `void` | Ištrina išsaugojimo failą (naudojama prieš naują žaidimą) |

### `Save` logika

```csharp
// Sukuria direktoriją jei neegzistuoja
Directory.CreateDirectory(Path.GetDirectoryName(SavePath)!);

// Serializuoja su prettify
File.WriteAllText(SavePath, JsonSerializer.Serialize(data,
    new JsonSerializerOptions { WriteIndented = true }));
```

### `RosterFromSave` logika

```csharp
public static PokemonRoster RosterFromSave(SaveData save)
{
    var all = save.AllPokemon
        .Select(e => new Pokemon(e.Name, e.MaxHp, e.Attack, e.Defense,
                                 e.Hp, e.Level, e.Experience))
        .ToList();
    return new PokemonRoster(all, save.PartyIndices);
}
```

Kiekvienas `PokemonSaveEntry` konvertuojamas į `Pokemon` objektą su išsaugotomis statistikomis.
