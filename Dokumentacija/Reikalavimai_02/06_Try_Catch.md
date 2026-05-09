# 6. try/catch blokai — 1 t.

## Reikalavimas

Yra blokai `try` `catch` vietose, kur gali įvykti klaida.

## Implementacija

### Battle.cs — mūšio atakos logika

**Failas:** `PokemonGame/Battle.cs`

Gaudome savas išimtis ir parodome žinutę žaidėjui, nenutraukdami programos:

```csharp
try
{
    if (_active.IsFainted()) throw new PokemonFaintedException(_active.Name);
    if (move.Power <= 0)    throw new InvalidMoveException(move.Name);
}
catch (PokemonFaintedException ex)
{
    AddLog(ex.Message);
    continue;
}
catch (InvalidMoveException ex)
{
    AddLog(ex.Message);
    continue;
}
```

### Game.cs — mūšio paleidimas

**Failas:** `PokemonGame/Game.cs`

Gaudome `NoPokemonAvailableException` jei roster'is tuščias:

```csharp
try
{
    var battle = new Battle(_roster, wild, _rng, _inventory);
    result = battle.Run();
}
catch (NoPokemonAvailableException ex)
{
    _roster.HealParty();
    _statusMessage = ex.Message + " Atsigavote Pokemon centre.";
    return;
}
```

### SaveSystem.cs — duomenų bazės operacijos

**Failas:** `PokemonGame/SaveSystem.cs`

Gaudome bet kokias duomenų bazės klaidas, kad žaidimas nesugriūtų:

```csharp
public static (...)? Load()
{
    try
    {
        EnsureDb();
        using var db = new GameDbContext();
        // ...
        return (roster, player, settings, inventory);
    }
    catch
    {
        return null;
    }
}
```
