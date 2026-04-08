# 3. IEquatable\<T\> — 0.5 t.

## Reikalavimas

Teisingai atlikote implementaciją `IEquatable<T>`.

## Implementacija

**Failas:** `PokemonGame.Core/Move.cs`

```csharp
public class Move : IEquatable<Move>
{
    public bool Equals(Move? other) =>
        other is not null && Name == other.Name && Power == other.Power;

    public override bool Equals(object? obj) => Equals(obj as Move);

    public override int GetHashCode() => HashCode.Combine(Name, Power);
}
```

## Semantika

Du judesiai laikomi lygiais jei sutampa ir pavadinimas, ir galia. Tai prasminga, nes toks pats judesys skirtinguose Pokemon yra tas pats judesys:

```csharp
new Move("Tackle", 40).Equals(new Move("Tackle", 40)) // true
new Move("Tackle", 40).Equals(new Move("Ember",  40)) // false
new Move("Tackle", 40).Equals(new Move("Tackle", 50)) // false
```

Teisingai implementuotas `GetHashCode()` naudoja `HashCode.Combine`, kad vienodi objektai turėtų vienodus hash kodus – tai būtina taisyklė implementuojant `IEquatable<T>`.
