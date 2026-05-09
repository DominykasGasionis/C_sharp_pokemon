namespace PokemonGame;

// IEquatable<T> implementacija – leidžia lyginti Move objektus pagal reikšmę
public class Move : IEquatable<Move>
{
    public string Name  { get; }
    public int    Power { get; }

    public Move(string name, int power)
    {
        Name  = name;
        Power = power;
    }

    // IEquatable<T>.Equals – lyginami abu laukai (vardas ir galia)
    public bool Equals(Move? other) =>
        other is not null && Name == other.Name && Power == other.Power; // 'is' operatorius

    public override bool Equals(object? obj) => Equals(obj as Move);

    public override int GetHashCode() => HashCode.Combine(Name, Power);
}
