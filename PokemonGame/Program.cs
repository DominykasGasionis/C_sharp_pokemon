using PokemonGame;

Console.CursorVisible = false;
Console.OutputEncoding = System.Text.Encoding.UTF8;

var rng = new Random();
var settings = new GameSettings();
var menu = new Menu(settings);

while (true)
{
    var action = menu.Run();

    switch (action)
    {
        case MenuAction.Exit:
            Console.Clear();
            Console.WriteLine("Iki pasimatymo!");
            return;

        case MenuAction.Settings:
            menu.OpenSettings();
            break;

        case MenuAction.Continue:
        {
            var save = SaveSystem.Load();
            if (save is null) break;

            var (roster, player, savedSettings, inventory) = save.Value;
            settings.EncounterChance = savedSettings.EncounterChance;
            Console.Clear();
            new Game(settings, roster, player.X, player.Y, inventory).Run();
            break;
        }

        case MenuAction.NewGame:
        {
            var selector = new PokemonSelector(rng);
            var chosen = selector.Run();
            if (chosen is null) break;

            if (SaveSystem.SaveExists())
                SaveSystem.Delete();

            Console.Clear();
            new Game(settings, new PokemonRoster(chosen)).Run();
            break;
        }
    }
}
