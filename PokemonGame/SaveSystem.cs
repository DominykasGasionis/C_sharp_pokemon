using System.Text.Json;

namespace PokemonGame;

public class PokemonSaveEntry
{
    public string Name       { get; set; } = "";
    public int    MaxHp      { get; set; }
    public int    Hp         { get; set; }
    public int    Attack     { get; set; }
    public int    Defense    { get; set; }
    public int    Level      { get; set; } = 5;
    public int    Experience { get; set; } = 0;
}

public class SaveData
{
    public List<PokemonSaveEntry> AllPokemon   { get; set; } = new();
    public int[]                  PartyIndices { get; set; } = new[] { 0, -1, -1 };
    public int PlayerX          { get; set; }
    public int PlayerY          { get; set; }
    public int EncounterChance  { get; set; } = 25;
    public int Pokeballs        { get; set; } = 5;
    public int Potions          { get; set; } = 3;
}

public static class SaveSystem
{
    private static readonly string SavePath = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
        ".pokemongame", "save.json");

    public static bool SaveExists() => File.Exists(SavePath);

    public static void Save(PokemonRoster roster, Player player, GameSettings settings, Inventory inventory)
    {
        var data = new SaveData
        {
            AllPokemon = roster.All.Select(p => new PokemonSaveEntry
            {
                Name       = p.Name,
                MaxHp      = p.MaxHp,
                Hp         = p.Hp,
                Attack     = p.Attack,
                Defense    = p.Defense,
                Level      = p.Level,
                Experience = p.Experience,
            }).ToList(),
            PartyIndices    = roster.GetPartyIndices(),
            PlayerX         = player.X,
            PlayerY         = player.Y,
            EncounterChance = settings.EncounterChance,
            Pokeballs       = inventory.Pokeballs,
            Potions         = inventory.Potions,
        };

        Directory.CreateDirectory(Path.GetDirectoryName(SavePath)!);
        File.WriteAllText(SavePath, JsonSerializer.Serialize(data,
            new JsonSerializerOptions { WriteIndented = true }));
    }

    public static SaveData? Load()
    {
        if (!SaveExists()) return null;
        try
        {
            return JsonSerializer.Deserialize<SaveData>(File.ReadAllText(SavePath));
        }
        catch
        {
            return null;
        }
    }

    public static PokemonRoster RosterFromSave(SaveData save)
    {
        var all = save.AllPokemon
            .Select(e => new Pokemon(e.Name, e.MaxHp, e.Attack, e.Defense, e.Hp, e.Level, e.Experience))
            .ToList();
        return new PokemonRoster(all, save.PartyIndices);
    }

    public static void Delete() => File.Delete(SavePath);
}
