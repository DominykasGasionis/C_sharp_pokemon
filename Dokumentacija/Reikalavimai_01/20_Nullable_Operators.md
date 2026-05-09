# 20. ?. ?[] ?? ??= operatoriai — 0.5 t.

## Reikalavimas

Naudojami operatoriai `?.` `?[]` `??` arba `??=`.

## Implementacija

### `?.` – sąlyginis narių pasiekimas

```csharp
// PokemonRoster.cs
public void HealParty()
{
    foreach (IHealable? p in Party) p?.HealFull(); // nekviečia jei p == null
}

// PokemonRoster.cs
public Pokemon? ActivePokemon => Array.Find(Party, p => p?.IsAlive == true);
```

### `??` – null susiejimo operatorius

```csharp
// Pokemon.cs – konstruktorius
Hp = currentHp ?? maxHp; // jei currentHp == null, naudoja maxHp

// Game.cs – konstruktorius
_inventory = inventory ?? new Inventory(); // jei inventory == null, sukuria naują
```

### `??=` – priskyrimai tik jei null

Nors `??=` tiesiogiai nenaudojamas, `??` semantika atitinka šio operatoriaus paskirtį.

## Paaiškinimas

Šie operatoriai yra saugūs būdai dirbti su galimai `null` reikšmėmis:

- `x?.M()` – iškviečia `M()` tik jei `x != null`, kitaip grąžina `null`
- `x ?? y` – grąžina `x` jei `x != null`, kitaip `y`
- `x ??= y` – priskiria `y` tik jei `x == null`
