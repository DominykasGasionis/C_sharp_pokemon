namespace PokemonGame;

public class Game
{
    private readonly Map _map;
    private readonly Player _player;
    private readonly Pokemon _playerPokemon;
    private readonly Random _rng = new();
    private readonly GameSettings _settings;
    private readonly ScreenBuffer _buf = new();
    private string _statusMessage = "Vaikščiokite naudodami WASD arba rodyklių klavišus.";

    public Game(GameSettings settings, Pokemon playerPokemon, int startX = 1, int startY = 1)
    {
        _settings = settings;
        _map = new Map();
        _player = new Player(startX, startY);
        _playerPokemon = playerPokemon;
    }

    public void Run()
    {
        Console.CursorVisible = false;
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        while (true)
        {
            Render();
            var key = Console.ReadKey(intercept: true).Key;

            if (key == ConsoleKey.Escape || key == ConsoleKey.Q)
            {
                SaveSystem.Save(_playerPokemon, _player, _settings);
                _statusMessage = "Žaidimas išsaugotas.";
                break;
            }

            HandleInput(key);
        }

        Console.CursorVisible = true;
        Console.ResetColor();
        Console.Clear();
    }

    private void HandleInput(ConsoleKey key)
    {
        int dx = 0, dy = 0;

        switch (key)
        {
            case ConsoleKey.W or ConsoleKey.UpArrow:    dy = -1; break;
            case ConsoleKey.S or ConsoleKey.DownArrow:  dy = +1; break;
            case ConsoleKey.A or ConsoleKey.LeftArrow:  dx = -1; break;
            case ConsoleKey.D or ConsoleKey.RightArrow: dx = +1; break;
            default: return;
        }

        bool moved = _player.TryMove(dx, dy, _map);

        if (moved)
        {
            var tile = _map.GetTile(_player.X, _player.Y);

            if (tile == TileType.HealCenter)
            {
                TriggerHeal();
            }
            else if (tile == TileType.TallGrass && _rng.Next(100) < _settings.EncounterChance)
            {
                TriggerBattle();
            }
            else
            {
                _statusMessage = tile switch
                {
                    TileType.TallGrass => "Aukšta žolė... gali pasirodyti Pokemon!",
                    TileType.Sand      => "Smėlio takas.",
                    TileType.Flower    => "Čia auga gražios gėlės.",
                    _                  => "",
                };
            }
        }
        else
        {
            _statusMessage = "Negalima eiti ten!";
        }
    }

    private void TriggerHeal()
    {
        if (_playerPokemon.Hp == _playerPokemon.MaxHp)
        {
            _statusMessage = "✚ Pokemon centras: jūsų Pokemon visiškai sveikas!";
            return;
        }

        _playerPokemon.HealFull();
        _statusMessage = $"✚ Pokemon centras: {_playerPokemon.Name} atgavo visą sveikatą!";
    }

    private void TriggerBattle()
    {
        var wild = Pokemon.RandomWild(_rng);
        ClearScreen();
        var battle = new Battle(_playerPokemon, wild, _rng);
        var result = battle.Run();

        _statusMessage = result switch
        {
            BattleResult.PlayerWon  => $"Nugalėjote {wild.Name}!",
            BattleResult.PlayerFled => "Pabėgote iš kovos.",
            BattleResult.PlayerLost => $"{_playerPokemon.Name} krito. Atsigavote.",
            _                       => "",
        };
    }

    private static void ClearScreen()
    {
        int h = Math.Max(40, Console.WindowHeight);
        int w = Math.Max(82, Console.WindowWidth);
        Console.SetCursorPosition(0, 0);
        string blank = new string(' ', w);
        for (int i = 0; i < h - 1; i++)
            Console.WriteLine(blank);
        Console.Write(blank);
        Console.SetCursorPosition(0, 0);
    }

