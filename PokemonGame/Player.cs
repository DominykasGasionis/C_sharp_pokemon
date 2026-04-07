namespace PokemonGame;

public class Player : Entity
{
    public int X { get; private set; }
    public int Y { get; private set; }

    public Player(int startX, int startY)
    {
        X = startX;
        Y = startY;
    }

    public override string GetDisplayName() => "@";

    public bool TryMove(int dx, int dy, Map map)
    {
        int newX = X + dx;
        int newY = Y + dy;

        if (!map.IsPassable(newX, newY))
            return false;

        X = newX;
        Y = newY;
        return true;
    }
}
