# 6. Range tipas — 0.5 t.

## Reikalavimas

Naudojate `Range` tipą.

## Implementacija

**Failas:** `PokemonGame/Battle.cs`

```csharp
// Range [^n..] – paskutinės iki 3 žurnalo žinučių
string[] recent = _log.ToArray()[^Math.Min(_log.Count, 3)..];
line1 = recent.Length > 2 ? $"  {recent[0]}"                : "";
line2 = recent.Length > 1 ? $"  {recent[recent.Length - 2]}" : "";
line3 = recent.Length > 0 ? $"  {recent[^1]}"               : "";
```

## Paaiškinimas

`[^n..]` yra `Range` reikšmė, sudaryta iš dviejų `Index` reikšmių:
- `^n` – `System.Index` nuo galo
- `..` – `System.Range` operatorius (nuo `^n` iki pabaigos)

Rezultatas: masyvas su paskutinėmis iki 3 žurnalo žinutėmis, kurios rodomos kovos ekrano kairėje pusėje.

## Skirtumas nuo Index

`_log[^1]` naudoja `System.Index` (vienas elementas), o `_log[^3..]` naudoja `System.Range` (poaibis).
