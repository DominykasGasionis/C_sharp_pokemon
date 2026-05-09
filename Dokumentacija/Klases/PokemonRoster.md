# PokemonRoster

**Projektas:** `PokemonGame`
**Failas:** `PokemonRoster.cs`

## Paskirtis

Valdo žaidėjo Pokemon kolekciją – tiek aktyvią partiją (iki 3 Pokemon), tiek visus pagautus Pokemon „dėžėje". Tvarko partijos keitimą, gydymą ir išsaugojimą.

## Kodas

```csharp
public class PokemonRoster
{
    public List<Pokemon> All  { get; } = new();
    public Pokemon?[] Party   { get; } = new Pokemon?[3];

    public Pokemon? ActivePokemon => Array.Find(Party, p => p?.IsAlive == true);
}
```

## Savybės

| Savybė | Tipas | Aprašas |
|---|---|---|
| `All` | `List<Pokemon>` | Visi žaidėjo Pokemon (partija + dėžė) |
| `Party` | `Pokemon?[3]` | Aktyvios partijos 3 lizdai (gali būti `null`) |
| `ActivePokemon` | `Pokemon?` | Pirmas gyvas (`IsAlive == true`) partijos Pokemon; `null` jei visi krito |

## Konstruktoriai

```csharp
// Naujas žaidimas – starteris į All ir Party[0]
public PokemonRoster(Pokemon starter)

// Atstatymas iš išsaugojimo – Pokemon iš JSON, partija pagal indeksus
public PokemonRoster(List<Pokemon> all, int[] partyIndices)
```

## Metodai

| Metodas | Grąžina | Aprašas |
|---|---|---|
| `Catch(Pokemon)` | `void` | Prideda pagautą Pokemon į `All`; automatiškai į partiją jei yra laisva vieta |
| `HealParty()` | `void` | Gydo visus partijos Pokemon per `IHealable` sąsają |
| `GetPartyIndices()` | `int[]` | Grąžina partijos Pokemon indeksus `All` sąraše (išsaugojimui); `-1` jei tuščias lizas |

## Partijos struktūra

```
Party[0] → Pokemon arba null
Party[1] → Pokemon arba null
Party[2] → Pokemon arba null
```

„Dėžė" yra visi `All` Pokemon, kurie **nėra** `Party` masyve.

## `Catch` logika

```csharp
public void Catch(Pokemon p)
{
    All.Add(p);
    for (int i = 0; i < 3; i++)
    {
        if (Party[i] == null) { Party[i] = p; return; }
    }
    // Partija pilna – Pokemon eina tik į dėžę
}
```

## `GetPartyIndices` – išsaugojimui

Grąžina `int[3]` masyvą su kiekvieno partijos nario indeksu `All` sąraše. Jei lizas tuščias – `-1`:
```csharp
// Pvz: Party[0]=All[0], Party[1]=All[2], Party[2]=null → [0, 2, -1]
```
