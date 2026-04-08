# Entity

**Projektas:** `PokemonGame.Core`
**Failas:** `Entity.cs`
**Tipas:** abstrakti klasė

## Paskirtis

Bendra bazinė klasė visoms žaidimo esybėms, kurios turi turėti rodomą pavadinimą. Apibrėžia bendrą kontraktą per abstraktų metodą.

## Metodai

| Metodas | Grąžina | Aprašas |
|---|---|---|
| `GetDisplayName()` | `string` | Abstraktus – privalo būti implementuotas paveldėtojų |

## Paveldėtojai

| Klasė | Implementacija |
|---|---|
| `Pokemon` | `"{Name} Lv{Level}"` – pvz. `"Pikachu Lv7"` |
| `Player` | `"@"` – žaidėjo simbolis žemėlapyje |
