# 9. Bendrasis plėtimo metodas — 1 t.

## Reikalavimas

Sukūrėte savo bendrąjį (generic) plėtimo metodą.

## Implementacija

**Failas:** `PokemonGame/Extensions.cs`

`FindStrongest<T>` yra generinis plėtimo metodas su `where T : Entity` apribojimu. Plečia bet kokį `IEnumerable<T>`, kurio elementai yra `Entity` paveldėtojai:

```csharp
// Generinis plėtimo metodas su 'where' apribojimu –
// veikia tik su Entity paveldėtojais, grąžina stipriausią pagal lygį
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

## Naudojimo pavyzdys

```csharp
// Randa stipriausią Pokemon iš viso roster'io
Pokemon? strongest = roster.FindStrongest();
```
