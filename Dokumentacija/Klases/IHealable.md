# IHealable

**Projektas:** `PokemonGame.Core`
**Failas:** `IHealable.cs`
**Tipas:** sąsaja (interface)

## Paskirtis

Apibrėžia kontraktą objektams, kuriuos galima gydyti. Leidžia rašyti kodą, kuris veikia su bet kokiu gydomu objektu nepriklausomai nuo konkretaus tipo.

## Kodas

```csharp
public interface IHealable
{
    int  Hp    { get; }
    int  MaxHp { get; }
    int  Heal(int amount);
    void HealFull();
}
```

## Nariai

| Narys | Tipas | Aprašas |
|---|---|---|
| `Hp` | `int` (savybė) | Dabartiniai gyvybės taškai (tik skaitymas) |
| `MaxHp` | `int` (savybė) | Maksimalūs gyvybės taškai (tik skaitymas) |
| `Heal(int amount)` | `int` | Gydo nurodytą kiekį HP, grąžina faktiškai atgautus HP |
| `HealFull()` | `void` | Pilnai atgauna HP iki `MaxHp` |

## Naudojimo pavyzdys

`PokemonRoster.HealParty()` naudoja `IHealable` tipą vietoj `Pokemon` – tai atsiejia gydymo logiką nuo konkretaus tipo:

```csharp
public void HealParty()
{
    foreach (IHealable? p in Party)
        p?.HealFull();
}
```

## Implementuoja

- `Pokemon` – vienintelis projekto objektas, kuris įgyvendina šią sąsają.

## Architektūrinė pastaba

`Heal(int amount)` grąžina **faktiškai atgautus HP** (ne prašytą kiekį). Tai leidžia žinoti tikslų gydymo rezultatą, pvz. kovos žurnale parodyti kiek HP iš tikrųjų atgavo Pokemon.
