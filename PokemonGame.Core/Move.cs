namespace PokemonGame;

public class Move : IEquatable<Move>
{
    public string Name  { get; }
    public int    Power { get; }

    public Move(string name, int power)
    {
        Name  = name;
        Power = power;
    }

    public bool Equals(Move? other) =>
        other is not null && Name == other.Name && Power == other.Power;

    public override bool Equals(object? obj) => Equals(obj as Move);

    public override int GetHashCode() => HashCode.Combine(Name, Power);
}
