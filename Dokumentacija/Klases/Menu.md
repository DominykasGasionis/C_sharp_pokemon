# Menu ir GameSettings

**Projektas:** `PokemonGame`
**Failas:** `Menu.cs`

---

## GameSettings

Paprasta konfigūracijos klasė, saugoma viso žaidimo sesijos metu ir išsaugoma JSON faile.

```csharp
public class GameSettings
{
    public int EncounterChance { get; set; } = 25;
}
```

| Savybė | Numatyta | Aprašas |
|---|---|---|
| `EncounterChance` | 25 | Tikimybė (%) sutikti Pokemon einant per aukštą žolę |

---

## MenuAction enum

```csharp
public enum MenuAction { NewGame, Continue, Settings, Exit }
```

Grąžinama reikšmė iš `Menu.Run()`, kurią naudoja `Program.cs` sprendimų srautui.

---

## Menu

### Lauko kintamieji

| Kintamasis | Tipas | Aprašas |
|---|---|---|
| `_settings` | `GameSettings` | Nuoroda į žaidimo nustatymų objektą |
| `_selected` | `int` | Šiuo metu pažymėtos parinkties indeksas |
| `Logo` | `string[]` | Statinis ASCII art logotipo masyvas (6 eilutės) |

---

## Menu metodai

### `Menu(GameSettings settings)` – konstruktorius

Išsaugo `settings` nuorodą. `_selected` prasideda nuo `0`.

---

### `MenuAction Run()`

**Pagrindinis meniu ciklas.** Piešia meniu ekraną ir laukia įvesties.

Prieš ciklą:
- Tikrina `SaveSystem.SaveExists()` → nustato `hasSave`
- Parenka parinkčių masyvą (`options`): su išsaugojimu – 4 parinktys, be – 3
- Apriboja `_selected` kad neviršytų `options.Length - 1`

Cikle:
1. `RenderMain(options, hasSave)` – piešia ekraną
2. `Console.ReadKey(intercept: true)` – laukia klavišo

| Klavišas | Veiksmas |
|---|---|
| `W` / `↑` | `_selected = (_selected - 1 + length) % length` (apskritas) |
| `S` / `↓` | `_selected = (_selected + 1) % length` (apskritas) |
| `Enter` / `Tarpas` | Grąžina `MenuAction` pagal `_selected` ir `hasSave` |
| `Esc` | Grąžina `MenuAction.Exit` |

---

### `void RenderMain(string[] options, bool hasSave)`

Privatus metodas. **Piešia pagrindinį meniu ekraną.**

Piešimo seka:
1. `Console.Clear()`, UTF-8 enkodavimas
2. Centruotas ASCII logotipas geltonai
3. `"─── Konsolinis RPG žaidimas ───"` paantraštė
4. Meniu rėmelis (`╔═╗`/`╚═╝`), prieš paskutinę parinktį – skyriklis `╠═╣`
5. Kiekviena parinktis su `► ` žymekliu jei aktyvi
6. Aktyvi parinktis – **invertuotos spalvos** (juodas tekstas ant geltono; „Tęsti" – ant žalio)
7. Jei `hasSave` – rodomas `"💾  Išsaugojimas rastas"`
8. Klavišų pagalba apačioje pilkos spalvos

---

### `void OpenSettings()`

Nustatymų submeniu ciklas. Valdo 2 elementus (`sel = 0` – slankiklis, `sel = 1` – Grįžti).

Cikle:
1. `RenderSettings(sel)` – piešia nustatymų ekraną
2. `Console.ReadKey(intercept: true)`

| Klavišas | `sel` | Veiksmas |
|---|---|---|
| `W` / `↑` | bet kuris | `sel = (sel - 1 + 2) % 2` |
| `S` / `↓` | bet kuris | `sel = (sel + 1) % 2` |
| `A` / `←` | 0 | `EncounterChance -= 5` (min 0) |
| `D` / `→` | 0 | `EncounterChance += 5` (max 100) |
| `Esc` / `Backspace` | bet kuris | `return` |
| `Enter` | 1 (Grįžti) | `return` |

---

### `void RenderSettings(int selected)`

Privatus metodas. **Piešia nustatymų ekraną.**

1. `Console.Clear()`
2. Rėmelio antraštė `NUSTATYMAI`
3. `EncounterChance` eilutė: `►` jei aktyvi, slankiklio juosta per `BuildSlider()`
4. Horizontali atskyrimo linija
5. „Grįžti į meniu" mygtukas – invertuotos spalvos jei pasirinktas

---

### `string BuildSlider(int value, int min, int max, int width)`

Statinis privatus metodas. Grąžina slankiklio vizualizaciją kaip eilutę.

```csharp
// Pvz: value=25, min=0, max=100, width=16
// Apskaičiuoja poziciją: pos = round(25/100 * 16) = 4
// Rezultatas: "[────●───────────]"
```

Algoritmas:
- `pos = round((value - min) / (max - min) * width)` – `●` simbolio pozicija
- Kiekviena pozicija: `i == pos` → `●`, kitu atveju → `─`
- Apgaubiama `[` ir `]`

---

## Pagalbiniai metodai

### `void Centered(string text, ConsoleColor color)`

Statinis privatus metodas. Spausdina `text` horizontaliai centruotą terminalo lange. Centravimas: `pad = max(0, (W - text.Length) / 2)`.

### `string Pad(string text, int totalWidth)`

Statinis privatus metodas. Grąžina eilutę su kairiuoju tarpų įtrauka, kad tekstas būtų centruotas `totalWidth` pločio stulpelyje.

### `int W`

Statinė savybė. Grąžina `max(80, Console.WindowWidth)` – minimalus plotis 80 simbolių.
