# Inventory

**Projektas:** `PokemonGame`
**Failas:** `Inventory.cs`

## Paskirtis

Saugo žaidėjo daiktų kiekius. Palaiko sudėties operatorių, leidžiantį gauti naujus daiktus.

## Savybės

| Savybė | Tipas | Numatyta | Aprašas |
|---|---|---|---|
| `Pokeballs` | `int` | 5 | Pokemon gaudymo kamuoliukų kiekis |
| `Potions` | `int` | 3 | Vaistų (atgauna 20 HP) kiekis |

## Operatorius `+`

```csharp
var result = inventoryA + inventoryB;
// result.Pokeballs = inventoryA.Pokeballs + inventoryB.Pokeballs
// result.Potions   = inventoryA.Potions   + inventoryB.Potions
```

Naudojama `Game.cs` kai žaidėjas lankosi Pokemon centre:

```csharp
_inventory = _inventory + new Inventory { Pokeballs = 1, Potions = 1 };
```
