namespace PokemonGame;

// Savo sukurta sąsaja (interface) – apibrėžia gydymo sutartį
// Implementuoja: Pokemon
public interface IHealable
{
    int  Hp    { get; }
    int  MaxHp { get; }
    int  Heal(int amount);
    void HealFull();
}
