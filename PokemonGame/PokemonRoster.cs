namespace PokemonGame;

public class PokemonRoster
{
    public List<Pokemon> All  { get; } = new();
    public Pokemon?[] Party   { get; } = new Pokemon?[3];

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
        foreach (var p in Party) p?.HealFull();
    }

    public int[] GetPartyIndices() =>
        Party.Select(p => p is null ? -1 : All.IndexOf(p)).ToArray();
}
