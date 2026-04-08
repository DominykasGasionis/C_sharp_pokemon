# 5. switch su when — 0.5 t.

## Reikalavimas

Naudojate `switch` su `when` raktažodžiu.

## Implementacija

**Failas:** `PokemonGame/Game.cs`

```csharp
_statusMessage = tile switch
{
    TileType.TallGrass when _settings.EncounterChance >= 50 =>
        "Aukšta žolė! Pokemon pasirodo labai dažnai.",

    TileType.TallGrass when _settings.EncounterChance >= 25 =>
        "Aukšta žolė... gali pasirodyti Pokemon!",

    TileType.TallGrass =>
        "Aukšta žolė. Pokemon reti.",

    TileType.Sand   => "Smėlio takas.",
    TileType.Flower => "Čia auga gražios gėlės.",
    _               => "",
};
```

## Veikimas

`when` sąlyga prideda papildomą tikrinimą prie `case` šablono. Switch apdorojamas iš viršaus žemyn – pirmasis tinkamas atvejis laimi. Žaidėjui rodoma skirtinga žinutė priklausomai nuo nustatymų meniu pasirinkto susidūrimų dažnio.
