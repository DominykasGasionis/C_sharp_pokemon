# Battle

**Projektas:** `PokemonGame`
**Failas:** `Battle.cs`

## Paskirtis

Valdo vieną kovos sesiją tarp žaidėjo aktyvaus Pokemon ir laukinio. Piešia GBA stiliaus kovos ekraną ir apdoroja visus žaidėjo veiksmus.

## Kovos ekrano išdėstymas

```
╔═══════════════════════════════════════════════════════════════════════════════════════╗
║  ~ Rattata         Lv4                                                               ║
║  HP [██████████░░░░░░░░░░░░░░░░░░░░]                                                 ║
║                                                                                      ║
║                                        * Swalot         Lv8                         ║
║                                        HP [████████████████░░] 98/100               ║
╠══════════════════════════════════════════════╦═══════════════════════════════════════╣
║  Prieš tai buvusi žinutė                     ║  >FIGHT          POKEMON              ║
║  Dabartinė žinutė                            ║   BAG(3)         POKEBALL(5)          ║
║                                              ║   RUN                                 ║
╚══════════════════════════════════════════════╩═══════════════════════════════════════╝
```

## Meniu struktūra

| Pozicija | Veiksmas |
|---|---|
| (0,0) FIGHT | Atidaro judesių pasirinkimą |
| (0,1) POKEMON | Atidaro partijos keitimą |
| (1,0) BAG | Naudoja vaistą (Potion, +20 HP) |
| (1,1) POKEBALL | Meta Pokeball – pagauti laukinį |
| (2,0) RUN | Bando pabėgti (60% šansas) |

## Navigacija

- `WASD` arba rodyklių klavišai – juda per meniu
- `Enter`/`Tarpas` – patvirtina
- `Esc` – grįžta atgal
- `1`–`5` – tiesioginis pasirinkimas

## Kovos eiga

1. Žaidėjas pasirenka veiksmą
2. Veiksmas vykdomas (ataka, keitimas, daiktas ir t.t.)
3. Laukinis atakuoja (nebent žaidėjas pabėgo ar nukovė laukinį)
4. Tikrinamas statusų poveikis (nuodai −HP, paralyžius 30% praleidžia ataką)
5. Tikrinama ar kas nors krito

## Statusų efektai kovoje

- **Poisoned (PSN):** aktyvus Pokemon netenka `MaxHp/8` HP kiekvieną ėjimą
- **Paralyzed (PAR):** 30% šansas, kad laukinis negali atakuoti; 15% šansas apnuodyti arba paralyžiuoti po atakos

## XP sistema

Nugalėjus laukinį: `_active.GainExperience(_wild.XpReward)`. Jei pakyla lygis – rodoma žinutė kovos žurnale. XP atlygis priklauso nuo laukinio rūšies.

## Rezultatai (`BattleResult`)

| Reikšmė | Situacija |
|---|---|
| `PlayerWon` | Laukinis nukovotas |
| `PlayerFled` | Žaidėjas pabėgo |
| `PlayerLost` | Visi žaidėjo Pokemon krito |
| `PokemonCaught` | Laukinis pagautas Pokeball |
