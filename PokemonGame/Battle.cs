namespace PokemonGame;

public enum BattleResult { PlayerWon, PlayerFled, PlayerLost, PokemonCaught }

public class Battle
{
    private readonly PokemonRoster _roster;
    private readonly Pokemon       _wild;
    private readonly Random        _rng;
    private readonly Inventory     _inventory;
    private readonly List<string>  _log = new();
    private readonly ScreenBuffer  _buf = new();
    private Pokemon _active; // current fighting Pokemon
    private bool    _selectingAttack = false;

    private const int LeftW   = 38; // kairė vidaus plotis
    private const int RightW  = 30; // dešinė vidaus plotis
    private const int TotalW  = 1 + LeftW + 1 + RightW + 1; // 71

    public Battle(PokemonRoster roster, Pokemon wild, Random rng, Inventory inventory)
    {
        _roster    = roster;
        _wild      = wild;
        _rng       = rng;
        _inventory = inventory;
        _active    = roster.ActivePokemon!;
    }

    public BattleResult Run()
    {
        while (true)
        {
            RenderBattle();

            if (_selectingAttack)
            {
                var move = PromptMove();
                if (move == null)
                {
                    _selectingAttack = false;
                    continue;
                }

                _selectingAttack = false;
                int rawDmg    = move.Power / 5 + _active.Attack + _rng.Next(-5, 6);
                int actualDmg = _wild.TakeDamage(rawDmg);
                AddLog($"{_active.Name} naudojo {move.Name}! {_wild.Name} gavo {actualDmg} žalos.");

                if (!_wild.IsAlive)
                {
                    string? lvMsg = _active.GainExperience(_wild.XpReward);
                    AddLog($"Laukinis {_wild.Name} nugalėtas!");
                    AddLog($"Gauta {_wild.XpReward} XP.");
                    if (lvMsg != null) AddLog(lvMsg);
                    RenderBattle();
                    WaitKey();
                    return BattleResult.PlayerWon;
                }
            }
            else
            {
                var choice = PromptChoice();

                if (choice == 1) // Pulti
                {
                    _selectingAttack = true;
                    continue;
                }
                else if (choice == 2) // Vaistas
                {
                    if (_inventory.Potions <= 0)
                    {
                        AddLog("Nėra vaistų!");
                    }
                    else
                    {
                        int healed = _active.Heal(20);
                        _inventory.Potions--;
                        AddLog($"{_active.Name} išgijo {healed} HP.");
                    }
                }
                else if (choice == 3) // Pokeball
                {
                    if (_inventory.Pokeballs <= 0)
                    {
                        AddLog("Nėra Pokeball!");
                    }
                    else
                    {
                        _inventory.Pokeballs--;
                        double hpFrac   = (double)_wild.Hp / _wild.MaxHp;
                        int catchChance = (int)((1.0 - hpFrac) * 70) + 10;
                        if (_rng.Next(100) < catchChance)
                        {
                            _roster.Catch(_wild);
                            AddLog($"{_wild.Name} pagautas!");
                            RenderBattle();
                            WaitKey();
                            return BattleResult.PokemonCaught;
                        }
                        AddLog($"{_wild.Name} ištrūko iš Pokeball!");
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
                    }
                }
            }

            // Wild Pokemon attacks
            int enemyDmg       = Math.Max(1, _wild.Attack - _active.Defense + _rng.Next(-5, 6));
            int actualEnemyDmg = _active.TakeDamage(enemyDmg);
            AddLog($"{_wild.Name} puolė – {_active.Name} gavo {actualEnemyDmg} žalos.");

            if (!_active.IsAlive)
            {
                AddLog($"{_active.Name} krito!");
                var next = _roster.ActivePokemon; // first alive remaining
                if (next == null)
                {
                    AddLog("Visi Pokemon krito...");
                    RenderBattle();
                    WaitKey();
                    return BattleResult.PlayerLost;
                }
                _active = next;
                AddLog($"{_active.Name} eina į kovą!");
            }
        }
    }

    private void AddLog(string msg) => _log.Add(msg);

    private static int BattlePad() =>
        Math.Max(0, (Math.Max(82, Console.WindowWidth) - TotalW) / 2);

    private static void WaitKey()
    {
        int pad = BattlePad();
        Console.SetCursorPosition(pad, 23);
        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.Write("  Spausk bet kurį klavišą...");
        Console.ResetColor();
        Console.ReadKey(true);
    }

    // ── Rendering ────────────────────────────────────────────────

