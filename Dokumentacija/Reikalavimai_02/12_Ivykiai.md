# 12. Įvykiai (events) — 1 t.

## Reikalavimas

Naudojate įvykius savo projekte.

## Implementacija

### Įvykio apibrėžimas

**Failas:** `PokemonGame.Core/Pokemon.cs`

`Pokemon` klasėje apibrėžtas įvykis `OnLevelUp`, naudojantis `Action<Pokemon, int>` delegatą:

```csharp
// Įvykis (event) – iššaukiamas kiekvieną kartą kai Pokemon pakyla lygiu
// Prenumeratoriai gauna Pokemon objektą ir naują lygį kaip argumentus
public event Action<Pokemon, int>? OnLevelUp;
```

Įvykis iššaukiamas `GainExperience()` metode:

```csharp
OnLevelUp?.Invoke(this, Level);
```

### Prenumerata

**Failas:** `PokemonGame/Battle.cs`

`Battle` konstruktoriuje prenumeruojamas įvykis kiekvienam roster Pokemon — lygio kėlimo žinutė automatiškai patenka į mūšio žurnalą:

```csharp
foreach (var p in _roster)
    p.OnLevelUp += (pokemon, level) =>
        _log.Add($"★ {pokemon.Name} pasiekė {level} lygį!");
```

## Veikimas

Kai `GainExperience()` iššaukia `OnLevelUp`, mūšio žurnale automatiškai pasirodo žinutė apie lygio pakilimą, nepriklausomai nuo to, kur metodas buvo iškviestas.
