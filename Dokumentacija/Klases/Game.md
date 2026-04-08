# Game

**Projektas:** `PokemonGame`
**Failas:** `Game.cs`

## Paskirtis

Pagrindinis žaidimo ciklo valdiklis. Valdo žemėlapio atvaizdavimą, žaidėjo judėjimą, susidūrimų su Pokemon trigerius ir Pokemon centro gydymą.

## Konstruktoriaus parametrai

| Parametras | Tipas | Aprašas |
|---|---|---|
| `settings` | `GameSettings` | Susidūrimų dažnio nustatymai |
| `roster` | `PokemonRoster` | Žaidėjo Pokemon kolekcija |
| `startX`, `startY` | `int` | Pradinė pozicija (numatyta: 1, 1) |
| `inventory` | `Inventory?` | Daiktų inventorius (numatyta: naujas) |

## Pagrindinis ciklas (`Run`)

1. Piešia žemėlapį su HUD
2. Laukia klavišo paspaudimo
3. `Esc`/`Q` → išsaugo ir išeina
4. `I` → atidaro Pokemon meniu
5. `WASD`/rodyklės → juda žemėlapiu

## HUD struktūra

```
╔═══════════════════════╗
║ [1]☻ Swalot   Lv8  HP [████████████████░░░░] 98/100 ║
╠───────────────────────╣
║ Statusas               ║
║ (5,8)  Vietovė: Kelias ║
╠───────────────────────╣
║ [WASD] Judėti [I] ...  ║
╚═══════════════════════╝
```

## Susidūrimų logika

Einant ant `TallGrass` plytelės – atsitiktinis patikrinimas pagal `settings.EncounterChance` (numatyta 25%). Jei trigeras – paleičiama kova per `Battle.Run()`.

## Pokemon centras

Einant ant `HealCenter` plytelės – pilnai gydo visą partiją ir prideda `+1 Pokeball` bei `+1 Potion` per `Inventory +` operatorių.

## Metodai

| Metodas | Aprašas |
|---|---|
| `Run()` | Pagrindinis žaidimo ciklas |
| `HandleInput(ConsoleKey)` | Apdoroja judėjimą ir triggerius |
| `TriggerBattle()` | Sukuria laukinį Pokemon, paleidžia kovą |
| `TriggerHeal()` | Gydo partiją Pokemon centre |
| `Render()` | Piešia žemėlapį ir HUD per `ScreenBuffer` |
