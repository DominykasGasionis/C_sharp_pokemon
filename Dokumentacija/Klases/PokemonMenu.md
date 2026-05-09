# PokemonMenu

**Projektas:** `PokemonGame`
**Failas:** `PokemonMenu.cs`

## Paskirtis

Interaktyvus meniu Pokemon kolekcijos peržiūrai ir valdymui. Pasiekiamas paspaudus `I` žaidimo metu. Leidžia peržiūrėti partijos ir dėžės Pokemon bei keisti partijos sudėtį.

## Lauko kintamieji

| Kintamasis | Tipas | Aprašas |
|---|---|---|
| `_roster` | `PokemonRoster` | Žaidėjo Pokemon kolekcija |
| `_buf` | `ScreenBuffer` | Ekrano buferis be mirksėjimo atvaizdavimui |
| `_selected` | `int` | Pasirinkto elemento indeksas (0–2: partija, 3+: dėžė) |
| `_message` | `string` | Statusų žinutė rodoma meniu apačioje |
| `_pickingSlot` | `bool` | `true` kai laukiama pozicijos pasirinkimo (1–3) pilnai partijai |
| `_pendingPokemon` | `Pokemon?` | Dėžės Pokemon laukiantis ekipavimo kai partija pilna |
| `W` (konstanta) | `int` | Meniu vidinis plotis: 70 simbolių |

---

## Meniu struktūra

```
╔══════════════════════════════════════════════════════════════════════╗
║                      POKEMON VALDYMAS                                ║
╠──────────────────────────────────────────────────────────────────────╣
║  PARTIJA                                                             ║
║  [1] ► Swalot        Lv8   HP:  98/100   XP: 45/400 (liko 355)     ║
║  [2]   Pikachu       Lv5   HP:  35/35    XP: 0/250  (liko 250)      ║
║  [3]   (tuščia)                                                      ║
╠──────────────────────────────────────────────────────────────────────╣
║  DĖŽĖ                                                                ║
║      Rattata    Lv4   HP: 30/39   XP: 0/200 (liko 200)              ║
╠══════════════════════════════════════════════════════════════════════╣
║  Pasirinkite Pokemon.                                                ║
╠──────────────────────────────────────────────────────────────────────╣
║  [↑↓/WS] Naršyti   [Enter] Ekipuoti/Pašalinti   [Esc] Atgal        ║
╚══════════════════════════════════════════════════════════════════════╝
```

---

## Metodai

### `PokemonMenu(PokemonRoster roster)` – konstruktorius

Išsaugo `roster` nuorodą. Naudoja expression-body sintaksę:
```csharp
public PokemonMenu(PokemonRoster roster) => _roster = roster;
```

---

### `void Run()`

**Pagrindinis meniu ciklas.** Vykdomas kol žaidėjas paspaudžia `Esc`.

1. `ClearScreen()` – išvalo ekraną prieš pirmą atvaizdavimą
2. `while(true)`:
   - `Render()` – piešia meniu
   - `Console.ReadKey(intercept: true)` – laukia klavišo
   - Jei `_pickingSlot` aktyvus → `HandleSlotPick(key)`
   - Kitu atveju: `Esc` → `ClearScreen(); return`, kiti klavišai → `HandleNavigation(key)`

---

### `void HandleNavigation(ConsoleKey key)`

Privatus metodas. Apdoroja navigacijos klavišus kai **NE** `_pickingSlot` režimas.

```
Iš viso elementų: total = 3 (partija) + box.Count (dėžė)

W / ↑    → _selected = max(0, _selected - 1); _message = ""
S / ↓    → _selected = min(total - 1, _selected + 1); _message = ""
Enter    → HandleSelect(box)
Tarpas   → HandleSelect(box)
```

---

### `void HandleSelect(List<Pokemon> box)`

Privatus metodas. Vykdomas paspaudus `Enter`. Elgsena priklauso nuo `_selected` reikšmės.

**Jei `_selected < 3` (partijos Pokemon pasirinktas):**
```
Lizas tuščias?          → _message = "Šis lizdas tuščias."
Tik 1 Pokemon partijoje? → _message = "Negalima pašalinti paskutinio Pokemon!"
Kitu atveju:            → Party[_selected] = null
                          _message = "{vardas} pašalintas iš partijos."
```

**Jei `_selected >= 3` (dėžės Pokemon pasirinktas):**
```
Yra laisva vieta Party?  → Party[laisvaVieta] = pokemon
                           _message = "{vardas} ekipuotas į poziciją {n}."
Partija pilna?           → _pendingPokemon = pokemon
                           _pickingSlot = true
                           _message = "Partija pilna! [1][2][3] – kurią poziciją pakeisti? [Esc] atšaukti"
```

