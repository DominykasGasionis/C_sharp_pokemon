# 13. LINQ — 1 t.

## Reikalavimas

Naudojate LINQ.

## Implementacija

LINQ naudojamas keliose vietose projekte.

### Game.cs — party būsenos tikrinimas

**Failas:** `PokemonGame/Game.cs`

```csharp
bool allHealthy = _roster.Party
    .Where(p => p != null)
    .All(p => p!.Hp == p.MaxHp);
```

### SaveSystem.cs — Pokemon įkėlimas iš duomenų bazės

**Failas:** `PokemonGame/SaveSystem.cs`

```csharp
var all = db.Pokemon
    .OrderBy(p => p.SortOrder)
    .Select(e => new Pokemon(e.Name, e.MaxHp, e.Attack, e.Defense,
                             e.Hp, e.Level, e.Experience, e.XpReward))
    .ToList();
```

```csharp
int[] indices = state.PartyIndices
    .Split(',')
    .Select(int.Parse)
    .ToArray();
```

### PokemonRoster.cs — party indeksų apskaičiavimas

**Failas:** `PokemonGame/PokemonRoster.cs`

```csharp
public int[] GetPartyIndices() =>
    Party.Select(p => p is null ? -1 : All.IndexOf(p)).ToArray();
```

### PokemonMenu.cs — Pokemon rūšiavimas

**Failas:** `PokemonGame/PokemonMenu.cs`

```csharp
var alive = _roster.All.Where(p => p.IsAlive).ToList();
alive.Sort();
```
