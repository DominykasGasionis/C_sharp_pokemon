# PokemonSelector

**Projektas:** `PokemonGame`
**Failas:** `PokemonSelector.cs`

## Paskirtis

Pradinio Pokemon pasirinkimo ekranas. Rodomas kuriant naują žaidimą. Atsitiktinai parodo 3 skirtingus Pokemon iš galimų starterių ir leidžia žaidėjui pasirinkti vieną.

## Veikimas

### Konstruktorius

```csharp
public PokemonSelector(Random rng)
{
    var pool = Pokemon.StarterPool.ToList();  // 8 galimi starteriai
    _choices = new (string, int, int, int)[3];
    for (int i = 0; i < 3; i++)
    {
        int idx = rng.Next(pool.Count);
        _choices[i] = pool[idx];
        pool.RemoveAt(idx);  // pašalina kad nesikartotų
    }
}
```

Išrenkami **3 unikalūs** starteriai be pasikartojimų (kiekvienas pasirinktas Pokemon pašalinamas iš pool'o).

### `Run()` grąžina

- **`Pokemon`** – pasirinktas starteris (naujas objektas su pradžios statistikomis)
- **`null`** – žaidėjas paspaudė `Esc`/`Backspace` ir grįžo į pagrindinį meniu

## Kortelių vizualizacija

```
  ╔══════════════╗   ┌──────────────┐   ┌──────────────┐
  ║  Swalot      ║   │  Pikachu     │   │  Eevee       │
  ║  HP:  100    ║   │  HP:  35     │   │  HP:  55     │
  ║  ATK: 73     ║   │  ATK: 55     │   │  ATK: 45     │
  ║  DEF: 83     ║   │  DEF: 40     │   │  DEF: 45     │
  ╚══════════════╝   └──────────────┘   └──────────────┘
          ▲
```

- **Aktyvus** pasirinkimas – dvigubi rėmelio simboliai `╔╗╚╝`, geltona spalva, `▲` žymeklis apačioje
- **Neaktyvūs** – viengubi `┌┐└┘`, tamsiai pilka spalva

## Navigacija

| Klavišas | Veiksmas |
|---|---|
| `A` / `←` | Pasirinkti kairįjį Pokemon |
| `D` / `→` | Pasirinkti dešinįjį Pokemon |
| `Enter` / `Tarpas` | Patvirtinti pasirinkimą |
| `Esc` / `Backspace` | Grįžti į pagrindinį meniu (grąžina `null`) |

## Galimi starteriai (`Pokemon.StarterPool`)

| Pokemon | HP | ATK | DEF |
|---|---|---|---|
| Bulbasaur | 45 | 49 | 49 |
| Charmander | 39 | 52 | 43 |
| Squirtle | 44 | 48 | 65 |
| Pikachu | 35 | 55 | 40 |
| Eevee | 55 | 45 | 45 |
| Gulpin | 40 | 45 | 35 |
| Swalot | 100 | 73 | 83 |
| Slugma | 70 | 80 | 50 |

Pasirinktas Pokemon sukuriamas **5-ame lygyje** (numatyta konstruktoriaus reikšmė).
