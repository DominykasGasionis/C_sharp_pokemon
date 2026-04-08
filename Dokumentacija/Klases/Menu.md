# Menu ir GameSettings

**Projektas:** `PokemonGame`
**Failas:** `Menu.cs`

## GameSettings

Paprasta konfigūracijos klasė, saugoma viso žaidimo sesijos metu.

| Savybė | Tipas | Numatyta | Aprašas |
|---|---|---|---|
| `EncounterChance` | `int` | 25 | Tikimybė (%) sutikti Pokemon aukštoje žolėje |

## Menu

Pagrindinis meniu su ASCII art logotipu. Valdo navigaciją ir grąžina `MenuAction` reikšmę.

### Parinktys (su išsaugojimu)

1. Naujas žaidimas
2. Tęsti
3. Nustatymai
4. Išeiti

### Parinktys (be išsaugojimo)

1. Žaisti
2. Nustatymai
3. Išeiti

### Nustatymų meniu

Leidžia keisti Pokemon pasirodymo dažnį (0–100%) naudojant slankiklį (`←`/`→`). Vizualiai rodomas slankiklio juostos atvaizdas.

## MenuAction

```csharp
public enum MenuAction { NewGame, Continue, Settings, Exit }
```
