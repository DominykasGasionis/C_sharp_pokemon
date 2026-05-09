# 1. Savo sąsaja (interface) — 0.5 t.

## Reikalavimas

Sukūrėte ir pritaikėte savo sąsają (interface).

## Implementacija

**Failas:** `PokemonGame.Core/IHealable.cs`

```csharp
public interface IHealable
{
    int  Hp    { get; }
    int  MaxHp { get; }
    int  Heal(int amount);
    void HealFull();
}
```

## Kur naudojama

`Pokemon` klasė implementuoja `IHealable`:

```csharp
public class Pokemon : Entity, IComparable<Pokemon>, IFormattable, IHealable
```

`PokemonRoster.HealParty()` naudoja `IHealable` tipą, atsieidamas nuo konkretaus `Pokemon` tipo:

```csharp
foreach (IHealable? p in Party)
    p?.HealFull();
```