    private void RenderBattle()
    {
        int pad = BattlePad();
        string padding = new string(' ', pad);

        ConsoleColor wildHp   = HpColor(_wild);
        ConsoleColor playerHp = HpColor(_active);

        var moves = _active.Moves;

        // Build rows 11-16 depending on mode
        (string Text, ConsoleColor Color) row11, row12, row13, row14, row15, row16;

        if (!_selectingAttack)
        {
            row11 = ("  ┌──────────────────────┐", ConsoleColor.White);
            row12 = ("  │  [1] Pulti           │", ConsoleColor.White);
            row13 = ($"  │  [2] Vaistas (x{_inventory.Potions})   │".PadRight(27)[..27],
                      _inventory.Potions   > 0 ? ConsoleColor.White : ConsoleColor.DarkGray);
            row14 = ($"  │  [3] Pokeball (x{_inventory.Pokeballs})  │".PadRight(27)[..27],
                      _inventory.Pokeballs > 0 ? ConsoleColor.White : ConsoleColor.DarkGray);
            row15 = ("  │  [4] Bėgti           │", ConsoleColor.White);
            row16 = ("  └──────────────────────┘", ConsoleColor.White);
        }
        else
        {
            static (string, ConsoleColor) MoveRow(int i, IReadOnlyList<Move> mv)
            {
                if (i < mv.Count)
                    return ($"  │  [{i + 1}] {mv[i].Name,-12} {mv[i].Power,3} │", ConsoleColor.White);
                return ("  │  ---                  │", ConsoleColor.DarkGray);
            }

            row11 = ("  ┌──────────────────────┐", ConsoleColor.White);
            row12 = MoveRow(0, moves);
            row13 = MoveRow(1, moves);
            row14 = MoveRow(2, moves);
            row15 = MoveRow(3, moves);
            row16 = ("  └─── [Esc] Atgal ──────┘", ConsoleColor.DarkGray);
        }

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
            ($"  ☻ {_active.Name}",                                  ConsoleColor.Cyan),
            ($"  HP: {_active.Hp}/{_active.MaxHp}",                 playerHp),
            ($"  {_active.HpBar(LeftW - 4)}",                       playerHp),
            ("",                                                     ConsoleColor.White),
            row11,
            row12,
            row13,
            row14,
            row15,
            row16,
            ("",                                                     ConsoleColor.White),
        };

        // Paruošiame dešinę kolumną – istorija
        var right = BuildLogColumn(left.Length);

        // Viršutinė eilutė
        Border(padding, "╔", LeftW, "╦", RightW, "╗");
        RowColored(padding,
            $"{"POKEMON KOVA!",LeftW / 2 + 6}".PadRight(LeftW), ConsoleColor.Yellow,
            $" KOVOS ISTORIJA".PadRight(RightW),                 ConsoleColor.DarkYellow);
        Border(padding, "╠", LeftW, "╣", RightW, "║", secondFill: '─');

        for (int i = 0; i < left.Length; i++)
        {
            string lText = left[i].Text.PadRight(LeftW);
            if (lText.Length > LeftW) lText = lText[..LeftW];
            RowColored(padding, lText, left[i].Color, right[i], ConsoleColor.DarkGray);
        }

        Border(padding, "╚", LeftW, "╩", RightW, "╝");

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

    private void Border(string pad, string l, int lw, string mid, int rw, string r, char secondFill = '═')
    {
        _buf.Set(ConsoleColor.DarkCyan, ConsoleColor.Black);
        _buf.WriteLine(pad + l + new string('═', lw) + mid + new string(secondFill, rw) + r);
    }

    private void RowColored(string pad, string leftText, ConsoleColor lColor, string rightText, ConsoleColor rColor)
    {
        _buf.Set(ConsoleColor.DarkCyan, ConsoleColor.Black);
        _buf.Write(pad + "║");
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
        while (true)
        {
            var key = Console.ReadKey(intercept: true).Key;
            if (key == ConsoleKey.D1 || key == ConsoleKey.NumPad1) return 1;
            if (key == ConsoleKey.D2 || key == ConsoleKey.NumPad2) return 2;
            if (key == ConsoleKey.D3 || key == ConsoleKey.NumPad3) return 3;
            if (key == ConsoleKey.D4 || key == ConsoleKey.NumPad4) return 4;
        }
    }

    private Move? PromptMove()
    {
        var moves = _active.Moves;
        while (true)
        {
            var key = Console.ReadKey(intercept: true).Key;
            if (key == ConsoleKey.Escape) return null;

            int idx = key switch
            {
                ConsoleKey.D1 or ConsoleKey.NumPad1 => 0,
                ConsoleKey.D2 or ConsoleKey.NumPad2 => 1,
                ConsoleKey.D3 or ConsoleKey.NumPad3 => 2,
                ConsoleKey.D4 or ConsoleKey.NumPad4 => 3,
                _ => -1,
            };

            if (idx >= 0 && idx < moves.Count)
                return moves[idx];
        }
    }
}
