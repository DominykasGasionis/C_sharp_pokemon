# PokemonRoster

**Projektas:** `PokemonGame`
**Failas:** `PokemonRoster.cs`

## Paskirtis

Saugo visą žaidėjo Pokemon kolekciją – tiek aktyvią partiją (iki 3), tiek visus pagautus Pokemon dėžėje. Valdo partijos keitimą ir gydymą.

## Savybės

| Savybė | Tipas | Aprašas |
|---|---|---|
| `All` | `List<Pokemon>` | Visi žaidėjo Pokemon |
| `Party` | `Pokemon?[3]` | Aktyvios partijos lizdai (max 3) |
| `ActivePokemon` | `Pokemon?` | Pirmas gyvas partijos Pokemon |

## Konstruktoriai

| Konstruktorius | Naudojimas |
|---|---|
| `PokemonRoster(Pokemon)` | Naujas žaidimas – vienas starteris |
| `PokemonRoster(List<Pokemon>, int[])` | Atstatymas iš išsaugojimo |

## Metodai

| Metodas | Aprašas |
|---|---|
| `Catch(Pokemon)` | Prideda pagautą Pokemon į `All`; jei partija nepilna – automatiškai į partiją |
| `HealParty()` | Gydo visus partijos Pokemon per `IHealable` sąsają |
| `GetPartyIndices()` | Grąžina partijos indeksus `All` sąraše (išsaugojimui) |

## Partijos valdymas

Partijoje yra 3 lizdai (`Party[0]`, `Party[1]`, `Party[2]`). Lizde gali būti `null` (tuščia vieta). `ActivePokemon` ieško pirmo gyvo (`IsAlive == true`) partijos nario.
