namespace PokemonGame;

public interface IHealable
{
    int  Hp    { get; }
    int  MaxHp { get; }
    int  Heal(int amount);
    void HealFull();
}
