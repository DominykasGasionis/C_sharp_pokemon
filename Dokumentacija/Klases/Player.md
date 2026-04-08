# Player

**Projektas:** `PokemonGame`
**Failas:** `Player.cs`
**Paveldėjimas:** `Entity`

## Paskirtis

Saugo žaidėjo poziciją žemėlapyje ir valdo judėjimą. Tikrina ar nauja pozicija yra praeinama prieš judant.

## Savybės

| Savybė | Tipas | Aprašas |
|---|---|---|
| `X` | `int` | Horizontali pozicija žemėlapyje |
| `Y` | `int` | Vertikali pozicija žemėlapyje |

## Metodai

| Metodas | Grąžina | Aprašas |
|---|---|---|
| `TryMove(int dx, int dy, Map)` | `bool` | Bando pajudėti – grąžina `true` jei judėjimas pavyko |
| `GetDisplayName()` | `string` | Grąžina `"@"` – žaidėjo simbolis |

## Judėjimo logika

`TryMove` apskaičiuoja naują poziciją (`X+dx`, `Y+dy`) ir klausia `Map.IsPassable()`. Jei vieta praeinama – atnaujina koordinates ir grąžina `true`. Jei ne – lieka vietoje ir grąžina `false`.
