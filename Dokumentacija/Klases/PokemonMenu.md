# PokemonMenu

**Projektas:** `PokemonGame`
**Failas:** `PokemonMenu.cs`

## Paskirtis

Interaktyvus meniu Pokemon kolekcijos peržiūrai ir valdymui. Pasiekiamas per `I` klavišą žaidimo metu. Leidžia peržiūrėti partijos ir dėžės Pokemon, keisti partijos sudėtį.

## Meniu struktūra

```
╔══════════════════════════════════════════════════════════════════════╗
║  PARTIJA                                                             ║
╠──────────────────────────────────────────────────────────────────────╣
║  [1] ► Swalot        Lv8   HP:  98/100   XP: 45/400 (liko 355)     ║
║  [2]   Pikachu       Lv5   HP:  35/35    XP: 0/250  (liko 250)      ║
║  [3]   (tuščia)                                                      ║
╠──────────────────────────────────────────────────────────────────────╣
║  DĖŽĖ  (pagauti, nerūšiuoti)                                         ║
╠──────────────────────────────────────────────────────────────────────╣
║  Rattata    Lv4   HP: 30/39   XP: 0/200  (liko 200)                 ║
╚══════════════════════════════════════════════════════════════════════╝
```

## Navigacija

- `W`/`S` arba rodyklės – juda per sąrašą
- `Enter` – pakeisti partijos Pokemon (jei pasirinkta dėžės Pokemon)
- `Esc` – išeiti

## Partijos keitimas

Pasirinkus dėžės Pokemon su pilna partija – prašoma nurodyti kurią partiją poziciją pakeisti (1–3). Dėžės Pokemon rodomi surūšiuoti pagal lygį (per `IComparable<Pokemon>`).

## XP rodymas

Kiekvienam Pokemon rodoma: `XP: {dabartinis}/{reikalingas} (liko {skirtumas})`, kur liko = `ExperienceToNextLevel - Experience`.
