# 7. Savos bendrojo (generic) tipo kūrimas — 1 t.

## Reikalavimas

Sukūrėte savo bendrąjį (generic) tipą.

## Implementacija

**Failas:** `PokemonGame/GameLog.cs`

`GameLog<T>` — universalus žurnalo konteineris su fiksuotu pajėgumu. Parametras `T` leidžia naudoti klasę bet kokio tipo įrašams:

```csharp
// Bendrasis (generic) tipas su 'where T : class' apribojimu
public class GameLog<T> where T : class
{
    private readonly List<T> _entries = new();
    private readonly int     _capacity;

    public GameLog(int capacity = 100) => _capacity = capacity;

    public IReadOnlyList<T> Entries => _entries;
    public int Count => _entries.Count;

    // Prideda įrašą; jei viršijamas pajėgumas – ištrinamas seniausias
    public void Add(T entry)
    {
        if (_entries.Count >= _capacity)
            _entries.RemoveAt(0);
        _entries.Add(entry);
    }

    public void Clear() => _entries.Clear();

    public IEnumerable<T> GetLast(int count) =>
        _entries.Skip(Math.Max(0, _entries.Count - count));
}
```

## Kur naudojama

`Battle.cs` naudoja `GameLog<string>` mūšio pranešimams:

```csharp
private readonly GameLog<string> _log = new(50);
```
