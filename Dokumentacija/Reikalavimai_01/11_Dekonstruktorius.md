# 11. Dekonstruktorius — 0.5 t.

## Reikalavimas

Naudojamas dekonstruktorius.

## Implementacija

**Failas:** `PokemonGame.Core/Pokemon.cs`

```csharp
public void Deconstruct(out string name, out int hp, out int maxHp)
{
    name  = Name;
    hp    = Hp;
    maxHp = MaxHp;
}
```

## Kur naudojama

**Failas:** `PokemonGame/PokemonMenu.cs`

```csharp
var (pokeName, hp, maxHp) = poke; // Deconstruct
string name = pokeName.PadRight(13);
return $"  {slotTag} {cursor} {name} Lv{poke.Level,-2}  HP: {hp,3}/{maxHp,-3} ...";
```

## Paaiškinimas

`Deconstruct` metodas su `out` parametrais leidžia naudoti C# destruktūrizacijos sintaksę. Kai rašoma `var (a, b, c) = objektas`, kompiliatorius automatiškai iškviečia `Deconstruct(out a, out b, out c)`. Tai leidžia patogiai išskleisti objekto savybes į atskirus kintamuosius.
