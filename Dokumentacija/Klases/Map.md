# Map

**Projektas:** `PokemonGame`
**Failas:** `Map.cs`

## Paskirtis

Saugo 2D žemėlapio plytelių masyvą, valdo praeinamumo patikrinimą ir piešia žemėlapį į `ScreenBuffer`. Žemėlapio dydis: **71×17** plytelių.

## Plytelių tipai (`TileType`)

| Simbolis | Tipas | Praeinama | Aprašas |
|---|---|---|---|
| `.` | `Path` | Taip | Kelias |
| `g` | `TallGrass` | Taip | Aukšta žolė – susidūrimai su Pokemon |
| `~` | `Water` | Ne | Vanduo |
| `^` | `Tree` | Ne | Medis |
| `B` | `Building` | Ne | Pastatas |
| `,` | `Sand` | Taip | Smėlis |
| `*` | `Flower` | Taip | Gėlės |
| `H` | `HealCenter` | Taip | Pokemon centras – gydo partiją |

## Statinis konstruktorius

`static Map()` inicializuoja du statinius žodynus:
- `TileChars` – plytelės tipas → rodomas simbolis
- `TileColors` – plytelės tipas → priekinio ir fono spalvos

## Metodai

| Metodas | Grąžina | Aprašas |
|---|---|---|
| `IsPassable(int x, int y)` | `bool` | Ar pozicija praeinama |
| `GetTile(int x, int y)` | `TileType` | Grąžina plytelės tipą |
| `Render(ScreenBuffer, int, int, int, int)` | `void` | Piešia žemėlapį su rėmeliu, žaidėjas rodomas kaip `@` |
