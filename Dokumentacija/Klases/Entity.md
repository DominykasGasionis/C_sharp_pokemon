# Entity

**Projektas:** `PokemonGame.Core`
**Failas:** `Entity.cs`
**Tipas:** abstrakti klasė

## Paskirtis

Bazinė abstrakti klasė visoms žaidimo esybėms. Apibrėžia bendrą kontraktą – kiekviena esybė turi mokėti grąžinti savo rodomą vardą.

## Kodas

```csharp
public abstract class Entity
{
    public abstract string GetDisplayName();
}
```

## Metodai

| Metodas | Grąžina | Aprašas |
|---|---|---|
| `GetDisplayName()` | `string` | Abstraktus – kiekviena poklasė privalo įgyvendinti ir grąžinti rodomą pavadinimą |

## Paveldėtojai

| Klasė | `GetDisplayName()` grąžina | Pavyzdys |
|---|---|---|
| `Pokemon` | `"{Name} Lv{Level}"` | `"Pikachu Lv7"` |
| `Player` | `"@"` | Žaidėjo simbolis žemėlapyje |

## Architektūrinė pastaba

Naudoja **abstraktų metodą** (Template Method principas) – priverčia visas poklases aprašyti, kaip jos turi būti rodomos. Tai užtikrina, kad bet koks `Entity` objektas visada turės vardą, kurį galima parodyti ekrane.
