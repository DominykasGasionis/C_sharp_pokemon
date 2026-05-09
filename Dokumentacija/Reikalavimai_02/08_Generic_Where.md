# 8. Bendrasis tipas su `where` raktažodžiu — 1 t.

## Reikalavimas

Pritaikėte savo bendrąjį (generic) tipą naudojant raktažodį `where`.

## Implementacija

**Failas:** `PokemonGame/GameLog.cs`

`GameLog<T>` naudoja `where T : class` apribojimą — tipas `T` turi būti referencinis tipas (ne `int`, `bool` ir pan.):

```csharp
public class GameLog<T> where T : class
```

**Failas:** `PokemonGame/Extensions.cs`

Generinis plėtimo metodas `FindStrongest<T>` su `where T : Entity` — metodas veikia tik su `Entity` paveldėtojais:

```csharp
public static T? FindStrongest<T>(this IEnumerable<T> source) where T : Entity
{
    T? best = null;
    foreach (var item in source)
    {
        if (item is Pokemon p && (best is not Pokemon bp || p.CompareTo(bp) > 0))
            best = item;
    }
    return best;
}
```

`where T : Entity` garantuoja, kad metodas bus iššauktas tik su `Pokemon`, `Player` ar kitais `Entity` paveldėtojais.
