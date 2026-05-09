namespace PokemonGame;

public class Inventory
{
    public int Pokeballs { get; set; } = 5;
    public int Potions   { get; set; } = 3;

    // Operatorių perkrovimas – leidžia sudėti du Inventory objektus su +
    public static Inventory operator +(Inventory a, Inventory b) =>
        new Inventory { Pokeballs = a.Pokeballs + b.Pokeballs, Potions = a.Potions + b.Potions };
}
