# PokemonGame – Dokumentacija

## Klasių aprašymai

### PokemonGame.Core (klasių biblioteka)
| Failas | Aprašas |
|---|---|
| [Entity.md](Klases/Entity.md) | Abstrakti bazinė klasė visoms esybėms |
| [IHealable.md](Klases/IHealable.md) | Gydymo sąsaja |
| [Move.md](Klases/Move.md) | Kovos judesys |
| [Pokemon.md](Klases/Pokemon.md) | Pagrindinė žaidimo esybė |

### PokemonGame (vykdomoji programa)
| Failas | Aprašas |
|---|---|
| [Player.md](Klases/Player.md) | Žaidėjo pozicija ir judėjimas |
| [Map.md](Klases/Map.md) | 2D žemėlapis su plytelių sistema |
| [Game.md](Klases/Game.md) | Pagrindinis žaidimo ciklas ir HUD |
| [Battle.md](Klases/Battle.md) | Kovos sistema |
| [PokemonRoster.md](Klases/PokemonRoster.md) | Pokemon kolekcijos valdymas |
| [Inventory.md](Klases/Inventory.md) | Daiktų inventorius |
| [ScreenBuffer.md](Klases/ScreenBuffer.md) | Buferizuotas ekrano atvaizdavimas |
| [SaveSystem.md](Klases/SaveSystem.md) | Žaidimo išsaugojimas ir įkėlimas |
| [PokemonMenu.md](Klases/PokemonMenu.md) | Pokemon peržiūros ir valdymo meniu |
| [PokemonSelector.md](Klases/PokemonSelector.md) | Pradinio Pokemon pasirinkimas |
| [Menu.md](Klases/Menu.md) | Pagrindinis meniu ir nustatymai |

---

## Reikalavimų įvykdymas

| # | Reikalavimas | Taškai | Failas |
|---|---|---|---|
| 1 | [Savo interface](Reikalavimai/01_Interface.md) | 0.5 | `IHealable.cs` |
| 2 | [IComparable\<T\>](Reikalavimai/02_IComparable.md) | 0.5 | `Pokemon.cs` |
| 3 | [IEquatable\<T\>](Reikalavimai/03_IEquatable.md) | 0.5 | `Move.cs` |
| 4 | [IFormattable](Reikalavimai/04_IFormattable.md) | 1.0 | `Pokemon.cs` |
| 5 | [switch su when](Reikalavimai/05_Switch_When.md) | 0.5 | `Game.cs` |
| 6 | [Range tipas](Reikalavimai/06_Range.md) | 0.5 | `Battle.cs` |
| 7 | [Keli moduliai](Reikalavimai/07_Keli_Moduliai.md) | 1.0 | `PokemonGame.Core` + `PokemonGame` |
| 8 | [sealed klasė](Reikalavimai/08_Sealed_Partial.md) | 0.5 | `ScreenBuffer.cs` |
| 9 | [Abstrakti klasė](Reikalavimai/09_Abstrakti_Klase.md) | 0.5 | `Entity.cs` |
| 10 | [Statinis konstruktorius](Reikalavimai/10_Statinis_Konstruktorius.md) | 1.0 | `Map.cs` |
| 11 | [Dekonstruktorius](Reikalavimai/11_Dekonstruktorius.md) | 0.5 | `Pokemon.cs` |
| 12 | [Operatorių perkrovimas](Reikalavimai/12_Operatoriu_Perkrovimas.md) | 0.5 | `Inventory.cs` |
| 13 | [System.Collections.Generic](Reikalavimai/13_Collections.md) | 1.0 | `PokemonRoster.cs`, `Map.cs` |
| 14 | [is operatorius](Reikalavimai/14_Is_Operatorius.md) | 0.5 | `Program.cs`, `Move.cs` |
| 15 | [Numatytieji/vardiniai arg.](Reikalavimai/15_Numatytieji_Vardiniai.md) | 0.5 | `Pokemon.cs` |
| 16 | [params](Reikalavimai/16_Params.md) | 0.5 | `Game.cs` |
| 17 | [out argumentai](Reikalavimai/17_Out.md) | 1.0 | `Pokemon.cs` |
| 18 | [Lambda funkcijos](Reikalavimai/18_Lambda.md) | 1.5 | visur |
| 19 | [Bitinės operacijos](Reikalavimai/19_Bitines_Operacijos.md) | 1.0 | `Pokemon.cs`, `Battle.cs` |
| 20 | [?. ?? operatoriai](Reikalavimai/20_Nullable_Operators.md) | 0.5 | `Pokemon.cs`, `PokemonRoster.cs` |
| 21 | [Šablonų atitikimas](Reikalavimai/21_Sablonu_Atitikimas.md) | 1.0 | `Game.cs`, `Map.cs`, `Battle.cs` |
| | **Viso** | **15.0** | |
