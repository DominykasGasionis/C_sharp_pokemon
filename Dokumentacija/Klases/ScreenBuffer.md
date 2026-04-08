# ScreenBuffer

**Projektas:** `PokemonGame`
**Failas:** `ScreenBuffer.cs`
**Modifikatorius:** `sealed`

## Paskirtis

Kaupiantis buferis, kuris surenka visą kadro tekstą (su ANSI spalvų kodais) į `StringBuilder` ir išveda jį vienu `Console.Write` kvietimu. Tai pašalina ekrano mirksėjimą, kuris atsirastų kviečiant `Console.Write` daug kartų per kadrą.

## Veikimo principas

1. Visos piešimo operacijos rašo į vidinį `StringBuilder`
2. `Flush()` iškviečia `Console.SetCursorPosition(0, 0)` ir vienu kvietimu išveda visą buferį
3. Spalvos perjungiamos ANSI escape kodais (`\x1B[31m` ir pan.) – tai efektyviau nei `Console.ForegroundColor`

## Metodai

| Metodas | Aprašas |
|---|---|
| `SetFg(ConsoleColor)` | Nustato priekinio plano spalvą (optimizuota – nerašo jei nesikeitė) |
| `SetBg(ConsoleColor)` | Nustato fono spalvą |
| `Set(ConsoleColor, ConsoleColor)` | Nustato abi spalvas |
| `Write(string)` / `Write(char)` | Prideda tekstą į buferį |
| `WriteLine(string)` | Prideda tekstą su eilutės laužimu |
| `Reset()` | Prideda ANSI reset kodą |
| `Flush()` | Išveda visą buferį į konsolę vienu rašymu |

## Spalvų optimizacija

`SetFg` ir `SetBg` saugo paskutinę nustatytą spalvą ir ANSI kodą rašo tik jei spalva pasikeitė. Tai sumažina buferio dydį ir pagreitina atvaizdavimą.
