# Battle

**Projektas:** `PokemonGame`
**Failas:** `Battle.cs`

## Paskirtis

Valdo vieną kovos sesiją tarp žaidėjo aktyvaus Pokemon ir laukinio. Piešia konsolinį kovos ekraną ir apdoroja visus žaidėjo veiksmus iki kol kova pasibaigia.

## Lauko kintamieji

| Kintamasis | Tipas | Aprašas |
|---|---|---|
| `_roster` | `PokemonRoster` | Žaidėjo Pokemon kolekcija |
| `_wild` | `Pokemon` | Laukinis Pokemon su kuriuo vyksta kova |
| `_rng` | `Random` | Atsitiktinumo generatorius |
| `_inventory` | `Inventory` | Žaidėjo inventorius (Potions, Pokeballs) |
| `_log` | `List<string>` | Kovos žurnalo žinutės (rodomos apatinėje dalyje) |
| `_buf` | `ScreenBuffer` | Ekrano buferis be mirksėjimo atvaizdavimui |
| `_active` | `Pokemon` | Šiuo metu kovoje esantis žaidėjo Pokemon |
| `_selectingAttack` | `bool` | `true` kai rodomas judesių pasirinkimo meniu |
| `_selectingPokemon` | `bool` | `true` kai rodomas partijos keitimo meniu |
| `_waitForKey` | `bool` | `true` kai laukiama klavišo paspaudimo (kova baigėsi) |
| `_menuRow`, `_menuCol` | `int` | Pagrindinio meniu žymeklio pozicija (eilutė 0–2, stulpelis 0–1) |
| `_moveRow`, `_moveCol` | `int` | Judesių meniu žymeklio pozicija |
| `_pokemonRow` | `int` | Partijos keitimo meniu žymeklio pozicija (0–2) |

## Kovos ekrano išdėstymas

```
╔═══════════════════════════════════════════════════════════════════════════════════╗
║  ~ Rattata         Lv4                                                           ║
║  HP [████████████░░░░░░░░░░░░░░░░░░░░]                                           ║
║                                                                                  ║
║                              * Swalot       Lv8                                  ║
║                              HP [████████████████░░] 98/100                     ║
╠══════════════════════════════════════════════╦═══════════════════════════════════╣
║  Prieš tai buvusi žinutė                     ║  >FIGHT          POKEMON          ║
║  Dabartinė žinutė                            ║   BAG(3)         POKEBALL(5)      ║
║                                              ║   RUN                             ║
╚══════════════════════════════════════════════╩═══════════════════════════════════╝
```

---

## Metodai

### `Battle(PokemonRoster, Pokemon, Random, Inventory)` – konstruktorius

Inicializuoja kovą. Išsaugo visus priklausomybes kaip `readonly` laukus. Nustato `_active = roster.ActivePokemon!` – pirmą gyvą partijos Pokemon, kuris pradeda kovoti.

---

### `BattleResult Run()`

**Pagrindinis kovos ciklas.** Grąžina `BattleResult` kai kova pasibaigia.

Kiekviena iteracija:
1. Iškviečia `RenderBattle()` – atnaujina ekraną
2. Tikrina aktyvų režimą (`_selectingAttack`, `_selectingPokemon` arba pagrindinis meniu)
3. Vykdo pasirinktą veiksmą
4. Po veiksmo: taiko nuodų žalą, laukinis atakuoja
5. Tikrina ar kas nors krito

**Kiekvieno veiksmo rezultatai:**

| Veiksmas | Logika |
|---|---|
| **FIGHT** | Nustato `_selectingAttack = true`, grįžta į ciklo pradžią |
| **POKEMON** | Nustato `_selectingPokemon = true`, grįžta į ciklo pradžią |
| **BAG** | Jei `Potions > 0`: `_active.Heal(20)`, `Potions--`; kitu atveju – klaidos žinutė |
| **POKEBALL** | Jei `Pokeballs > 0`: skaičiuoja pagavimo šansą, meta Pokeball; kitu atveju – klaidos žinutė |
| **RUN** | 60% šansas pabėgti; jei pavyksta – `return PlayerFled` |

**Laukinio atakos eiga (po kiekvieno veiksmo):**
1. Jei laukinis **paralyžiuotas** ir `rng < 30%` → praleido ataką
2. Kitu atveju: `damage = max(1, wild.Attack - active.Defense + rng(-5..+5))`
3. `15%` šansas apnuodyti aktyvų Pokemon (jei dar nenudytas)
4. `10%` šansas paralyžiuoti laukinį (jei dar neparalyžiuotas)

**Kovos pabaigos sąlygos:**

| Sąlyga | Grąžina |
|---|---|
| Laukinis `!IsAlive` po atakos | `PlayerWon` |
| Laukinis pagautas Pokeball | `PokemonCaught` |
| Žaidėjas pabėgo (60%) | `PlayerFled` |
| `_roster.ActivePokemon == null` (visi krito) | `PlayerLost` |

---

### `void AddLog(string msg)`

Privatus pagalbinis metodas. Prideda žinutę į `_log` sąrašą. Paskutinės 3 žinutės rodomos kovos ekrano apatiniame kairiajame skydelyje.

```csharp
private void AddLog(string msg) => _log.Add(msg);
```

---

### `void WaitKey()`

Nustato `_waitForKey = true`, iškviečia `RenderBattle()` (ekrane pasirodo `"Spausk bet kurį klavišą..."`), tada blokuoja su `Console.ReadKey(true)`. Naudojama prieš grąžinant kovos rezultatą, kad žaidėjas galėtų perskaityti paskutinę žinutę.

