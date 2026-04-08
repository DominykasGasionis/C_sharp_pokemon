# IHealable

**Projektas:** `PokemonGame.Core`
**Failas:** `IHealable.cs`
**Tipas:** sąsaja (interface)

## Paskirtis

Apibrėžia kontraktą objektams, kuriuos galima gydyti. Leidžia rašyti kodą, kuris dirba su bet kokiu gydomu objektu nepriklausomai nuo konkretaus tipo.

## Nariai

| Narys | Tipas | Aprašas |
|---|---|---|
| `Hp` | `int` (savybė) | Dabartiniai gyvybės taškai |
| `MaxHp` | `int` (savybė) | Maksimalūs gyvybės taškai |
| `Heal(int amount)` | `int` | Gydo nurodytą kiekį HP, grąžina faktiškai atgautus HP |
| `HealFull()` | `void` | Pilnai atgauna HP iki `MaxHp` |

## Naudojimas

`PokemonRoster.HealParty()` naudoja `IHealable` tipą vietoj `Pokemon`, taip atsieja gydymo logiką nuo konkretaus tipo:

```csharp
foreach (IHealable? p in Party)
    p?.HealFull();
```

## Implementuoja

- `Pokemon`
