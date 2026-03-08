namespace PokemonGame;

public enum MenuAction { NewGame, Continue, Settings, Exit }

public class GameSettings
{
    public int EncounterChance { get; set; } = 25;
}

public class Menu
{
    private readonly GameSettings _settings;
    private int _selected = 0;

    public Menu(GameSettings settings)
    {
        _settings = settings;
    }

    public MenuAction Run()
    {
        bool hasSave = SaveSystem.SaveExists();
        var options = hasSave
            ? new[] { "Naujas žaidimas", "Tęsti", "Nustatymai", "Išeiti" }
            : new[] { "Žaisti", "Nustatymai", "Išeiti" };

        _selected = Math.Min(_selected, options.Length - 1);

        while (true)
        {
            RenderMain(options, hasSave);
            var key = Console.ReadKey(intercept: true).Key;

            switch (key)
            {
                case ConsoleKey.UpArrow or ConsoleKey.W:
                    _selected = (_selected - 1 + options.Length) % options.Length;
                    break;

                case ConsoleKey.DownArrow or ConsoleKey.S:
                    _selected = (_selected + 1) % options.Length;
                    break;

                case ConsoleKey.Enter or ConsoleKey.Spacebar:
                    if (hasSave)
                    {
                        return _selected switch
                        {
                            0 => MenuAction.NewGame,
                            1 => MenuAction.Continue,
                            2 => MenuAction.Settings,
                            _ => MenuAction.Exit,
                        };
                    }
                    else
                    {
                        return _selected switch
                        {
                            0 => MenuAction.NewGame,
                            1 => MenuAction.Settings,
                            _ => MenuAction.Exit,
                        };
                    }

                case ConsoleKey.Escape:
                    return MenuAction.Exit;
            }
        }
    }

    private static readonly string[] Logo =
    [
        @"██████╗  ██████╗ ██╗  ██╗███████╗███╗   ███╗ ██████╗ ███╗   ██╗",
        @"██╔══██╗██╔═══██╗██║ ██╔╝██╔════╝████╗ ████║██╔═══██╗████╗  ██║",
        @"██████╔╝██║   ██║█████╔╝ █████╗  ██╔████╔██║██║   ██║██╔██╗ ██║",
        @"██╔═══╝ ██║   ██║██╔═██╗ ██╔══╝  ██║╚██╔╝██║██║   ██║██║╚██╗██║",
        @"██║     ╚██████╔╝██║  ██╗███████╗██║ ╚═╝ ██║╚██████╔╝██║ ╚████║",
        @"╚═╝      ╚═════╝ ╚═╝  ╚═╝╚══════╝╚═╝     ╚═╝ ╚═════╝ ╚═╝  ╚═══╝",
    ];

    private static int W => Math.Max(80, Console.WindowWidth);

    private static void Centered(string text, ConsoleColor color = ConsoleColor.White)
    {
        int pad = Math.Max(0, (W - text.Length) / 2);
        Console.ForegroundColor = color;
        Console.WriteLine(new string(' ', pad) + text);
        Console.ResetColor();
    }

    private static string Pad(string text, int totalWidth)
    {
        int pad = Math.Max(0, (totalWidth - text.Length) / 2);
        return new string(' ', pad) + text;
    }

