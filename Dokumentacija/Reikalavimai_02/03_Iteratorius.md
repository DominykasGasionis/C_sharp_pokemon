# 3. Iteratorius — 0.5 t.

## Reikalavimas

Sukūrėte iteratorių.

## Implementacija

**Failas:** `PokemonGame/PokemonRoster.cs`

`GetAlive()` metodas naudoja `yield return` — C# iteratoriaus sintaksę. Metodas grąžina gyvus party narius po vieną, nekurdamas tarpinio sąrašo:

```csharp
// Iteratorius su yield return – grąžina tik gyvus Party narius po vieną
public IEnumerable<Pokemon> GetAlive()
{
    foreach (var p in Party)
        if (p?.IsAlive == true)
            yield return p;
}
```

## Kur naudojama

Naudojama vietose kur reikia tik gyvų party narių sekos, pvz. patikrinti ar lieka kas kovoti.
