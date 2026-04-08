# 14. is operatorius — 0.5 t.

## Reikalavimas

Naudojamas operatorius `is`.

## Implementacija

**Failas:** `PokemonGame/Program.cs`

```csharp
var save = SaveSystem.Load();
if (save is null) break;
```

**Failas:** `PokemonGame.Core/Move.cs`

```csharp
public bool Equals(Move? other) =>
    other is not null && Name == other.Name && Power == other.Power;
```

**Failas:** `PokemonGame.Core/Pokemon.cs`

```csharp
public int CompareTo(Pokemon? other) => other is null ? 1 : Level.CompareTo(other.Level);
```

## Paaiškinimas

`is` operatorius tikrina ar reikšmė atitinka nurodytą tipą arba šabloną. Naudojamas trims tikslams:

- `x is null` – tipo saugesnis null tikrinimas nei `x == null`
- `x is not null` – priešinga sąlyga
- `obj is SomeType t` – tipo tikrinimas su priskyrimu (pattern matching)