```csharp
private void WaitKey()
{
    _waitForKey = true;
    RenderBattle();
    _waitForKey = false;
    Console.ReadKey(true);
}
```

---

### `int BattlePad()`

Statinis pagalbinis metodas. Apskaičiuoja horizontalų centravimą: `max(0, (terminaloPlotics - kovosPlotis) / 2)`. Užtikrina, kad kovos ekranas būtų centre nepriklausomai nuo terminalo dydžio.

---

### `void RenderBattle()`

**Piešia visą kovos ekraną** į `_buf` (ScreenBuffer) ir iškviečia `_buf.Flush()`.

Piešimo tvarka:
1. Apskaičiuoja tarpą (`pad`) centravimui
2. Suformuoja **laukinio Pokemon** eilutes: vardas/lygis viršuje kairėje, HP juosta
3. Suformuoja **aktyvaus Pokemon** eilutes: vardas/lygis apačioje dešinėje, HP juosta su `HP/MaxHP`; jei statusas – rodo `[PSN]` arba `[PAR]`
4. Suformuoja **žinutės skydelį** (kairys apačia) – 3 eilutės:
   - `_waitForKey = true` → rodomas `"Spausk bet kurį klavišą..."`
   - `_selectingAttack` → `"Ko imsis {name}?"`
   - `_selectingPokemon` → partijos keitimo antraštė
   - Kitu atveju → paskutinės ≤3 `_log` žinutės
5. Suformuoja **meniu skydelį** (dešinys apačia):
   - `_selectingAttack` → judesių tinklelis per `MoveSlots()`
   - `_selectingPokemon` → partijos eilutės per `PartySlot()`
   - Kitu atveju → pagrindinis meniu su žymekliu `>`
6. Prideda viršutines ir apatines tušias eilutes vertikaliam centravimui
7. Iškviečia `_buf.Flush()`

---

### `void FR(string pStr, int fW, string content, ConsoleColor color)`

*(Field Row)* Piešia **vieną pilno pločio** kovos lauko eilutę. Rašo: `║` (tamsiai žydra), `content` (nurodytos `color` spalvos, papildyta tarpais iki `fW`), `║`.

---

### `void RowColored(string pad, string leftText, ConsoleColor lColor, string rightText, ConsoleColor rColor)`

Piešia **apatinio skydelio** eilutę su dviem skirtingų spalvų dalimis: `║` + kairysis tekstas + `║` + dešinysis tekstas + `║`. Naudojama žinutės ir meniu skydelių eilutėms.

---

### `string MoveSlots(IReadOnlyList<Move> m, int rowIdx)`

Grąžina formatuotą eilutę su **dviem judesių lizdais** (kairys ir dešinys) nurodytai eilutei. Žymeklis `>` rodomas aktyviam lizdui pagal `_moveRow` ir `_moveCol`. Jei judesio nėra – rodomas `---`.

```
 >Flamethrower  90    Scratch      40
```

---

### `ConsoleColor HpColor(Pokemon p)`

Statinis metodas. Grąžina spalvą pagal HP lygį:
- `Green` – `Hp > MaxHp / 2`
- `Yellow` – `Hp > MaxHp / 4`
- `Red` – `Hp <= MaxHp / 4`

---

### `string PartySlot(int idx)`

Grąžina formatuotą partijos nario eilutę indeksui `idx` (0–2). Rodo: žymeklį `>` jei pasirinktas, Pokemon vardą, lygį, HP. Papildomos žymės: `[kovoja]` aktyviam, `[KO]` mirusiam. Jei lizas tuščias – `(tuščia)`.

---

### `int PromptChoice()`

**Laukia žaidėjo įvesties** pagrindiniame meniu. Apdoroja klavišus:
- `WASD` / rodyklės → juda per `_menuRow`/`_menuCol`, iškviečia `RenderBattle()`
- `Enter`/`Tarpas` → grąžina `_menuRow * 2 + _menuCol + 1` (1–5)
- `1`–`5` → tiesioginis pasirinkimas

**Grąžina:** 1=FIGHT, 2=POKEMON, 3=BAG, 4=POKEBALL, 5=RUN

---

### `Pokemon? PromptPokemon()`

**Laukia žaidėjo įvesties** partijos keitimo meniu. Apdoroja:
- `Esc` → grąžina `null` (atšaukimas)
- `WS` / rodyklės → juda per `_pokemonRow` (0–2), iškviečia `RenderBattle()`
- `Enter`/`Tarpas` → grąžina `_roster.Party[_pokemonRow]`
- `1`–`3` → tiesioginis pasirinkimas pagal eilę

**Grąžina:** pasirinktą `Pokemon` arba `null` jei atšaukta.

---

### `Move? PromptMove()`

**Laukia žaidėjo įvesties** judesių pasirinkimo meniu. Apdoroja:
- `Esc` → grąžina `null` (atšaukimas, grįžtama į pagrindinį meniu)
- `WASD` / rodyklės → juda per `_moveRow`/`_moveCol` (2×2 tinklelis), iškviečia `RenderBattle()`
- `Enter`/`Tarpas` → grąžina `moves[_moveRow * 2 + _moveCol]` jei toks egzistuoja
- `1`–`4` → tiesioginis pasirinkimas pagal judesio eilės numerį

**Grąžina:** pasirinktą `Move` arba `null` jei atšaukta.

---

## Rezultatai (`BattleResult` enum)

| Reikšmė | Situacija |
|---|---|
| `PlayerWon` | Laukinis nukovotas |
| `PlayerFled` | Žaidėjas pabėgo (60% šansas) |
| `PlayerLost` | Visi žaidėjo Pokemon krito |
| `PokemonCaught` | Laukinis pagautas Pokeball |
