# 17. out argumentai — 1 t.

## Reikalavimas

Realizuota inicializacija naudojant `out` argumentus.

## Implementacija

### `out` naudojimas su `TryGetValue`

**Failas:** `PokemonGame.Core/Pokemon.cs`

```csharp
Moves = SpeciesMoves.TryGetValue(name, out var moves)
    ? moves
    : new List<Move> { new Move("Tackle", 40) };
```

`Dictionary.TryGetValue` grąžina `bool` ir per `out` parametrą pateikia rastą reikšmę. Tai leidžia vienu iškvietimu patikrinti buvimą ir gauti reikšmę.

### `out` naudojimas `Deconstruct`

**Failas:** `PokemonGame.Core/Pokemon.cs`

```csharp
public void Deconstruct(out string name, out int hp, out int maxHp)
{
    name  = Name;
    hp    = Hp;
    maxHp = MaxHp;
}
```

`out` parametrai privalo būti priskirti metodo viduje. Tai garantuoja, kad iškviečiantis kodas visada gaus inicializuotas reikšmes.

## Nauda

`out` parametrai leidžia metodui grąžinti kelias reikšmes nesinaudojant `Tuple` ar papildomomis klasėmis. `TryGet*` šablonas yra paplitęs .NET bibliotekose kaip alternatyva išimtims.
