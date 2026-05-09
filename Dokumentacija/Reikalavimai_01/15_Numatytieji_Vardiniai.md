# 15. Numatytieji ir vardiniai argumentai — 0.5 t.

## Reikalavimas

Naudojami numatyti ir vardiniai argumentai.

## Implementacija

### Numatytieji argumentai

**Failas:** `PokemonGame.Core/Pokemon.cs`

```csharp
public Pokemon(string name, int maxHp, int attack, int defense,
    int? currentHp = null,   // numatyta: null (naudojamas maxHp)
    int level      = 5,      // numatyta: 5
    int experience = 0,      // numatyta: 0
    int xpReward   = 0)      // numatyta: 0
```

**Failas:** `PokemonGame.Core/Pokemon.cs`

```csharp
public string HpBar(int barWidth = 20) { ... }
```

### Vardiniai argumentai

```csharp
// Pokemon.cs – RandomWild
return new Pokemon(name, scaledHp, scaledAtk, scaledDef,
    xpReward: xpReward,   // vardinis argumentas
    level: level);        // vardinis argumentas

// SaveSystem.cs
new Pokemon(e.Name, e.MaxHp, e.Attack, e.Defense, e.Hp, e.Level, e.Experience)
```

## Nauda

Numatytieji argumentai leidžia iškviesti konstruktorių ar metodą su mažiau parametrų. Vardiniai argumentai pagerina kodo skaitomumą ir leidžia praleisti tarpines reikšmes.
