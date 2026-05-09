# 12. Operatorių perkrovimas — 0.5 t.

## Reikalavimas

Naudojamas operatorių perkrovimas.

## Implementacija

**Failas:** `PokemonGame/Inventory.cs`

```csharp
public static Inventory operator +(Inventory a, Inventory b) =>
    new Inventory { Pokeballs = a.Pokeballs + b.Pokeballs, Potions = a.Potions + b.Potions };
```

## Kur naudojama

**Failas:** `PokemonGame/Game.cs` – kai žaidėjas lankosi Pokemon centre:

```csharp
_inventory = _inventory + new Inventory { Pokeballs = 1, Potions = 1 };
```

Žaidėjas gauna +1 Pokeball ir +1 Potion kaskart apsilankęs Pokemon centre ir pasigydęs.

## Paaiškinimas

Operatorių perkrovimas leidžia naudoti įprastą matematikos sintaksę su savo klasėmis. `+` operatorius turi būti `static` ir grąžinti naują objektą (nekeistu originalų). Tai atitinka C# kalbos konvenciją – operatoriai neturėtų turėti šalutinių efektų.
