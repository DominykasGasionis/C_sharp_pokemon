# 2. IEnumerator\<T\> implementacija — 1 t.

## Reikalavimas

Teisingai atlikote implementaciją `IEnumerator<T>`.

## Implementacija

**Failas:** `PokemonGame/PokemonRoster.cs`

Sukurta atskira `PokemonRosterEnumerator` klasė, implementuojanti `IEnumerator<Pokemon>`:

```csharp
public class PokemonRosterEnumerator : IEnumerator<Pokemon>
{
    private readonly List<Pokemon> _all;
    private int _index = -1; // pradinis indeksas prieš pirmą elementą

    public PokemonRosterEnumerator(List<Pokemon> all) => _all = all;

    public Pokemon Current => _all[_index];
    object System.Collections.IEnumerator.Current => Current;

    public bool MoveNext()
    {
        _index++;
        return _index < _all.Count;
    }

    public void Reset() => _index = -1;
    public void Dispose() { }
}
```

## Ryšys su IEnumerable\<T\>

`PokemonRoster.GetEnumerator()` grąžina šį enumeratorių:

```csharp
public IEnumerator<Pokemon> GetEnumerator() => new PokemonRosterEnumerator(All);
```
