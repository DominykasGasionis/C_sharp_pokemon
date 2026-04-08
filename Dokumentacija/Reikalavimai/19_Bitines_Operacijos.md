# 19. Bitinės operacijos — 1 t.

## Reikalavimas

Naudojamos bitinės operacijos.

## Implementacija

### `[Flags]` enum

**Failas:** `PokemonGame.Core/Pokemon.cs`

```csharp
[Flags]
public enum StatusEffect
{
    None      = 0,
    Poisoned  = 1 << 0, // 1  – bitinė kairiojo postūmio operacija
    Paralyzed = 1 << 1, // 2  – bitinė kairiojo postūmio operacija
}
```

### Statusų metodai

```csharp
public void ApplyStatus(StatusEffect effect)  => Status |= effect;  // ARBA – prideda bitą
public void ClearStatus(StatusEffect effect)   => Status &= ~effect; // IR su NOT – pašalina bitą
public bool HasStatus(StatusEffect effect)     => (Status & effect) != 0; // IR – tikrina bitą
```

### Naudojimas kovoje

**Failas:** `PokemonGame/Battle.cs`

```csharp
// Tikrinimas
if (_active.HasStatus(StatusEffect.Poisoned)) { ... }

// Pridėjimas
_active.ApplyStatus(StatusEffect.Poisoned);
_wild.ApplyStatus(StatusEffect.Paralyzed);
```

## Paaiškinimas

`[Flags]` enum leidžia saugoti kelis statusus viename `int` lauke kaip bitus. Pvz., jei Pokemon yra ir apnuodytas, ir paralyžiuotas – `Status = 0b11 = 3`. Bitinės operacijos leidžia efektyviai tikrinti, pridėti ir šalinti atskirus statusus nekeičiant kitų.

| Operacija | Simbolis | Veiksmas |
|---|---|---|
| Kairysis postūmis | `<<` | Sukuria bito maskę |
| Bitinis ARBA | `\|=` | Prideda statusą |
| Bitinis IR su NOT | `&= ~` | Pašalina statusą |
| Bitinis IR | `&` | Tikrina statusą |
