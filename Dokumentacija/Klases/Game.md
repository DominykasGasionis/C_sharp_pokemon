# Game

**Projektas:** `PokemonGame`
**Failas:** `Game.cs`

## Paskirtis

Pagrindinis žaidimo ciklo valdiklis. Valdo žemėlapio atvaizdavimą, žaidėjo judėjimą, susidūrimų su Pokemon trigerius, Pokemon centro gydymą ir žaidimo išsaugojimą.

## Lauko kintamieji

| Kintamasis | Tipas | Aprašas |
|---|---|---|
| `_map` | `Map` | Žemėlapio plytelių masyvas |
| `_player` | `Player` | Žaidėjo pozicija |
| `_roster` | `PokemonRoster` | Žaidėjo Pokemon kolekcija |
| `_rng` | `Random` | Atsitiktinumo generatorius (susidūrimams) |
| `_settings` | `GameSettings` | Nustatymai (susidūrimų dažnis) |
| `_buf` | `ScreenBuffer` | Ekrano buferis be mirksėjimo atvaizdavimui |
| `_inventory` | `Inventory` | Žaidėjo daiktai |
| `_statusMessage` | `string` | Žinutė rodoma HUD statusų eilutėje |

---

## Metodai

### `Game(GameSettings, PokemonRoster, int, int, Inventory?)` – konstruktorius

Inicializuoja žaidimą. Sukuria naują `Map` ir `Player` objektus. Jei `inventory` nenurodytas – sukuriamas naujas su numatytomis reikšmėmis (5 Pokeballs, 3 Potions).

```csharp
public Game(GameSettings settings, PokemonRoster roster,
            int startX = 1, int startY = 1, Inventory? inventory = null)
```

---

### `void Run()`

**Pagrindinis žaidimo ciklas.** Vykdo `while(true)` ciklą kol žaidėjas išeina.

Kiekviena iteracija:
1. `Render()` – piešia žemėlapį ir HUD
2. `Console.ReadKey(intercept: true)` – laukia klavišo
3. `Esc`/`Q` → `SaveSystem.Save(...)`, nustato žinutę, `break`
4. `I` → `new PokemonMenu(_roster).Run()`, `continue`
5. Kiti klavišai → `HandleInput(key)`

Po ciklo: `Console.CursorVisible = true`, `Console.ResetColor()`, `Console.Clear()`.

---

### `void HandleInput(ConsoleKey key)`

Privatus metodas. Apdoroja judėjimo klavišą ir vykdo aplinkos trigerius.

**Judėjimo atvaizdavimas:**

| Klavišas | `dx` | `dy` |
|---|---|---|
| `W` / `↑` | 0 | −1 |
| `S` / `↓` | 0 | +1 |
| `A` / `←` | −1 | 0 |
| `D` / `→` | +1 | 0 |
| Kiti | — | grąžina iš karto |

Po `_player.TryMove(dx, dy, _map)`:
- **Judėjimas pavyko:**
  - `HealCenter` → `TriggerHeal()`
  - `TallGrass` ir `rng.Next(100) < EncounterChance` → `TriggerBattle()`
  - Kitos plytelės → nustato informacinę `_statusMessage` pagal plytelės tipą
- **Judėjimas nepavyko:** `_statusMessage = "Negalima eiti ten!"`

---

### `void TriggerHeal()`

Privatus metodas. Vykdomas įžengiant ant `HealCenter` plytelės.

```
Ar visi Pokemon pilni HP?
├─ Taip  → _statusMessage = "...visi Pokemon visiškai sveiki!"
└─ Ne    → _roster.HealParty()
           _inventory = _inventory + new Inventory { Pokeballs=1, Potions=1 }
           _statusMessage = "...pasveiko! Gauta +1 Pokeball, +1 Potion."
```

---

### `void TriggerBattle()`

Privatus metodas. Vykdomas kai atsitiktinis skaičius patenka į susidūrimo dažnio ribą ant `TallGrass`.

```
ActivePokemon == null (visi krito)?
├─ Taip  → HealParty(), "Atsigavote Pokemon centre."
└─ Ne    → Pokemon.RandomWild(rng) → sukuria laukinį
           ClearScreen()
           new Battle(roster, wild, rng, inventory).Run() → result
           _statusMessage pagal BattleResult
           Jei PlayerLost → HealParty()
```