---

### `void HandleSlotPick(ConsoleKey key)`

Privatus metodas. Vykdomas kai `_pickingSlot == true` – žaidėjas pasirenka kurią partijos vietą pakeisti.

```
Esc           → _pickingSlot = false; _pendingPokemon = null; _message = "Atšaukta."
1 / NumPad1  → slot = 0
2 / NumPad2  → slot = 1
3 / NumPad3  → slot = 2
Kiti klavišai → nieko nedaro (grąžina)

Jei slot >= 0:
    Party[slot] = _pendingPokemon
    _message = "{vardas} ekipuotas į poziciją {slot+1}."
    _pickingSlot = false; _pendingPokemon = null
```

---

### `List<Pokemon> BoxPokemon()`

Privatus pagalbinis metodas. Grąžina **dėžės Pokemon sąrašą** – visus `All` Pokemon, kurių nėra `Party` masyve, surūšiuotus pagal lygį.

```csharp
private List<Pokemon> BoxPokemon()
{
    var box = _roster.All.Where(p => !_roster.Party.Contains(p)).ToList();
    box.Sort(); // naudoja Pokemon.CompareTo – rūšiuoja pagal lygį
    return box;
}
```

Rūšiavimas naudoja `Pokemon.CompareTo()` kuris lygina pagal `Level` – mažesnis lygis pirmiau.

---

### `void Render()`

Privatus metodas. **Piešia visą meniu ekraną** per `_buf` ir iškviečia `_buf.Flush()`.

Piešimo seka:
1. Apskaičiuoja centravimą (`pad`) ir inicializuoja `lineCount` skaitiklį
2. Antraštė su `POKEMON VALDYMAS`
3. `PARTIJA` sekcija – 3 eilutės per `PokemonRow()`:
   - Pasirinktas → žydra spalva
   - Miręs arba tuščias → tamsiai pilka
   - Gyvas → balta
4. Skyriklis
5. `DĖŽĖ` sekcija – dėžės Pokemon iš `BoxPokemon()`:
   - Jei tuščia → `"(dėžė tuščia)"`
   - Kitu atveju – to paties formato eilutės
6. Statuso žinutės eilutė (`_message`)
7. Klavišų pagalbos eilutė
8. Likusios eilutės užpildomos tarpais (seni kadrai)
9. `_buf.Flush()`

**Vietinės funkcijos `Render()` viduje:**

- **`HBorder(char l, char fill, char r)`** – piešia horizontalią rėmelio liniją; incrementina `lineCount`
- **`HRow(string text, ConsoleColor color)`** – piešia turinio eilutę su `║` kraštais; apkarpo tekstą jei per ilgas; incrementina `lineCount`
- **`PokemonRow(string slotTag, bool selected, Pokemon? poke)`** – grąžina formatuotą Pokemon eilutės tekstą:
  ```
  "  [1] ► Swalot        Lv8   HP:  98/100   XP: 45/400 (liko 355)"
  ```
  Naudoja `poke` dekonstrukciją: `var (pokeName, hp, maxHp) = poke`

---

### `void ClearScreen()`

Statinis privatus metodas. Išvalo ekraną užpildant visą terminalą tarpais ir perkeldamas kursorių į (0,0). Identiškas `Game.ClearScreen()` – naudojamas prieš atidarant meniu ir prieš grįžtant, kad neliktų žemėlapio artefaktų.

---

## Spalvų logika

| Situacija | Spalva |
|---|---|
| Pasirinktas Pokemon (`►`) | Žydra (`Cyan`) |
| Miręs Pokemon (`[KO]`) arba tuščias lizas | Tamsiai pilka (`DarkGray`) |
| Aktyvus (gyvas) Pokemon | Balta (`White`) |
| Sekcijų antraštės (`PARTIJA`, `DĖŽĖ`) | Tamsiai pilka (`DarkGray`) |
| Rėmelio simboliai | Tamsiai žydra (`DarkCyan`) |

## Navigacija

| Klavišas | Veiksmas |
|---|---|
| `W` / `↑` | Judėti aukštyn sąraše |
| `S` / `↓` | Judėti žemyn sąraše |
| `Enter` | Pašalinti iš partijos (partijos Pokemon) arba ekipuoti (dėžės Pokemon) |
| `1`–`3` | Pasirinkti poziciją kai partija pilna (`_pickingSlot` režimas) |
| `Esc` | Išeiti iš meniu |
