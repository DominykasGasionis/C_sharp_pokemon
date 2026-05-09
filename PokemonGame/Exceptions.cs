namespace PokemonGame;

// Savos išimčių klasės – leidžia tiksliau identifikuoti klaidos priežastį mūšio metu

// Metama kai bandoma naudoti Pokemon, kuris jau krito
public class PokemonFaintedException(string pokemonName)
    : Exception($"{pokemonName} krito ir negali kovoti!")
{
    public string PokemonName { get; } = pokemonName;
}

// Metama kai roster'yje nėra nė vieno gyvo Pokemon (pvz. konstruktoriuje)
public class NoPokemonAvailableException()
    : Exception("Nėra gyvų Pokemon, kurie galėtų kovoti!")
{ }

// Metama kai judėjimo galia yra neteisinga (pvz. 0 ar neigiama)
public class InvalidMoveException(string moveName)
    : Exception($"Judėjimas '{moveName}' negalioja arba neegzistuoja.")
{
    public string MoveName { get; } = moveName;
}
