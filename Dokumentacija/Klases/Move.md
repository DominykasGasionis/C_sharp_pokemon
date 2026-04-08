# Move

**Projektas:** `PokemonGame.Core`
**Failas:** `Move.cs`
**Implementuoja:** `IEquatable<Move>`

## Paskirtis

Reprezentuoja vieną kovos judesį. Kiekvienas Pokemon turi iki 4 judesių, kuriuos naudoja kovos metu.

## Savybės

| Savybė | Tipas | Aprašas |
|---|---|---|
| `Name` | `string` | Judesio pavadinimas (pvz. `"Flamethrower"`) |
| `Power` | `int` | Judesio galia, naudojama žalos skaičiavimui |

## Žalos formulė

```
rawDamage = Power / 5 + Pokemon.Attack + rng(-5..5)
actualDamage = max(1, rawDamage - enemy.Defense)
```

## Lygybė

Du judesiai laikomi lygiais jei sutampa ir `Name`, ir `Power`:

```csharp
new Move("Tackle", 40).Equals(new Move("Tackle", 40)) // true
new Move("Tackle", 40).Equals(new Move("Tackle", 50)) // false
```

`GetHashCode()` naudoja `HashCode.Combine(Name, Power)`.
