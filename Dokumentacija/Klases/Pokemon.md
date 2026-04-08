# Pokemon

**Projektas:** `PokemonGame.Core`
**Failas:** `Pokemon.cs`
**Paveldėjimas:** `Entity`, `IComparable<Pokemon>`, `IFormattable`, `IHealable`

## Paskirtis

Pagrindinė žaidimo esybė – tiek žaidėjo, tiek laukinio Pokemon duomenų saugojimas ir logika. Kiekvienas Pokemon turi statistikas, judėjimų sąrašą, lygį, patyrimą ir statusų sistemą.

## Savybės

| Savybė | Tipas | Aprašas |
|---|---|---|
| `Name` | `string` | Pokemon vardas |
| `Hp` | `int` | Dabartiniai gyvybės taškai |
| `MaxHp` | `int` | Maksimalūs gyvybės taškai |
| `Attack` | `int` | Atakos statistika |
| `Defense` | `int` | Gynybos statistika |
| `Level` | `int` | Dabartinis lygis |
| `Experience` | `int` | Sukauptas XP einamajam lygiui |
| `XpReward` | `int` | XP, kurį gauna priešas nugalėjus |
| `Moves` | `IReadOnlyList<Move>` | Judesių sąrašas |
| `Status` | `StatusEffect` | Aktyvūs statusų efektai (Flags enum) |
| `IsAlive` | `bool` | `true` jei `Hp > 0` |
| `ExperienceToNextLevel` | `int` | XP reikalingas sekančiam lygiui (`Level * 50`) |

## Metodai

| Metodas | Grąžina | Aprašas |
|---|---|---|
| `TakeDamage(int)` | `int` | Pritaiko žalą (atimant Defense), grąžina faktinę žalą |
| `Heal(int)` | `int` | Gydo iki MaxHp, grąžina faktiškai atgautus HP |
| `HealFull()` | `void` | Pilnai atgauna HP |
| `GainExperience(int)` | `string?` | Prideda XP, jei pakyla lygis – grąžina žinutę |
| `ApplyStatus(StatusEffect)` | `void` | Prideda statusą (`\|=`) |
| `ClearStatus(StatusEffect)` | `void` | Pašalina statusą (`&= ~`) |
| `HasStatus(StatusEffect)` | `bool` | Tikrina statusą (`&`) |
| `HpBar(int)` | `string` | Grąžina HP juostą simboliais `█░` |
| `Deconstruct(...)` | `void` | Leidžia dekonstruoti: `var (name, hp, maxHp) = pokemon` |
| `CompareTo(Pokemon?)` | `int` | Lygina pagal lygį |
| `ToString(string?, ...)` | `string` | `"S"` → trumpas, `"L"` → ilgas, kita → vardas |
| `GetDisplayName()` | `string` | `"Vardas LvN"` |
| `RandomWild(Random)` | `Pokemon` | Sukuria atsitiktinį laukinį Pokemon (lygiai 2–8) |

## Statiniai duomenys

- `SpeciesMoves` – žodynas rūšies vardas → judesių sąrašas (12 rūšių)
- `StarterPool` – galimi starteriai pasirinkimo ekrane
- `WildPool` – galimi laukiniai Pokemon su XP atlygiais

## Statusų sistema

Naudoja `[Flags] StatusEffect` enum. Galima turėti kelis statusus vienu metu:

```csharp
pokemon.ApplyStatus(StatusEffect.Poisoned | StatusEffect.Paralyzed);
pokemon.HasStatus(StatusEffect.Poisoned); // true
pokemon.ClearStatus(StatusEffect.Poisoned);
```
