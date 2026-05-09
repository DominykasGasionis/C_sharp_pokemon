# 11. ICloneable implementacija — 1 t.

## Reikalavimas

Teisingai atlikote implementaciją `ICloneable`.

## Implementacija

**Failas:** `PokemonGame.Core/Pokemon.cs`

`Pokemon` klasė implementuoja `ICloneable` ir jo `Clone()` metodą, kuris sukuria naują Pokemon objektą su identiškomis statistikomis:

```csharp
public class Pokemon : Entity, IComparable<Pokemon>, IFormattable, IHealable, ICloneable
{
    // ...

    // ICloneable implementacija – sukuria naują Pokemon kopiją su identiškomis statistikomis
    public object Clone() =>
        new Pokemon(Name, MaxHp, Attack, Defense, Hp, Level, Experience, XpReward);
}
```

## Naudojimo pavyzdys

```csharp
var original = new Pokemon("Pikachu", 35, 55, 40);
var kopija   = (Pokemon)original.Clone();
// kopija yra atskiras objektas su tomis pačiomis statistikomis
```
