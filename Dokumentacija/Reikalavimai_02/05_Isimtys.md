# 5. Savos išimčių klasės — 1 t.

## Reikalavimas

Sukūrėte ir panaudojote savo išimties tipus.

## Implementacija

**Failas:** `PokemonGame/Exceptions.cs`

Sukurtos trys išimčių klasės, paveldėjusios `Exception`:

```csharp
// Metama kai bandoma naudoti Pokemon, kuris jau krito
public class PokemonFaintedException(string pokemonName)
    : Exception($"{pokemonName} krito ir negali kovoti!")
{
    public string PokemonName { get; } = pokemonName;
}

// Metama kai roster'yje nėra nė vieno gyvo Pokemon
public class NoPokemonAvailableException()
    : Exception("Nėra gyvų Pokemon, kurie galėtų kovoti!")
{ }

// Metama kai judėjimo galia yra neteisinga
public class InvalidMoveException(string moveName)
    : Exception($"Judėjimas '{moveName}' negalioja arba neegzistuoja.")
{
    public string MoveName { get; } = moveName;
}
```

## Kur naudojama

`Battle.cs` konstruktoriuje ir mūšio cikle:

```csharp
_active = roster.ActivePokemon ?? throw new NoPokemonAvailableException();
```

```csharp
if (_active.IsFainted()) throw new PokemonFaintedException(_active.Name);
if (move.Power <= 0)    throw new InvalidMoveException(move.Name);
```
