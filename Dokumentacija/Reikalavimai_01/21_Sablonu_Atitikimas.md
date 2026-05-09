# 21. Šablonų atitikimas (pattern matching) — 1 t.

## Reikalavimas

Naudojamas šablonų atitikimas.

## Implementacija

### Switch išraiška su tipo šablonais

**Failas:** `PokemonGame/Game.cs`

```csharp
// Rezultato žinutė
_statusMessage = result switch
{
    BattleResult.PlayerWon     => $"Nugalėjote {wild.Name}!",
    BattleResult.PlayerFled    => "Pabėgote iš kovos.",
    BattleResult.PlayerLost    => "Visi Pokemon krito. Atsigavote.",
    BattleResult.PokemonCaught => $"Pagavote {wild.Name}!",
    _                          => "",
};
```

### Switch su when sąlyga

```csharp
_statusMessage = tile switch
{
    TileType.TallGrass when _settings.EncounterChance >= 50 => "...",
    TileType.TallGrass when _settings.EncounterChance >= 25 => "...",
    TileType.TallGrass                                       => "...",
    TileType.Sand                                            => "Smėlio takas.",
    _                                                        => "",
};
```

### Switch su `is` šablonu

```csharp
// Pokemon.cs
public int CompareTo(Pokemon? other) => other is null ? 1 : Level.CompareTo(other.Level);

// Move.cs
public bool Equals(Move? other) => other is not null && Name == other.Name && ...;
```

### Switch teiginys su arba šablonu (`or`)

```csharp
// Game.cs / Battle.cs
case ConsoleKey.W or ConsoleKey.UpArrow:   dy = -1; break;
case ConsoleKey.S or ConsoleKey.DownArrow: dy = +1; break;
```

## Nauda

Šablonų atitikimas leidžia išreikšti sudėtingą sąlygų logiką aiškiau nei grandinės `if-else if`. Switch išraiška (ne teiginys) grąžina reikšmę ir kompiliatorius tikrina ar visi atvejai padengti.
