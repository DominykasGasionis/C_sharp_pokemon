namespace PokemonGame;

// IEnumerator<T> implementacija – leidžia rankiniu būdu eiti per visus Pokemon sąraše
public class PokemonRosterEnumerator : IEnumerator<Pokemon>
{
    private readonly List<Pokemon> _all;
    private int _index = -1; // pradinis indeksas prieš pirmą elementą

    public PokemonRosterEnumerator(List<Pokemon> all) => _all = all;

    // Grąžina dabartinį elementą pagal indeksą
    public Pokemon Current => _all[_index];
    object System.Collections.IEnumerator.Current => Current;

    // Pereina prie kito elemento; grąžina false kai elementų nebėra
    public bool MoveNext()
    {
        _index++;
        return _index < _all.Count;
    }

    public void Reset() => _index = -1;
    public void Dispose() { }
}

// IEnumerable<T> implementacija – leidžia naudoti foreach su PokemonRoster
public class PokemonRoster : IEnumerable<Pokemon>
{
    // Visi sugauti Pokemon (party + dėžutė)
    public List<Pokemon> All  { get; } = new();
    // Aktyvūs party nariai (max 3 vietos, gali būti null)
    public Pokemon?[] Party   { get; } = new Pokemon?[3];

    // Pirmas gyvas party narys; null jei visi krito
    public Pokemon? ActivePokemon => Array.Find(Party, p => p?.IsAlive == true);

    // New game – start with one Pokemon
    public PokemonRoster(Pokemon starter)
    {
        All.Add(starter);
        Party[0] = starter;
    }

    // Restore from save
    public PokemonRoster(List<Pokemon> all, int[] partyIndices)
    {
        All.AddRange(all);
        for (int i = 0; i < Math.Min(3, partyIndices.Length); i++)
        {
            int idx = partyIndices[i];
            if (idx >= 0 && idx < All.Count)
                Party[i] = All[idx];
        }
    }

    // Add a caught Pokemon; auto-fills first empty party slot
    public void Catch(Pokemon p)
    {
        All.Add(p);
        for (int i = 0; i < 3; i++)
        {
            if (Party[i] == null) { Party[i] = p; return; }
        }
        // Party full – Pokemon goes to box only
    }

    public void HealParty()
    {
        foreach (IHealable? p in Party) p?.HealFull();
    }

    // LINQ Select: kiekvieną party Pokemon paverčia jo indeksu All sąraše (-1 jei tuščia)
    public int[] GetPartyIndices() =>
        Party.Select(p => p is null ? -1 : All.IndexOf(p)).ToArray();

    // GetEnumerator grąžina mūsų PokemonRosterEnumerator, kuris įgyvendina IEnumerator<T>
    public IEnumerator<Pokemon> GetEnumerator() => new PokemonRosterEnumerator(All);
    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => GetEnumerator();

    // Iteratorius su yield return – grąžina tik gyvus Party narius po vieną
    public IEnumerable<Pokemon> GetAlive()
    {
        foreach (var p in Party)
            if (p?.IsAlive == true)
                yield return p;
    }
}
