# 13. System.Collections.Generic — 1 t.

## Reikalavimas

Naudojamos duomenų struktūros iš `System.Collections` arba `System.Collections.Generic`.

## Implementacija

Kode naudojamos kelios generinės kolekcijos:

### `List<T>`

```csharp
// PokemonRoster.cs
public List<Pokemon> All { get; } = new();

// Battle.cs
private readonly List<string> _log = new();
```

### `Dictionary<TKey, TValue>`

```csharp
// Pokemon.cs
public static readonly Dictionary<string, List<Move>> SpeciesMoves = new() { ... };

// Map.cs (statiniame konstruktoriuje)
TileChars  = new Dictionary<TileType, char> { ... };
TileColors = new Dictionary<TileType, (ConsoleColor, ConsoleColor)> { ... };
```

### `IReadOnlyList<T>`

```csharp
// Pokemon.cs
public IReadOnlyList<Move> Moves { get; }
```

## Nauda

Generinės kolekcijos suteikia tipo saugumą kompiliavimo metu – negalima atsitiktinai pridėti netinkamo tipo elemento. `Dictionary` leidžia O(1) paiešką pagal raktą, kas naudojama greitam plytelės simbolio ir spalvos radimui žemėlapyje.
