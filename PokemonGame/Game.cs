namespace PokemonGame;

public class Game
{
    private readonly Map           _map;
    private readonly Player        _player;
    private readonly PokemonRoster _roster;
    private readonly Random        _rng = new();
    private readonly GameSettings  _settings;
    private readonly ScreenBuffer  _buf = new();
    private readonly Inventory     _inventory;
    private string _statusMessage = "Vaikščiokite naudodami WASD arba rodyklių klavišus.";

    public Game(GameSettings settings, PokemonRoster roster, int startX = 1, int startY = 1, Inventory? inventory = null)
    {
        _settings  = settings;
        _map       = new Map();
        _player    = new Player(startX, startY);
        _roster    = roster;
        _inventory = inventory ?? new Inventory();
    }

    public void Run()
    {
        Console.CursorVisible = false;
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        Console.Clear();

        while (true)
        {
            Render();
            var key = Console.ReadKey(intercept: true).Key;

            if (key == ConsoleKey.Escape || key == ConsoleKey.Q)
            {
                SaveSystem.Save(_roster, _player, _settings, _inventory);
                _statusMessage = "Žaidimas išsaugotas.";
                break;
            }

            if (key == ConsoleKey.I)
            {
                new PokemonMenu(_roster).Run();
                continue;
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
        bool allHealthy = _roster.Party
            .Where(p => p != null)
            .All(p => p!.Hp == p.MaxHp);

        if (allHealthy)
        {
            _statusMessage = "✚ Pokemon centras: visi Pokemon visiškai sveiki!";
            return;
        }

        _roster.HealParty();
        _statusMessage = "✚ Pokemon centras: visi Pokemon atgavo visą sveikatą!";
    }

    private void TriggerBattle()
    {
        if (_roster.ActivePokemon == null)
        {
            _roster.HealParty();
            _statusMessage = "Visi Pokemon krito. Atsigavote Pokemon centre.";
            return;
        }

        var wild   = Pokemon.RandomWild(_rng);
        ClearScreen();
        var battle = new Battle(_roster, wild, _rng, _inventory);
        var result = battle.Run();

        _statusMessage = result switch
        {
            BattleResult.PlayerWon     => $"Nugalėjote {wild.Name}!",
            BattleResult.PlayerFled    => "Pabėgote iš kovos.",
            BattleResult.PlayerLost    => "Visi Pokemon krito. Atsigavote.",
            BattleResult.PokemonCaught => $"Pagavote {wild.Name}! Patikrink inventorių [I].",
            _                          => "",
        };

        if (result == BattleResult.PlayerLost) _roster.HealParty();
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

        int partyCount  = _roster.Party.Count(p => p != null);
        int contentRows = _map.Height + 2 + partyCount + 7;
        int termH0      = Math.Max(contentRows + 2, Console.WindowHeight);
        int topMargin   = Math.Max(0, (termH0 - contentRows) / 2);
        string blankRow = new string(' ', termW);
        for (int i = 0; i < topMargin; i++) _buf.WriteLine(blankRow);

        _map.Render(_buf, _player.X, _player.Y, leftPad, rightPad);

        // HUD tokio paties pločio ir centravimo kaip žemėlapis
        int hudLeftPad  = leftPad;
        int hudInner    = _map.Width;
        int hudRightPad = rightPad;
        string hpad     = pad;

        var active = _roster.ActivePokemon ?? _roster.Party.First(p => p != null);

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

        const int barW = 20;

        // Statistikos eilutė fiksuota dalis: " ATK "(5)+atk(3)+"  DEF "(6)+def(3)+"  │  "(5)+pos(7)+"  Vietovė: "(11)+" "(1) = 41
        const int fixedStats = 41;
        string tileField = tileName.PadRight(Math.Max(0, hudInner - fixedStats));

        string hint   = "[WASD/↑↓←→] Judėti  [I] Pokemon  [Q/Esc] Išsaugoti ir išeiti";
        string status = _statusMessage.Length > hudInner - 2
            ? _statusMessage[..(hudInner - 5)] + "..."
            : _statusMessage;

        // ─── HUD ─────────────────────────────────────────────────
        Hline(_buf, hpad, hudInner, '╔', '═', '╗', hudRightPad);

        // Po eilutę kiekvienam užimtam partijos lizdui
        int partyRows = 0;
        for (int i = 0; i < 3; i++)
        {
            var p = _roster.Party[i];
            if (p == null) continue;

            bool isActive = p == active;
            string marker = isActive ? "☻" : "·";
            ConsoleColor nameColor  = isActive ? ConsoleColor.Cyan : ConsoleColor.White;
            ConsoleColor hpColor    = p.Hp > p.MaxHp / 2 ? ConsoleColor.Green
                                    : p.Hp > p.MaxHp / 4 ? ConsoleColor.Yellow
                                    : ConsoleColor.Red;
            string hpFrac = $"{p.Hp}/{p.MaxHp}";
            string hpBar  = p.HpBar(barW);

            HRow(_buf, hpad, hudInner, hudRightPad, (ConsoleColor.DarkCyan, ""),
                (ConsoleColor.DarkGray, $" [{i + 1}]"),
                (nameColor,             $"{marker} {p.Name,-10}"),
                (ConsoleColor.DarkGray, $" Lv{p.Level,-2}"),
                (ConsoleColor.DarkGray, "  HP "),
                (hpColor,               hpBar),
                (ConsoleColor.DarkGray, $" {hpFrac,-9} "));
            partyRows++;
        }

        Hline(_buf, hpad, hudInner, '╠', '─', '╣', hudRightPad);

        // statusas
        HRow(_buf, hpad, hudInner, hudRightPad, (ConsoleColor.DarkCyan, ""),
            (ConsoleColor.White, $" {status.PadRight(hudInner - 2)}"));

        // Pozicijos / vietovės eilutė
        HRow(_buf, hpad, hudInner, hudRightPad, (ConsoleColor.DarkCyan, ""),
            (ConsoleColor.White,    $" ({_player.X,2},{_player.Y,2})"),
            (ConsoleColor.DarkGray, "  Vietovė: "),
            (ConsoleColor.Green,    tileField),
            (ConsoleColor.DarkCyan, " "));

        Hline(_buf, hpad, hudInner, '╠', '─', '╣', hudRightPad);

        // klavišai
        HRow(_buf, hpad, hudInner, hudRightPad, (ConsoleColor.DarkCyan, ""),
            (ConsoleColor.DarkGray, $" {hint.PadRight(hudInner - 2)}"));

        Hline(_buf, hpad, hudInner, '╚', '═', '╝', hudRightPad);

        // Perrašyti likusias ekrano eilutes (senų kadrų likučiai)
        // Map: Height+2. HUD: 1(top)+partyRows+1+1+1+1+1(bot) = partyRows+7
        int renderedRows = topMargin + _map.Height + 2 + partyRows + 7;
        int termH = Math.Max(renderedRows + 2, Console.WindowHeight);
        string blankLine = new string(' ', termW);
        for (int i = renderedRows; i < termH; i++)
            _buf.WriteLine(blankLine);

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
