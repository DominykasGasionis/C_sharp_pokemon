# Map

**Projektas:** `PokemonGame`
**Failas:** `Map.cs`

## Paskirtis

Saugo 2D žemėlapio plytelių masyvą (`TileType[,]`), valdo praeinamumo patikrinimą ir piešia žemėlapį į `ScreenBuffer`. Žemėlapio dydis: **71×17** plytelių.

## Plytelių tipai (`TileType` enum)

| Simbolis layout'e | Tipas | Praeinama | Žemėlapio simbolis | Spalva |
|---|---|---|---|---|
| `.` | `Path` | Taip | `·` | Tamsiai geltona |
| `g` | `TallGrass` | Taip | `"` | Žalia |
| `~` | `Water` | Ne | `~` | Žydra ant tamsiai mėlyno fono |
| `^` | `Tree` | Ne | `^` | Tamsiai žalia |
| `B` | `Building` | Ne | `▪` | Pilka |
| `#` | `Wall` | Ne | `█` | Tamsiai pilka |
| `,` | `Sand` | Taip | `·` | Geltona |
| `*` | `Flower` | Taip | `✿` | Violetinė |
| `H` | `HealCenter` | Taip | `✚` | Balta ant tamsiai raudono fono |

## Statinis konstruktorius

`static Map()` inicializuoja du statinius žodynus vieną kartą:
- `TileChars` – `TileType` → rodomas simbolis
- `TileColors` – `TileType` → `(ConsoleColor Fg, ConsoleColor Bg)` pora

## Konstruktorius – žemėlapio generavimas

Žemėlapis aprašytas simbolių eilutėmis (`string[]`):
```
"^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^"
"^.....................................................................^"
"^.,,,,,,,,...BBB..^^^^^.........gggggggg...^^^^^........~~~~~~~~~~~..^"
...
```

Kiekvienas simbolis konvertuojamas į `TileType` per `switch` reikšmę.

## Metodai

| Metodas | Grąžina | Aprašas |
|---|---|---|
| `IsPassable(int x, int y)` | `bool` | `false` jei koordinatės už ribų arba plytelė nepraeinama |
| `GetTile(int x, int y)` | `TileType` | Grąžina plytelės tipą pagal koordinates |
| `Render(ScreenBuffer, playerX, playerY, leftPad, rightPad)` | `void` | Piešia žemėlapį su rėmeliu; žaidėjas rodomas kaip `@` |

## Praeinamumo logika

```csharp
public bool IsPassable(int x, int y)
{
    if (x < 0 || x >= Width || y < 0 || y >= Height) return false;

    return _tiles[y, x] switch
    {
        TileType.Water    => false,
        TileType.Tree     => false,
        TileType.Wall     => false,
        TileType.Building => false,
        _                 => true,
    };
}
```

## Piešimo logika

`Render()` piešia žemėlapį su rėmeliu `┌─┐│└┘`. Kiekvienai plytelei taikoma spalva iš `TileColors`. Žaidėjo pozicijoje vietoj plytelės piešiamas `@` (balta spalva).
