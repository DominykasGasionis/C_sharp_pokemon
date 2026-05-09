# 7. Keli moduliai (assembly) — 1 t.

## Reikalavimas

Projektas sudarytas iš daugiau nei vieno modulio (assembly).

## Implementacija

Sprendimas sudarytas iš **dviejų** atskirų .NET projektų (assembly):

### `PokemonGame.Core` (klasių biblioteka)

Grynos domenų klasės be UI priklausomybių:

- `Entity.cs` – abstrakti bazinė klasė
- `IHealable.cs` – gydymo sąsaja
- `Move.cs` – kovos judesys
- `Pokemon.cs` – Pokemon su visa logika ir StatusEffect enum

### `PokemonGame` (vykdomoji programa)

Žaidimo logika, atvaizdavimas ir UI:

- `Game.cs`, `Battle.cs`, `PokemonMenu.cs`, `Menu.cs` – žaidimo logika
- `Map.cs`, `Player.cs` – žaidimo pasaulis
- `ScreenBuffer.cs` – atvaizdavimo buferis
- `SaveSystem.cs`, `Inventory.cs`, `PokemonRoster.cs` – duomenų valdymas

### Projekto nuoroda

`PokemonGame.csproj` nurodo į `PokemonGame.Core`:

```xml
<ProjectReference Include="..\PokemonGame.Core\PokemonGame.Core.csproj" />
```

Abu projektai naudoja tą patį `namespace PokemonGame`, todėl nereikia papildomų `using` teiginių.
