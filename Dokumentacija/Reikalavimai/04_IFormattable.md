# 4. IFormattable — 1 t.

## Reikalavimas

Teisingai atlikote implementaciją `IFormattable`.

## Implementacija

**Failas:** `PokemonGame.Core/Pokemon.cs`

```csharp
public class Pokemon : Entity, IComparable<Pokemon>, IFormattable, IHealable
{
    public string ToString(string? format, IFormatProvider? _ = null) => format switch
    {
        "S" => $"{Name} Lv{Level}",
        "L" => $"{Name} Lv{Level} HP:{Hp}/{MaxHp}{(Status != StatusEffect.None ? $" [{Status}]" : "")}",
        _   => Name,
    };
}
```

## Formatų reikšmės

| Formatas | Grąžina | Pavyzdys |
|---|---|---|
| `"S"` | Trumpas | `"Swalot Lv8"` |
| `"L"` | Ilgas su HP ir statusu | `"Swalot Lv8 HP:98/100 [Poisoned]"` |
| `null` arba kitas | Numatytasis | `"Swalot"` |

## Naudojimas

```csharp
Pokemon p = ...;
Console.WriteLine(p.ToString("S"));           // "Swalot Lv8"
Console.WriteLine(p.ToString("L"));           // "Swalot Lv8 HP:98/100"
Console.WriteLine($"{p:S}");                  // "Swalot Lv8" (interpoliacija)
```
