# 18. Delegatai arba lambda funkcijos — 1.5 t.

## Reikalavimas

Naudojami delegatai arba lambda funkcijos.

## Implementacija

Lambda funkcijos naudojamos per visą projektą su LINQ metodais:

### `Where` – filtravimas

```csharp
// Game.cs
bool allHealthy = _roster.Party
    .Where(p => p != null)
    .All(p => p!.Hp == p.MaxHp);

// PokemonMenu.cs
_roster.All.Where(p => !_roster.Party.Contains(p)).ToList()
```

### `Select` – transformacija

```csharp
// SaveSystem.cs
AllPokemon = roster.All.Select(p => new PokemonSaveEntry
{
    Name       = p.Name,
    MaxHp      = p.MaxHp,
    Hp         = p.Hp,
    // ...
}).ToList()
```

### `All` – visuotinis tikrinimas

```csharp
// Game.cs
.All(p => p!.Hp == p.MaxHp)
```

### Lokalios lambda funkcijos

```csharp
// Battle.cs
string MC(int r, int c) => (_menuRow == r && _menuCol == c) ? ">" : " ";
string Clip(string s)   => s.Length > LeftW - 1 ? s[..(LeftW - 4)] + "..." : s;

// PokemonMenu.cs
void HBorder(char l, char fill, char r) { ... }
void HRow(string text, ConsoleColor color) { ... }
```

## Nauda

Lambda funkcijos leidžia perduoti elgesį kaip argumentą ir kurti trumpas vietines funkcijas. LINQ metodai su lambdomis leidžia deklaratyviai apdoroti kolekcijas.
