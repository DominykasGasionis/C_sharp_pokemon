# 10. Statinis konstruktorius — 1 t.

## Reikalavimas

Naudojamas statinis konstruktorius.

## Implementacija

**Failas:** `PokemonGame/Map.cs`

```csharp
public class Map
{
    private static readonly Dictionary<TileType, char> TileChars;
    private static readonly Dictionary<TileType, (ConsoleColor Fg, ConsoleColor Bg)> TileColors;

    static Map()
    {
        TileChars = new Dictionary<TileType, char>
        {
            { TileType.Path,       '·' },
            { TileType.TallGrass,  '"' },
            // ...
        };

        TileColors = new Dictionary<TileType, (ConsoleColor Fg, ConsoleColor Bg)>
        {
            { TileType.Path,       (ConsoleColor.DarkYellow, ConsoleColor.Black) },
            // ...
        };
    }
}
```

## Paaiškinimas

Statinis konstruktorius (`static ClassName()`) vykdomas automatiškai vieną kartą – prieš pirmą klasės naudojimą. Jis inicializuoja `TileChars` ir `TileColors` žodynus, kurie aprašo kiekvienos žemėlapio plytelės simbolį ir spalvą. `readonly` laukams leidžiama priskirti reikšmes statiniame konstruktoriuje.

Statinis konstruktorius veikia tą patį momentą kaip inline inicializacija, bet leidžia sudėtingesnę inicializacijos logiką ir aiškesnį kodo grupavimą.
