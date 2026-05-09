# Pokemon

**Projektas:** `PokemonGame.Core`
**Failas:** `Pokemon.cs`
**Paveldėjimas:** `Entity`
**Implementuoja:** `IComparable<Pokemon>`, `IFormattable`, `IHealable`

## Paskirtis

Pagrindinė žaidimo esybė – tiek žaidėjo, tiek laukinio Pokemon duomenų saugojimas ir logika. Kiekvienas Pokemon turi statistikas, judėjimų sąrašą, lygį, patyrimą ir statusų sistemą.

## Savybės

| Savybė | Tipas | Aprašas |
|---|---|---|
| `Name` | `string` | Pokemon vardas (nesikeičia) |
| `Hp` | `int` | Dabartiniai gyvybės taškai |
| `MaxHp` | `int` | Maksimalūs gyvybės taškai (auga kylant lygiui) |
| `Attack` | `int` | Atakos statistika (auga kylant lygiui) |
| `Defense` | `int` | Gynybos statistika (auga kylant lygiui) |
| `Level` | `int` | Dabartinis lygis |
| `Experience` | `int` | Sukauptas XP einamajam lygiui |
| `XpReward` | `int` | XP kiekis, kurį gauna priešas nugalėjus šį Pokemon |
| `Moves` | `IReadOnlyList<Move>` | Judesių sąrašas (iš `SpeciesMoves`) |
| `Status` | `StatusEffect` | Aktyvūs statusų efektai (Flags enum) |
| `IsAlive` | `bool` | `true` jei `Hp > 0` |
| `ExperienceToNextLevel` | `int` | XP reikalingas sekančiam lygiui: `Level * 50` |

## Konstruktorius

```csharp
public Pokemon(string name, int maxHp, int attack, int defense,
               int? currentHp = null, int level = 5, int experience = 0, int xpReward = 0)
```

- `currentHp` – jei nenurodytas, nustatomas lygus `maxHp` (naujas Pokemon)
- Judesiai automatiškai parenfami iš `SpeciesMoves` pagal `name`; jei rūšis nerasta – priskiriamas `Tackle`

## Metodai

| Metodas | Grąžina | Aprašas |
|---|---|---|
| `TakeDamage(int rawDamage)` | `int` | Pritaiko žalą: `max(1, rawDamage - Defense)`, grąžina faktinę žalą |
| `Heal(int amount)` | `int` | Gydo iki `MaxHp`, grąžina faktiškai atgautus HP |
| `HealFull()` | `void` | Nustato `Hp = MaxHp` |
| `GainExperience(int amount)` | `string?` | Prideda XP; jei pakyla lygis – grąžina pranešimą |
| `ApplyStatus(StatusEffect)` | `void` | Prideda statusą (bitų OR: `Status \|= effect`) |
| `ClearStatus(StatusEffect)` | `void` | Pašalina statusą (bitų AND NOT: `Status &= ~effect`) |
| `HasStatus(StatusEffect)` | `bool` | Tikrina statusą (bitų AND: `(Status & effect) != 0`) |
| `HpBar(int barWidth = 20)` | `string` | Grąžina HP juostą simboliais `█░`, pvz. `[████████░░░░░░░░░░░░]` |
| `Deconstruct(...)` | `void` | Leidžia: `var (name, hp, maxHp) = pokemon` |
| `CompareTo(Pokemon?)` | `int` | Lyginimas pagal lygį (naudojamas rūšiavimui) |
| `ToString(string?, ...)` | `string` | `"S"` → `"Pikachu Lv5"`, `"L"` → pilnas su HP ir statusu, kita → tik vardas |
| `GetDisplayName()` | `string` | Grąžina `"{Name} Lv{Level}"` |
| `RandomWild(Random)` | `Pokemon` | (statinis) Sukuria atsitiktinį laukinį Pokemon, lygio 2–8 |

## Lygio kėlimo sistema

```csharp
public string? GainExperience(int amount)
{
    Experience += amount;
    while (Experience >= ExperienceToNextLevel)  // ExperienceToNextLevel = Level * 50
    {
        Experience -= ExperienceToNextLevel;
        Level++;
        MaxHp  += 5;
        Hp      = Math.Min(Hp + 5, MaxHp);   // atgauna 5 HP (ne daugiau nei MaxHp)
        Attack  += 2;
        Defense += 2;
    }
}
```

Vienu kartu galima pakilti kelis lygius iš karto, jei XP yra pakankamai.

## Statusų sistema (`[Flags]` enum)

```csharp
[Flags]
public enum StatusEffect
{
    None      = 0,
    Poisoned  = 1 << 0,  // = 1
    Paralyzed = 1 << 1,  // = 2
}
```

`[Flags]` leidžia turėti kelis statusus vienu metu (bitų operacijos):

```csharp
pokemon.ApplyStatus(StatusEffect.Poisoned);   // Prideda nuodus
pokemon.ApplyStatus(StatusEffect.Paralyzed);  // Prideda paralyžių (abu aktyvūs)
pokemon.HasStatus(StatusEffect.Poisoned);     // true
pokemon.ClearStatus(StatusEffect.Poisoned);   // Pašalina tik nuodus
```

## Statiniai duomenys

### `SpeciesMoves` – 12 rūšių judesių žodynas

| Rūšis | Judesiai |
|---|---|
| Bulbasaur | Tackle, Vine Whip, Razor Leaf, Solar Beam (120) |
| Charmander | Scratch, Ember, Fire Fang, Flamethrower (90) |
| Squirtle | Tackle, Water Gun, Bubble Beam, Hydro Pump (110) |
| Pikachu | Tackle, Thunder Shock, Quick Attack, Thunderbolt (90) |
| Eevee | Tackle, Quick Attack, Swift, Last Resort (140) |
| Gulpin | Pound, Acid, Sludge, Sludge Bomb (90) |
| Swalot | Pound, Body Slam, Sludge Bomb, Gunk Shot (120) |
| ir kt. | ... |

### `StarterPool` – 8 galimi starteriai
Naudojamas `PokemonSelector` ekrane.

### `WildPool` – 8 laukiniai Pokemon
Naudojamas `RandomWild()`. Lygiai atsitiktiniai 2–8, statistikos perskaičiuojamos pagal lygį:
```csharp
scaledHp  = max(10, baseHp  + (level - 5) * 3)
scaledAtk = max(5,  baseAtk + (level - 5) * 2)
scaledDef = max(5,  baseDef + (level - 5) * 2)
```
