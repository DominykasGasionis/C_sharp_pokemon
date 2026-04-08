# 8. sealed arba partial klasė — 0.5 t.

## Reikalavimas

Naudojate uždarytą (`sealed`) arba dalinę (`partial`) klasę.

## Implementacija

**Failas:** `PokemonGame/ScreenBuffer.cs`

```csharp
public sealed class ScreenBuffer
{
    // ...
}
```

## Paaiškinimas

`sealed` raktažodis draudžia paveldėti iš šios klasės. `ScreenBuffer` yra tinkamas kandidatas dėl kelių priežasčių:

- Klasė yra žemo lygio infrastruktūros komponentas – jos elgesio keisti nereikia
- Paveldėjimas galėtų sulaužyti vidinę buferizavimo logiką (spalvų optimizacija saugo `_lastFg`/`_lastBg` būseną)
- `sealed` leidžia kompiliatoriui optimizuoti virtualių metodų kvietimus
