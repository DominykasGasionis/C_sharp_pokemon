# 9. Abstrakti klasė — 0.5 t.

## Reikalavimas

Naudojate abstrakčią klasę.

## Implementacija

**Failas:** `PokemonGame.Core/Entity.cs`

```csharp
public abstract class Entity
{
    public abstract string GetDisplayName();
}
```

## Paveldėtojai

### `Pokemon : Entity`

```csharp
public override string GetDisplayName() => $"{Name} Lv{Level}";
// Pvz.: "Swalot Lv8"
```

### `Player : Entity`

```csharp
public override string GetDisplayName() => "@";
// Žaidėjo simbolis žemėlapyje
```

## Paaiškinimas

`abstract class` skiriasi nuo `interface` tuo, kad gali turėti būseną ir neabstrakčius metodus. `Entity` naudojama kaip bendra bazė, leidžianti rašyti kodą, kuris dirba su bet kokia žaidimo esybe per `GetDisplayName()` metodą, nežinant konkretaus tipo.
