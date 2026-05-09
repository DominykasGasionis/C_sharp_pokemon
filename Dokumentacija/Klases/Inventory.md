# Inventory

**Projektas:** `PokemonGame`
**Failas:** `Inventory.cs`

## Paskirtis

Saugo žaidėjo daiktų kiekius. Palaiko sudėties operatorių `+`, leidžiantį patogiai pridėti daiktus.

## Kodas

```csharp
public class Inventory
{
    public int Pokeballs { get; set; } = 5;
    public int Potions   { get; set; } = 3;

    public static Inventory operator +(Inventory a, Inventory b) =>
        new Inventory { Pokeballs = a.Pokeballs + b.Pokeballs, Potions = a.Potions + b.Potions };
}
```

## Savybės

| Savybė | Tipas | Numatyta | Aprašas |
|---|---|---|---|
| `Pokeballs` | `int` | 5 | Pokemon gaudymo kamuoliukų kiekis |
| `Potions` | `int` | 3 | Vaistų kiekis (kiekvienas atgauna 20 HP) |

## Operatorius `+`

Sukuria naują `Inventory` objektą, sudėjus abiejų inventorių kiekius:

```csharp
var a = new Inventory { Pokeballs = 3, Potions = 1 };
var b = new Inventory { Pokeballs = 1, Potions = 1 };
var result = a + b;
// result.Pokeballs = 4, result.Potions = 2
```

## Naudojimas žaidime

**Pokemon centras** – kiekvieną kartą užeinant automatiškai pridedama +1 Pokeball ir +1 Potion:
```csharp
_inventory = _inventory + new Inventory { Pokeballs = 1, Potions = 1 };
```

**Kova** – `Potions` naudojami `BAG` veiksmu (−1 per naudojimą, +20 HP aktyviam Pokemon), `Pokeballs` – `POKEBALL` veiksmu (−1 per metimą).

## Išsaugojimas

`Pokeballs` ir `Potions` išsaugomi JSON faile per `SaveData` klasę ir atstatomi įkeliant žaidimą.
