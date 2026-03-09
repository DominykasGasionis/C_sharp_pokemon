namespace PokemonGame;

public class Move
{
    public string Name  { get; }
    public int    Power { get; }

    public Move(string name, int power)
    {
        Name  = name;
        Power = power;
    }
}