---

### `void ClearScreen()`

Statinis privatus metodas. Išvalo ekraną prieš paleidžiant kovą – užpildo visą terminalą tarpais ir perkelia kursorių į (0,0). Naudojama vietoj `Console.Clear()`, nes tai neperloja senų eilučių iš žemėlapio.

```csharp
private static void ClearScreen()
{
    int h = Math.Max(40, Console.WindowHeight);
    int w = Math.Max(82, Console.WindowWidth);
    Console.SetCursorPosition(0, 0);
    string blank = new string(' ', w);
    for (int i = 0; i < h - 1; i++) Console.WriteLine(blank);
    Console.Write(blank);
    Console.SetCursorPosition(0, 0);
}
```

---

### `void Render()`

Privatus metodas. **Piešia visą ekraną** (žemėlapį + HUD) per `_buf` ir iškviečia `_buf.Flush()`.

**Piešimo seka:**
1. Apskaičiuoja `leftPad` ir `rightPad` horizontaliam centravimui
2. Apskaičiuoja `topMargin` vertikaliam centravimui
3. Rašo tuščias eilutes iki `topMargin`
4. `_map.Render(_buf, playerX, playerY, leftPad, rightPad)` – žemėlapis
5. **HUD** (to paties pločio kaip žemėlapis):
   - Viršutinė rėmelio linija `╔═╗`
   - Po eilutę kiekvienam **ne tuščiam** partijos lizui (su HP juosta ir spalva)
   - Skyriklis `╠─╣`
   - Statuso žinutės eilutė
   - Pozicijos (`X`,`Y`) ir vietovės pavadinimo eilutė
   - Skyriklis `╠─╣`
   - Klavišų pagalbos eilutė
   - Apatinė rėmelio linija `╚═╝`
6. Užpildo likusias ekrano eilutes tarpais (seni kadrai)
7. `_buf.Flush()`

---

### `void Hline(ScreenBuffer, string pad, int inner, char l, char fill, char r, int rightPad)`

Statinis privatus pagalbinis metodas. Piešia **horizontalią rėmelio liniją** (pvz. `╔═══╗` arba `╠───╣`). Simboliai `l` (kairys), `fill` (vidurys kartojamas `inner` kartų), `r` (dešinys) perduodami kaip parametrai – tai leidžia piešti ir `═` ir `─` tipo linijas.

---

### `void HRow(ScreenBuffer, string pad, int inner, int rightPad, params (ConsoleColor, string)[])`

Statinis privatus pagalbinis metodas. Piešia **turinio eilutę** su `║` kraštais. Priima kintamo ilgio spalvotų segmentų masyvą – kiekvienas segmentas yra `(spalva, tekstas)` pora. Po visų segmentų automatiškai papildo likusį plotą iki `inner` juodais tarpais, kad eilutė visada būtų vienodo pločio.

```csharp
// Naudojimo pavyzdys:
HRow(_buf, hpad, hudInner, hudRightPad, (ConsoleColor.DarkCyan, ""),
    (ConsoleColor.DarkGray, $" [{i+1}]"),
    (nameColor,             $"☻ {name,-10}"),
    (hpColor,               hpBar),
    (ConsoleColor.DarkGray, $" {hpFrac}"));
```

---

## HUD struktūra

```
╔══════════════════════════════════════════════╗
║ [1]☻ Swalot      Lv8  HP [████████████░░░░] 98/100  ║
║ [2]· Pikachu     Lv5  HP [████████████████] 35/35   ║
╠──────────────────────────────────────────────╣
║ Statusas žinutė                               ║
║ ( 5, 8)  Vietovė: Aukšta žolė                ║
╠──────────────────────────────────────────────╣
║ [WASD/↑↓←→] Judėti  [I] Pokemon  [Q/Esc] Išsaugoti ║
╚══════════════════════════════════════════════╝
```

- `☻` – aktyvus Pokemon; `·` – kitas partijos narys
- HP spalva: žalia `> 50%`, geltona `> 25%`, raudona `≤ 25%`
- Rodomos tik **užimtos** partijos vietos
