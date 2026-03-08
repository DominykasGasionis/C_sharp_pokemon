using System.Text.Json;

namespace PokemonGame;

public class SaveData
{
    public string PokemonName { get; set; } = "";
    public int PokemonMaxHp { get; set; }
    public int PokemonHp { get; set; }
    public int PokemonAttack { get; set; }
    public int PokemonDefense { get; set; }
    public int PlayerX { get; set; }
    public int PlayerY { get; set; }
    public int EncounterChance { get; set; } = 25;
}

public static class SaveSystem
{
    private static readonly string SavePath = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
        ".pokemongame", "save.json");

    public static bool SaveExists() => File.Exists(SavePath);

    public static void Save(Pokemon pokemon, Player player, GameSettings settings)
    {
        var data = new SaveData
        {
            PokemonName    = pokemon.Name,
            PokemonMaxHp   = pokemon.MaxHp,
            PokemonHp      = pokemon.Hp,
            PokemonAttack  = pokemon.Attack,
            PokemonDefense = pokemon.Defense,
            PlayerX        = player.X,
            PlayerY        = player.Y,
            EncounterChance = settings.EncounterChance,
        };

        Directory.CreateDirectory(Path.GetDirectoryName(SavePath)!);
        File.WriteAllText(SavePath, JsonSerializer.Serialize(data, new JsonSerializerOptions { WriteIndented = true }));
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

    public static void Delete() => File.Delete(SavePath);
}
