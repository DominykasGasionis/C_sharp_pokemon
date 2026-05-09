# 4. C# tipų plėtimas — 0.5 t.

## Reikalavimas

Praplėtėte C# tipus.

## Implementacija

**Failas:** `PokemonGame/Extensions.cs`

Sukurta statinė `Extensions` klasė su plėtimo metodais `Pokemon` tipui:

```csharp
public static class Extensions
{
    // Plėtimo metodas: patogus būdas patikrinti ar Pokemon nebegali kovoti
    public static bool IsFainted(this Pokemon p) => !p.IsAlive;

    // Plėtimo metodas: suformuoja statusų eilutę rodymui ekrane
    public static string ToStatusString(this Pokemon p)
    {
        if (p.Status == StatusEffect.None) return "";
        var parts = new List<string>();
        if (p.HasStatus(StatusEffect.Poisoned))  parts.Add("Apnuodytas");
        if (p.HasStatus(StatusEffect.Paralyzed)) parts.Add("Paralyžuotas");
        return $" [{string.Join(", ", parts)}]";
    }
}
```

## Kur naudojama

`Battle.cs` mūšio logikoje:

```csharp
if (_active.IsFainted()) throw new PokemonFaintedException(_active.Name);
```
