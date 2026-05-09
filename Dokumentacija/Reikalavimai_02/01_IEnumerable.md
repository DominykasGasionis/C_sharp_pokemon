# 1. IEnumerable\<T\> implementacija — 1 t.

## Reikalavimas

Teisingai atlikote implementaciją `IEnumerable<T>`.

## Implementacija

**Failas:** `PokemonGame/PokemonRoster.cs`

`PokemonRoster` klasė implementuoja `IEnumerable<Pokemon>`:

```csharp
public class PokemonRoster : IEnumerable<Pokemon>
{
    // ...

    public IEnumerator<Pokemon> GetEnumerator() => new PokemonRosterEnumerator(All);
    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => GetEnumerator();
}
```

## Kur naudojama

`Battle` konstruktoriuje iteruojama per visus roster Pokemon naudojant `foreach`:

```csharp
foreach (var p in _roster)
    p.OnLevelUp += (pokemon, level) =>
        _log.Add($"★ {pokemon.Name} pasiekė {level} lygį!");
```