    private void RenderMain(string[] options, bool hasSave)
    {
        Console.Clear();
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        Console.WriteLine();
        foreach (var line in Logo)
            Centered(line, ConsoleColor.Yellow);

        Console.WriteLine();
        Centered("─── Konsolinis RPG žaidimas ───", ConsoleColor.DarkYellow);
        Console.WriteLine();

        // Meniu rėmelis
        int boxWidth = 28;
        string top    = "╔" + new string('═', boxWidth) + "╗";
        string bottom = "╚" + new string('═', boxWidth) + "╝";
        string divider= "╠" + new string('═', boxWidth) + "╣";

        Centered(top, ConsoleColor.DarkYellow);

        for (int i = 0; i < options.Length; i++)
        {
            bool active     = i == _selected;
            bool isContinue = hasSave && i == 1;
            bool isLast     = i == options.Length - 1;

            // Skyriklis prieš paskutinę parinktį (Išeiti)
            if (isLast && options.Length > 2)
                Centered(divider, ConsoleColor.DarkYellow);

            string label = active ? $"► {options[i]}" : $"  {options[i]}";
            string inner = label.PadRight(boxWidth - 2);
            string row   = $"║ {inner} ║";

            int pad = Math.Max(0, (W - row.Length) / 2);
            Console.Write(new string(' ', pad));

            if (active)
            {
                Console.ForegroundColor = ConsoleColor.Black;
                Console.BackgroundColor = isContinue ? ConsoleColor.Green : ConsoleColor.Yellow;
                Console.Write($" {label.PadRight(boxWidth - 2)} ");
                Console.ResetColor();
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.Write("║");
                // perrašome pradžią
                Console.SetCursorPosition(pad, Console.CursorTop);
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.Write("║");
                Console.ForegroundColor = ConsoleColor.Black;
                Console.BackgroundColor = isContinue ? ConsoleColor.Green : ConsoleColor.Yellow;
                Console.Write($" {label.PadRight(boxWidth - 2)} ");
                Console.ResetColor();
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine("║");
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.Write("║ ");
                Console.ForegroundColor = isContinue ? ConsoleColor.Green : ConsoleColor.White;
                Console.Write(label.PadRight(boxWidth - 2));
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine(" ║");
            }
            Console.ResetColor();
        }

        Centered(bottom, ConsoleColor.DarkYellow);
        Console.WriteLine();

        if (hasSave)
            Centered("💾  Išsaugojimas rastas", ConsoleColor.DarkGreen);

        Centered("[↑ ↓] Judėti    [Enter] Pasirinkti    [Esc] Išeiti", ConsoleColor.DarkGray);
    }

    public void OpenSettings()
    {
        int sel = 0;

        while (true)
        {
            RenderSettings(sel);
            var key = Console.ReadKey(intercept: true).Key;

            switch (key)
            {
                case ConsoleKey.UpArrow or ConsoleKey.W:
                    sel = (sel - 1 + 2) % 2;
                    break;

                case ConsoleKey.DownArrow or ConsoleKey.S:
                    sel = (sel + 1) % 2;
                    break;

                case ConsoleKey.LeftArrow or ConsoleKey.A:
                    if (sel == 0) _settings.EncounterChance = Math.Max(0, _settings.EncounterChance - 5);
                    break;

                case ConsoleKey.RightArrow or ConsoleKey.D:
                    if (sel == 0) _settings.EncounterChance = Math.Min(100, _settings.EncounterChance + 5);
                    break;

                case ConsoleKey.Escape or ConsoleKey.Backspace:
                case ConsoleKey.Enter when sel == 1:
                    return;
            }
        }
    }

    private void RenderSettings(int selected)
    {
        Console.Clear();
        Console.WriteLine();

        Centered("╔══════════════════════════════╗", ConsoleColor.Yellow);
        Centered("║          NUSTATYMAI          ║", ConsoleColor.Yellow);
        Centered("╠══════════════════════════════╣", ConsoleColor.Yellow);
        Console.WriteLine();

        // Encounter rate eilutė
        bool enc = selected == 0;
        string arrow = enc ? "►" : " ";
        string slider = $"◄ {BuildSlider(_settings.EncounterChance, 0, 100, 16)} {_settings.EncounterChance,3}% ►";

        Centered($"{(enc ? "►" : " ")} Pokemon pasirodymo dažnis", enc ? ConsoleColor.Yellow : ConsoleColor.White);
        Centered(enc ? slider : $"  {BuildSlider(_settings.EncounterChance, 0, 100, 16)} {_settings.EncounterChance,3}%  ", enc ? ConsoleColor.Yellow : ConsoleColor.DarkGray);
        Console.WriteLine();

        Centered("──────────────────────────────", ConsoleColor.DarkGray);
        Console.WriteLine();

        // Grįžti mygtukas
        bool back = selected == 1;
        int pad = Math.Max(0, (W - 20) / 2);
        Console.Write(new string(' ', pad));
        if (back)
        {
            Console.ForegroundColor = ConsoleColor.Black;
            Console.BackgroundColor = ConsoleColor.Yellow;
            Console.Write("  ► Grįžti į meniu  ");
            Console.ResetColor();
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("    Grįžti į meniu  ");
            Console.ResetColor();
        }
        Console.WriteLine();
        Console.WriteLine();

        Centered("[↑ ↓] Pasirinkti    [← →] Keisti    [Esc] Grįžti", ConsoleColor.DarkGray);
    }

    private static string BuildSlider(int value, int min, int max, int width)
    {
        int pos = (int)Math.Round((double)(value - min) / (max - min) * width);
        var sb = new System.Text.StringBuilder("[");
        for (int i = 0; i < width; i++)
            sb.Append(i == pos ? '●' : '─');
        sb.Append(']');
        return sb.ToString();
    }
}