    private void Render()
    {
        int termW    = Math.Max(82, Console.WindowWidth);
        int mapW     = _map.Width + 2;
        int leftPad  = Math.Max(0, (termW - mapW) / 2);
        int rightPad = Math.Max(0, termW - leftPad - mapW);
        string pad   = new string(' ', leftPad);

        _map.Render(_buf, _player.X, _player.Y, leftPad, rightPad);

        // HUD plotis: platesnis nei žemėlapis, centruotas tarp jo kraštų
        int hudLeftPad  = Math.Max(0, leftPad - 12);
        int hudInner    = termW - 2 * hudLeftPad - 2;
        int hudRightPad = Math.Max(0, termW - hudLeftPad - hudInner - 2);
        string hpad     = new string(' ', hudLeftPad);

        ConsoleColor hpColor = _playerPokemon.Hp > _playerPokemon.MaxHp / 2
            ? ConsoleColor.Green
            : _playerPokemon.Hp > _playerPokemon.MaxHp / 4
                ? ConsoleColor.Yellow
                : ConsoleColor.Red;

        var tile = _map.GetTile(_player.X, _player.Y);
        string tileName = tile switch
        {
            TileType.TallGrass  => "Aukšta žolė",
            TileType.Water      => "Vanduo",
            TileType.Sand       => "Smėlis",
            TileType.Flower     => "Gėlės",
            TileType.Building   => "Pastatas",
            TileType.HealCenter => "Pokemon centras",
            _                   => "Kelias",
        };

        // 1 eilutė: " ☻ " + name(12) + "  HP " + bar(?) + " " + hpFrac(9) + " "
        //            4        12          5         ?        1    9             1  = 32 fixed
        const int fixedHp = 32;
        string hpFrac = $"{_playerPokemon.Hp}/{_playerPokemon.MaxHp}";
        int    barW   = hudInner - fixedHp;
        string hpBar  = _playerPokemon.HpBar(Math.Max(4, barW));

        // 2 eilutė fiksuota dalis: " ATK "(5) + atk(3) + "  DEF "(6) + def(3)
        //   + "  │  "(5) + pos(7) + "  Vietovė: "(11) + " "(1) = 41 fixed
        const int fixedStats = 41;
        string tileField = tileName.PadRight(Math.Max(0, hudInner - fixedStats));

        string hint   = "[WASD/↑↓←→] Judėti    [Q/Esc] Išsaugoti ir išeiti";
        string status = _statusMessage.Length > hudInner - 2
            ? _statusMessage[..(hudInner - 5)] + "..."
            : _statusMessage;
        string saveSymbol = SaveSystem.SaveExists() ? "✓" : "✗";
        ConsoleColor saveColor = SaveSystem.SaveExists() ? ConsoleColor.Green : ConsoleColor.DarkGray;

        // ─── HUD ─────────────────────────────────────────────────
        Hline(_buf, hpad, hudInner, '╔', '═', '╗', hudRightPad);

        // 1 eilutė: Pokemon + HP
        HRow(_buf, hpad, hudInner, hudRightPad, (ConsoleColor.DarkCyan, ""),
            (ConsoleColor.Cyan,    $" ☻ {_playerPokemon.Name,-12}"),
            (ConsoleColor.DarkGray,"  HP "),
            (hpColor,              hpBar),
            (ConsoleColor.DarkGray,$" {hpFrac,-9} "));

        // 2 eilutė: ATK / DEF / pozicija / vietovė
        HRow(_buf, hpad, hudInner, hudRightPad, (ConsoleColor.DarkCyan, ""),
            (ConsoleColor.DarkGray, " ATK "),
            (ConsoleColor.Yellow,   $"{_playerPokemon.Attack,-3}"),
            (ConsoleColor.DarkGray, "  DEF "),
            (ConsoleColor.Yellow,   $"{_playerPokemon.Defense,-3}"),
            (ConsoleColor.DarkGray, "  │  "),
            (ConsoleColor.White,    $"({_player.X,2},{_player.Y,2})"),
            (ConsoleColor.DarkGray, "  Vietovė: "),
            (ConsoleColor.Green,    tileField),
            (ConsoleColor.DarkCyan, " "));

        // 3 eilutė: susitikimų šansas / išsaugojimas
        HRow(_buf, hpad, hudInner, hudRightPad, (ConsoleColor.DarkCyan, ""),
            (ConsoleColor.DarkGray, " Šansas: "),
            (ConsoleColor.Yellow,   $"{_settings.EncounterChance}%"),
            (ConsoleColor.DarkGray, "  │  Išsaugota: "),
            (saveColor,             saveSymbol),
            (ConsoleColor.DarkCyan, " "));

        Hline(_buf, hpad, hudInner, '╠', '─', '╣', hudRightPad);

        // 4 eilutė: statusas
        HRow(_buf, hpad, hudInner, hudRightPad, (ConsoleColor.DarkCyan, ""),
            (ConsoleColor.White, $" {status.PadRight(hudInner - 2)}"));

        Hline(_buf, hpad, hudInner, '╠', '─', '╣', hudRightPad);

        // 5 eilutė: klavišai
        HRow(_buf, hpad, hudInner, hudRightPad, (ConsoleColor.DarkCyan, ""),
            (ConsoleColor.DarkGray, $" {hint.PadRight(hudInner - 2)}"));

        Hline(_buf, hpad, hudInner, '╚', '═', '╝', hudRightPad);

        _buf.Flush();
    }

    // Rėmelio eilutė
    private static void Hline(ScreenBuffer buf, string pad, int inner, char l, char fill, char r, int rightPad = 0)
    {
        buf.Set(ConsoleColor.DarkCyan, ConsoleColor.Black);
        buf.WriteLine(pad + l + new string(fill, inner) + r + new string(' ', rightPad));
    }

    // Turinio eilutė: automatiškai papildo iki tikslaus `inner` pločio
    private static void HRow(ScreenBuffer buf, string pad, int inner, int rightPad,
        params (ConsoleColor Color, string Text)[] segments)
    {
        buf.Set(ConsoleColor.DarkCyan, ConsoleColor.Black);
        buf.Write(pad + "║");

        int written = 0;
        foreach (var (color, text) in segments)
        {
            buf.SetFg(color);
            buf.Write(text);
            written += text.Length;
        }

        // Užpildome likusį plotą tuščiais simboliais
        if (written < inner)
        {
            buf.SetFg(ConsoleColor.Black);
            buf.Write(new string(' ', inner - written));
        }

        buf.Set(ConsoleColor.DarkCyan, ConsoleColor.Black);
        buf.WriteLine("║" + new string(' ', rightPad));
    }
}
