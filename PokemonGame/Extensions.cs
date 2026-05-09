namespace PokemonGame;

// Statinė klasė su C# tipo plėtimo metodais (extension methods)
public static class Extensions
{
    // Generinis plėtimo metodas su 'where' apribojimu –
    // veikia tik su Entity paveldėtojais, grąžina stipriausią pagal lygį
    public static T? FindStrongest<T>(this IEnumerable<T> source) where T : Entity
    {
        T? best = null;
        foreach (var item in source)
        {
            if (item is Pokemon p && (best is not Pokemon bp || p.CompareTo(bp) > 0))
                best = item;
        }
        return best;
    }

    // Plėtimo metodas: patogus būdas patikrinti ar Pokemon nebegali kovoti
    public static bool IsFainted(this Pokemon p) => !p.IsAlive;

    // Plėtimo metodas: suformuoja statusų eilutę rodymui ekrane
    public static string ToStatusString(this Pokemon p)
    {
        if (p.Status == StatusEffect.None) return "";
        var parts = new List<string>();
        if (p.HasStatus(StatusEffect.Poisoned))  parts.Add("Apnuodytas");
        if (p.HasStatus(StatusEffect.Paralyzed)) parts.Add("Paralyžuotas");
        return $" [{string.Join(", ", parts)}]";
    }

    // Plėtimo dekonstruktorius – leidžia naudoti var (name, power) = move;
    public static void Deconstruct(this Move m, out string name, out int power)
    {
        name  = m.Name;
        power = m.Power;
    }
}
