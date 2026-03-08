namespace PokemonGame;

public enum BattleResult { PlayerWon, PlayerFled, PlayerLost }

public class Battle
{
    private readonly Pokemon _player;
    private readonly Pokemon _wild;
    private readonly Random _rng;
    private readonly List<string> _log = new();
    private readonly ScreenBuffer _buf = new();

    private const int LeftW  = 38; // kairė vidaus plotis
    private const int RightW = 30; // dešinė vidaus plotis

    public Battle(Pokemon player, Pokemon wild, Random rng)
    {
        _player = player;
        _wild   = wild;
        _rng    = rng;
    }

    public BattleResult Run()
    {

        while (true)
        {
            RenderBattle();
            var choice = PromptChoice();

            if (choice == 1) // Kovoti
            {
                int playerDmg  = Math.Max(1, _player.Attack - _wild.Defense + _rng.Next(-5, 6));
                int actualDmg  = _wild.TakeDamage(playerDmg);
                AddLog($"{_player.Name} puolė – {_wild.Name} gavo {actualDmg} žalos.");

                if (!_wild.IsAlive)
                {
                    AddLog($"Laukinis {_wild.Name} nugalėtas!");
                    RenderBattle();
                    WaitKey();
                    return BattleResult.PlayerWon;
                }

                int enemyDmg      = Math.Max(1, _wild.Attack - _player.Defense + _rng.Next(-5, 6));
                int actualEnemyDmg = _player.TakeDamage(enemyDmg);
                AddLog($"{_wild.Name} puolė – {_player.Name} gavo {actualEnemyDmg} žalos.");

                if (!_player.IsAlive)
                {
                    AddLog($"{_player.Name} krito...");
                    RenderBattle();
                    WaitKey();
                    return BattleResult.PlayerLost;
                }
            }
            else // Bėgti
            {
                if (_rng.Next(100) < 60)
                {
                    AddLog("Pabėgote sėkmingai!");
                    RenderBattle();
                    WaitKey();
                    return BattleResult.PlayerFled;
                }
                else
                {
                    AddLog("Nepavyko pabėgti!");
                    int enemyDmg      = Math.Max(1, _wild.Attack - _player.Defense);
                    int actualEnemyDmg = _player.TakeDamage(enemyDmg);
                    AddLog($"{_wild.Name} puolė – {_player.Name} gavo {actualEnemyDmg} žalos.");

                    if (!_player.IsAlive)
                    {
                        AddLog($"{_player.Name} krito...");
                        RenderBattle();
                        WaitKey();
                        return BattleResult.PlayerLost;
                    }
                }
            }
        }
    }

    private void AddLog(string msg) => _log.Add(msg);

    private static void WaitKey()
    {
        // Rodome žinutę žemiau ekrano
        Console.SetCursorPosition(0, 21);
        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.Write("  Spausk bet kurį klavišą...");
        Console.ResetColor();
        Console.ReadKey(true);
    }

    // ── Rendering ────────────────────────────────────────────────

    private void RenderBattle()
    {
        ConsoleColor wildHp   = HpColor(_wild);
        ConsoleColor playerHp = HpColor(_player);

        // Paruošiame kairę kolumną (fiksuotas eilučių sk.)
        var left = new (string Text, ConsoleColor Color)[]
        {
            ("",                                                     ConsoleColor.White),
            ($"  ⚡ Laukinis {_wild.Name}",                         ConsoleColor.Red),
            ($"  HP: {_wild.Hp}/{_wild.MaxHp}",                     wildHp),
            ($"  {_wild.HpBar(LeftW - 4)}",                         wildHp),
            ("",                                                     ConsoleColor.White),
            ("  " + new string('─', LeftW - 4),                     ConsoleColor.DarkGray),
            ("",                                                     ConsoleColor.White),
            ($"  ☻ {_player.Name}",                                  ConsoleColor.Cyan),
            ($"  HP: {_player.Hp}/{_player.MaxHp}",                 playerHp),
            ($"  {_player.HpBar(LeftW - 4)}",                       playerHp),
            ("",                                                     ConsoleColor.White),
            ("  ┌──────────────────┐",                               ConsoleColor.White),
            ("  │  [1] Kovoti      │",                               ConsoleColor.White),
            ("  │  [2] Bėgti       │",                               ConsoleColor.White),
            ("  └──────────────────┘",                               ConsoleColor.White),
            ("",                                                     ConsoleColor.White),
        };

        // Paruošiame dešinę kolumną – istorija
        var right = BuildLogColumn(left.Length);

        // Viršutinė eilutė
        Border("╔", LeftW, "╦", RightW, "╗");
        RowColored(
            $"{"POKEMON KOVA!",LeftW / 2 + 6}".PadRight(LeftW), ConsoleColor.Yellow,
            $" KOVOS ISTORIJA".PadRight(RightW),                 ConsoleColor.DarkYellow);
        Border("╠", LeftW, "╣", RightW, "║", secondFill: '─');

        for (int i = 0; i < left.Length; i++)
        {
            string lText = left[i].Text.PadRight(LeftW);
            if (lText.Length > LeftW) lText = lText[..LeftW];
            RowColored(lText, left[i].Color, right[i], ConsoleColor.DarkGray);
        }

        Border("╚", LeftW, "╩", RightW, "╝");

        _buf.Flush();
    }

    private string[] BuildLogColumn(int rows)
    {
        var lines = new string[rows];
        int logStart = 1; // pradedame nuo 1 eilutės (po antrašte)

        for (int i = 0; i < rows; i++)
        {
            int logIdx = i - logStart;
            if (logIdx >= 0 && logIdx < _log.Count)
            {
                string msg = _log[logIdx];
                // Eilutę lūžame jei per ilga
                if (msg.Length > RightW - 4)
                    msg = msg[..(RightW - 7)] + "...";
                lines[i] = $" › {msg}".PadRight(RightW);
            }
            else
            {
                lines[i] = new string(' ', RightW);
            }
        }
        return lines;
    }

    private static ConsoleColor HpColor(Pokemon p) =>
        p.Hp > p.MaxHp / 2 ? ConsoleColor.Green :
        p.Hp > p.MaxHp / 4 ? ConsoleColor.Yellow : ConsoleColor.Red;

    private void Border(string l, int lw, string mid, int rw, string r, char secondFill = '═')
    {
        _buf.Set(ConsoleColor.DarkCyan, ConsoleColor.Black);
        _buf.WriteLine(l + new string('═', lw) + mid + new string(secondFill, rw) + r);
    }

    private void RowColored(string leftText, ConsoleColor lColor, string rightText, ConsoleColor rColor)
    {
        _buf.Set(ConsoleColor.DarkCyan, ConsoleColor.Black);
        _buf.Write("║");
        _buf.SetFg(lColor);
        _buf.Write(leftText);
        _buf.SetFg(ConsoleColor.DarkCyan);
        _buf.Write("║");
        _buf.SetFg(rColor);
        _buf.Write(rightText);
        _buf.Set(ConsoleColor.DarkCyan, ConsoleColor.Black);
        _buf.WriteLine("║");
    }

    private int PromptChoice()
    {
        // Laukiame įvesties – ekranas jau atvaizduotas
        while (true)
        {
            var key = Console.ReadKey(intercept: true).Key;
            if (key == ConsoleKey.D1 || key == ConsoleKey.NumPad1) return 1;
            if (key == ConsoleKey.D2 || key == ConsoleKey.NumPad2) return 2;
        }
    }
}
