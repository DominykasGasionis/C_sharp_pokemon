# SaveSystem

**Projektas:** `PokemonGame`
**Failas:** `SaveSystem.cs`
**Tipas:** statinė klasė

## Paskirtis

Valdo žaidimo išsaugojimą ir įkėlimą JSON formatu. Išsaugojimo failas laikomas `~/.pokemongame/save.json`.

## Išsaugojimo struktūra (`SaveData`)

| Laukas | Tipas | Aprašas |
|---|---|---|
| `AllPokemon` | `List<PokemonSaveEntry>` | Visi žaidėjo Pokemon |
| `PartyIndices` | `int[]` | Partijos narių indeksai `AllPokemon` sąraše |
| `PlayerX`, `PlayerY` | `int` | Žaidėjo pozicija |
| `EncounterChance` | `int` | Susidūrimų dažnis |
| `Pokeballs`, `Potions` | `int` | Inventoriaus kiekiai |

## `PokemonSaveEntry` laukai

`Name`, `MaxHp`, `Hp`, `Attack`, `Defense`, `Level`, `Experience`

## Metodai

| Metodas | Aprašas |
|---|---|
| `Save(...)` | Serializuoja žaidimo būseną į JSON |
| `Load()` | Deserializuoja ir grąžina `SaveData?` |
| `RosterFromSave(SaveData)` | Atgamina `PokemonRoster` iš išsaugojimo duomenų |
| `SaveExists()` | Tikrina ar egzistuoja išsaugojimo failas |
| `Delete()` | Ištrina išsaugojimo failą (prieš naują žaidimą) |
