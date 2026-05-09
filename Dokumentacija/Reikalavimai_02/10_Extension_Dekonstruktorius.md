# 10. Plėtimo dekonstruktorius — 1 t.

## Reikalavimas

Sukūrėte praplėtimo dekonstruktorių.

## Implementacija

**Failas:** `PokemonGame/Extensions.cs`

`Move` tipui sukurtas plėtimo dekonstruktorius kaip statinis metodas su `this` parametru. Tai leidžia naudoti destruktūrizavimą su `Move` objektais nenekeičiant pačios klasės:

```csharp
// Plėtimo dekonstruktorius – leidžia naudoti var (name, power) = move;
public static void Deconstruct(this Move m, out string name, out int power)
{
    name  = m.Name;
    power = m.Power;
}
```

## Kur naudojama

```csharp
var (name, power) = someMove;
// name  == someMove.Name
// power == someMove.Power
```

## Skirtumas nuo įprasto dekonstruktoriaus

`Pokemon` klasėje yra įprasto tipo dekonstruktorius (metodas pačioje klasėje):

```csharp
public void Deconstruct(out string name, out int hp, out int maxHp) { ... }
```

Tuo tarpu `Move` dekonstruktorius yra **plėtimo** metodas — apibrėžtas išorinėje statinėje klasėje `Extensions`.
