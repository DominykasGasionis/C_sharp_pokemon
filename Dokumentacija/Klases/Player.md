# Player

**Projektas:** `PokemonGame`
**Failas:** `Player.cs`
**Paveldėjimas:** `Entity`

## Paskirtis

Saugo žaidėjo poziciją žemėlapyje ir valdo judėjimą. Prieš kiekvieną žingsnį tikrina ar nauja pozicija yra praeinama per `Map.IsPassable()`.

## Kodas

```csharp
public class Player : Entity
{
    public int X { get; private set; }
    public int Y { get; private set; }

    public Player(int startX, int startY) { X = startX; Y = startY; }

    public override string GetDisplayName() => "@";

    public bool TryMove(int dx, int dy, Map map)
    {
        int newX = X + dx;
        int newY = Y + dy;

        if (!map.IsPassable(newX, newY))
            return false;

        X = newX;
        Y = newY;
        return true;
    }
}
```

## Savybės

| Savybė | Tipas | Aprašas |
|---|---|---|
| `X` | `int` | Horizontali pozicija žemėlapyje (stulpelis) |
| `Y` | `int` | Vertikali pozicija žemėlapyje (eilutė) |

## Metodai

| Metodas | Grąžina | Aprašas |
|---|---|---|
| `TryMove(int dx, int dy, Map)` | `bool` | Bando pajudėti – `true` jei judėjimas pavyko, `false` jei blokuota |
| `GetDisplayName()` | `string` | Grąžina `"@"` – žaidėjo simbolis žemėlapyje |

## Judėjimo logika

1. Apskaičiuojama nauja pozicija: `newX = X + dx`, `newY = Y + dy`
2. Klausiama `Map.IsPassable(newX, newY)` – ar plytelė praeinama
3. Jei **taip** – atnaujinamos koordinatės, grąžinama `true`
4. Jei **ne** – koordinatės nesikeičia, grąžinama `false`

`Game.HandleInput()` tikrina grąžintą reikšmę – jei `false`, rodomas pranešimas `"Negalima eiti ten!"`.

## Koordinačių sistema

- `X` = 0 yra kairysis kraštas
- `Y` = 0 yra viršutinis kraštas
- `dx = +1` → judėjimas dešinėn, `dx = -1` → kairėn
- `dy = +1` → judėjimas žemyn, `dy = -1` → aukštyn
