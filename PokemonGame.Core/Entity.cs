namespace PokemonGame;

// Abstrakti klasė – negali būti tiesiogiai sukurta, tik paveldima
// Paveldėtojai: Pokemon, Player
public abstract class Entity
{
    public abstract string GetDisplayName();
}
