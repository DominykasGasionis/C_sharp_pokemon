# ScreenBuffer

**Projektas:** `PokemonGame`
**Failas:** `ScreenBuffer.cs`
**Modifikatorius:** `sealed`

## Paskirtis

Kaupiantis buferis, kuris surenka visą kadro tekstą su ANSI spalvų kodais į `StringBuilder` ir išveda jį **vienu** `Console.Write` kvietimu. Tai pašalina ekrano mirksėjimą, kuris atsirastų kviečiant `Console.Write` daug kartų per kadrą.

## Veikimo principas

```
Piešimas → StringBuilder    →   Flush()   →   Console.Write (vienas kvietimas)
SetFg(), Write(), WriteLine()   visas kadras   ekranas atnaujinamas vienu metu
```

1. Visos piešimo operacijos rašo ANSI kodus ir tekstą į vidinį `StringBuilder`
2. `Flush()` iškviečia `Console.SetCursorPosition(0, 0)` ir vienu kvietimu išveda visą buferį
3. Spalvos valdomos ANSI escape kodais (`\x1B[31m` ir pan.) – tai efektyviau nei `Console.ForegroundColor`

## Metodai

| Metodas | Aprašas |
|---|---|
| `SetFg(ConsoleColor)` | Nustato priekinio plano spalvą (rašo ANSI kodą tik jei spalva pasikeitė) |
| `SetBg(ConsoleColor)` | Nustato fono spalvą (ta pati optimizacija) |
| `Set(ConsoleColor fg, ConsoleColor bg)` | Nustato abi spalvas vienu kvietimu |
| `Write(string)` / `Write(char)` | Prideda tekstą į buferį |
| `WriteLine(string)` | Prideda tekstą ir eilutės laužimą `\n`; resetuoja fono spalvos stebėjimą |
| `Reset()` | Prideda ANSI reset kodą `\x1B[0m` |
| `Flush()` | Perkelia kursorių į (0,0) ir vienu `Console.Write(_sb)` išveda visą kadrą; išvalo buferį |

## ANSI spalvų žemėlapis

```csharp
// Priekinio plano kodai (FgCodes)
"\x1B[30m"  // Black       = 0
"\x1B[34m"  // DarkBlue    = 1
"\x1B[32m"  // DarkGreen   = 2
"\x1B[36m"  // DarkCyan    = 3
// ... (16 spalvų iš viso)

// Fono kodai (BgCodes)
"\x1B[40m"  // Black = 0
"\x1B[44m"  // DarkBlue = 1
// ...
```

## Spalvų optimizacija

`SetFg` ir `SetBg` saugo paskutinę nustatytą spalvą:

```csharp
public void SetFg(ConsoleColor c)
{
    int i = (int)c;
    if (i != _lastFg) { _sb.Append(FgCodes[i]); _lastFg = i; }
    // ANSI kodas rašomas TIK jei spalva pasikeitė
}
```

Tai sumažina buferio dydį ir pagreitina atvaizdavimą, nes nereikia kaskart rašyti tos pačios spalvos kodo.

## Pastaba apie `\n` ir fono spalvą

`WriteLine()` po eilutės laužimo nustato `_lastBg = -1`, nes kai kurie terminalai automatiškai reseti'na fono spalvą einant į naują eilutę.
