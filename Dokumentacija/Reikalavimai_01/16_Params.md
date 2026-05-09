# 16. params raktažodis — 0.5 t.

## Reikalavimas

Naudojamas raktažodis `params`.

## Implementacija

**Failas:** `PokemonGame/Game.cs`

```csharp
private static void HRow(ScreenBuffer buf, string pad, int inner, int rightPad,
    params (ConsoleColor Color, string Text)[] segments)
{
    buf.Set(ConsoleColor.DarkCyan, ConsoleColor.Black);
    buf.Write(pad + "║");

    int written = 0;
    foreach (var (color, text) in segments)
    {
        buf.SetFg(color);
        buf.Write(text);
        written += text.Length;
    }
    // ...
}
```

## Kur naudojama

`HRow` kviečiama su kintamu segmentų skaičiumi, kiekvienas segmentas yra `(spalva, tekstas)` pora:

```csharp
HRow(_buf, hpad, hudInner, hudRightPad,
    (ConsoleColor.DarkCyan, ""),
    (ConsoleColor.DarkGray, $" [{i + 1}]"),
    (nameColor,             $"{marker} {p.Name,-10}"),
    (ConsoleColor.DarkGray, $" Lv{p.Level,-2}"),
    (ConsoleColor.DarkGray, "  HP "),
    (hpColor,               hpBar),
    (ConsoleColor.DarkGray, $" {hpFrac,-9} "));
```

## Nauda

`params` leidžia perduoti bet kiek argumentų neapgaubiant jų masyvu. Tai leidžia lanksčiai konstruoti spalvotas HUD eilutes su skirtingu segmentų skaičiumi kiekvienai eilutei.
