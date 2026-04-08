# PokemonSelector

**Projektas:** `PokemonGame`
**Failas:** `PokemonSelector.cs`

## Paskirtis

Pradinio Pokemon pasirinkimo ekranas. Atsitiktinai parodo 3 Pokemon iš galimų starterių ir leidžia žaidėjui pasirinkti vieną.

## Veikimas

Konstruktoriuje iš `Pokemon.StarterPool` atsitiktinai išrenkami 3 unikalūs Pokemon (be pasikartojimų). Kiekvienas rodomas kaip kortelė su statistikomis.

```
  ╔══════════════╗   ┌──────────────┐   ┌──────────────┐
  ║  Swalot      ║   │  Pikachu     │   │  Eevee       │
  ║  HP:  100    ║   │  HP:  35     │   │  HP:  55     │
  ║  ATK: 73     ║   │  ATK: 55     │   │  ATK: 45     │
  ║  DEF: 83     ║   │  DEF: 40     │   │  DEF: 45     │
  ╚══════════════╝   └──────────────┘   └──────────────┘
          ▲
```

Aktyviai pažymėta kortelė rodoma su dvigubo rėmelio simboliais `╔╗╚╝` ir `▲` žymekliu apačioje.

## Navigacija

- `A`/`←` – į kairę
- `D`/`→` – į dešinę
- `Enter`/`Tarpas` – patvirtinti
- `Esc`/`Backspace` – grįžti į pagrindinį meniu (grąžina `null`)
