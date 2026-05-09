# Move

**Projektas:** `PokemonGame.Core`
**Failas:** `Move.cs`
**Implementuoja:** `IEquatable<Move>`

## Paskirtis

Reprezentuoja vieną kovos judesį. Kiekvienas Pokemon turi iki 4 judesių (apibrėžtų `Pokemon.SpeciesMoves`), kuriuos naudoja kovos metu.

## Kodas

```csharp
public class Move : IEquatable<Move>
{
    public string Name  { get; }
    public int    Power { get; }

    public Move(string name, int power)
    {
        Name  = name;
        Power = power;
    }

    public bool Equals(Move? other) =>
        other is not null && Name == other.Name && Power == other.Power;

    public override bool Equals(object? obj) => Equals(obj as Move);

    public override int GetHashCode() => HashCode.Combine(Name, Power);
}
```

## Savybės

| Savybė | Tipas | Aprašas |
|---|---|---|
| `Name` | `string` | Judesio pavadinimas (pvz. `"Flamethrower"`, `"Tackle"`) |
| `Power` | `int` | Judesio galia – bazinis žalos skaičius |

## Žalos formulė (naudojama `Battle.cs`)

```
rawDamage   = move.Power / 5 + attacker.Attack + rng(-5..+5)
actualDamage = max(1, rawDamage - defender.Defense)
```

Minimalus žalos kiekis visada yra **1**, net jei gynybos statistika yra labai didelė.

## Lygybė (IEquatable)

Du judesiai laikomi lygiais jei sutampa **ir** `Name`, **ir** `Power`:

```csharp
new Move("Tackle", 40).Equals(new Move("Tackle", 40)) // true
new Move("Tackle", 40).Equals(new Move("Tackle", 50)) // false (skirtingas Power)
new Move("Scratch", 40).Equals(new Move("Tackle", 40)) // false (skirtingas Name)
```

`GetHashCode()` naudoja `HashCode.Combine(Name, Power)` – atitinka `Equals` taisyklę.

## Judesių galingumas (orientacinė lentelė)

| Galia | Pavyzdžiai |
|---|---|
| 15–40 | Tackle, Scratch, Gust, Poison Sting |
| 45–65 | Vine Whip, Ember, Bug Bite, Fire Fang |
| 85–90 | Body Slam, Sludge Bomb, Flamethrower, Thunderbolt |
| 110–140 | Hydro Pump, Gunk Shot, Solar Beam, Last Resort |
