# 2. IComparable\<T\> — 0.5 t.

## Reikalavimas

Teisingai atlikote implementaciją `IComparable<T>`.

## Implementacija

**Failas:** `PokemonGame.Core/Pokemon.cs`

```csharp
public class Pokemon : Entity, IComparable<Pokemon>, IFormattable, IHealable
{
    public int CompareTo(Pokemon? other) =>
        other is null ? 1 : Level.CompareTo(other.Level);
}
```

## Semantika

Du Pokemon lyginami pagal jų **lygį**. Aukštesnio lygio Pokemon yra „didesnis".

## Kur naudojama

`PokemonMenu.cs` – dėžės Pokemon surūšiuojami nuo žemiausio iki aukščiausio lygio:

```csharp
var box = _roster.All.Where(p => !_roster.Party.Contains(p)).ToList();
box.Sort(); // naudoja Pokemon.CompareTo
return box;
```

Taip žaidėjui lengviau rasti stipriausius Pokemon dėžėje.
