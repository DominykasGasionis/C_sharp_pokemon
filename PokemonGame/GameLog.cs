namespace PokemonGame;

// Bendrasis (generic) tipas su 'where T : class' apribojimu –
// universalus žurnalo konteineris su fiksuotu pajėgumu
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

    // Grąžina paskutinius 'count' įrašų nenukopijuojant viso sąrašo
    public IEnumerable<T> GetLast(int count) =>
        _entries.Skip(Math.Max(0, _entries.Count - count));
}
