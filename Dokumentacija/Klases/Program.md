# Program

**Projektas:** `PokemonGame`
**Failas:** `Program.cs`
**Tipas:** Įėjimo taškas (top-level statements)

## Paskirtis

Žaidimo paleidimo taškas. Valdo pagrindinį programos srautą – rodo pagrindinį meniu ir pagal žaidėjo pasirinkimą inicializuoja žaidimą (naujas arba tęsiamas).

## Kodas

```csharp
Console.CursorVisible = false;
Console.OutputEncoding = System.Text.Encoding.UTF8;

var rng      = new Random();
var settings = new GameSettings();
var menu     = new Menu(settings);

while (true)
{
    var action = menu.Run();

    switch (action)
    {
        case MenuAction.Exit:     // Baigti programą
        case MenuAction.Settings: // Atidaryti nustatymus
        case MenuAction.Continue: // Tęsti išsaugotą žaidimą
        case MenuAction.NewGame:  // Pradėti naują žaidimą
    }
}
```

## Programos srautas

```
Paleidimas
│
├─ Console.CursorVisible = false  (paslepia mirksantį kursorių)
├─ UTF-8 enkodavimas              (reikalingas Unicode simboliams: ✿ ✚ █ ░ ir kt.)
│
└─ Pagrindinis ciklas:
    ├─ menu.Run() → laukia pasirinkimo
    │
    ├─ Exit     → Console.Clear() + "Iki pasimatymo!" + return
    │
    ├─ Settings → menu.OpenSettings() (nustatymų submeniu)
    │
    ├─ Continue → SaveSystem.Load()
    │             ├─ Jei null (nepavyko) → grįžta į meniu
    │             └─ Jei ok → atkuria settings, roster, inventory
    │                         → new Game(...).Run()
    │
    └─ NewGame  → new PokemonSelector(rng).Run()
                  ├─ Jei null (grįžo atgal) → grįžta į meniu
                  └─ Jei pasirinktas Pokemon:
                      ├─ SaveSystem.Delete() (jei egzistuoja senas išsaugojimas)
                      └─ new Game(settings, new PokemonRoster(chosen)).Run()
```

## Objektų gyvavimo laikas

| Objektas | Sukuriamas | Aprašas |
|---|---|---|
| `rng` | Vieną kartą | Bendras atsitiktinumo generatorius visam žaidimui |
| `settings` | Vieną kartą | Nustatymai išlieka tarp žaidimų sesijų |
| `menu` | Vieną kartą | Meniu objektas su `settings` nuoroda |
| `Game` | Kiekvienai sesijai | Sukuriamas naujai kiekvieną kartą žaidžiant |

## `Continue` atstatymo logika

```csharp
case MenuAction.Continue:
{
    var save = SaveSystem.Load();
    if (save is null) break;  // Nepavyko įkelti – grįžta į meniu

    settings.EncounterChance = save.EncounterChance;  // Atkuria nustatymus
    var roster    = SaveSystem.RosterFromSave(save);   // Atkuria Pokemon
    var inventory = new Inventory {
        Pokeballs = save.Pokeballs,
        Potions   = save.Potions
    };
    new Game(settings, roster, save.PlayerX, save.PlayerY, inventory).Run();
    break;
}
```
