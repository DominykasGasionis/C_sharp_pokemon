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
    private Pokemon _active;
    private bool    _selectingAttack  = false;
    private bool    _selectingPokemon = false;
    private bool    _waitForKey       = false;
    private int     _menuRow = 0, _menuCol    = 0; // main menu cursor
    private int     _moveRow = 0, _moveCol    = 0; // move menu cursor
    private int     _pokemonRow = 0;               // party switch cursor

    private const int LeftW  = 44;
    private const int RightW = 40;
    private const int TotalW = 1 + LeftW + 1 + RightW + 1; // 87

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
                if (move == null) { _selectingAttack = false; continue; }

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
                    WaitKey();
                    return BattleResult.PlayerWon;
                }
            }
            else if (_selectingPokemon)
            {
                var newPoke = PromptPokemon();
                _selectingPokemon = false;
                if (newPoke == null)            { continue; }
                if (!newPoke.IsAlive)           { AddLog($"{newPoke.Name} negali kovoti!"); continue; }
                if (newPoke == _active)         { AddLog($"{_active.Name} jau kovoja!"); continue; }
                _active = newPoke;
                AddLog($"{_active.Name} eina į kovą!");
                // costs a turn – wild attacks below
            }
            else
            {
                var choice = PromptChoice();

                if (choice == 1) // FIGHT
                {
                    _selectingAttack = true;
                    _moveRow = 0; _moveCol = 0;
                    continue;
                }
                else if (choice == 2) // POKEMON
                {
                    _selectingPokemon = true;
                    _pokemonRow = 0;
                    continue;
                }
                else if (choice == 3) // BAG
                {
                    if (_inventory.Potions <= 0)
                        AddLog("Nėra vaistų!");
                    else
                    {
                        int healed = _active.Heal(20);
                        _inventory.Potions--;
                        AddLog($"{_active.Name} išgijo {healed} HP.");
                    }
                }
                else if (choice == 4) // POKEBALL
                {
                    if (_inventory.Pokeballs <= 0)
                        AddLog("Nėra Pokeball!");
                    else
                    {
                        _inventory.Pokeballs--;
                        double hpFrac   = (double)_wild.Hp / _wild.MaxHp;
                        int catchChance = (int)((1.0 - hpFrac) * 70) + 10;
                        if (_rng.Next(100) < catchChance)
                        {
                            _roster.Catch(_wild);
                            AddLog($"{_wild.Name} pagautas!");
                            WaitKey();
                            return BattleResult.PokemonCaught;
                        }
                        AddLog($"{_wild.Name} ištrūko iš Pokeball!");
                    }
                }
                else if (choice == 5) // RUN
                {
                    if (_rng.Next(100) < 60)
                    {
                        AddLog("Pabėgote sėkmingai!");
                        WaitKey();
                        return BattleResult.PlayerFled;
                    }
                    AddLog("Nepavyko pabėgti!");
                }
                else { continue; } // invalid cell (no action)
            }

            // Nuodų žala prieš laukinio ataką
            if (_active.HasStatus(StatusEffect.Poisoned))
            {
                int poisonDmg = Math.Max(1, _active.MaxHp / 8);
                _active.TakeDamage(poisonDmg);
                AddLog($"{_active.Name} kenčia nuo nuodų! -{poisonDmg} HP.");
                if (!_active.IsAlive) { AddLog($"{_active.Name} krito nuo nuodų!"); goto checkFaint; }
            }

            // Wild attacks
            {
                // Paralyžius: 30% šansas praleisti ataką
                if (_wild.HasStatus(StatusEffect.Paralyzed) && _rng.Next(100) < 30)
                {
                    AddLog($"{_wild.Name} paralyžiuotas ir negali atakuoti!");
                }
                else
                {
                    int enemyDmg       = Math.Max(1, _wild.Attack - _active.Defense + _rng.Next(-5, 6));
                    int actualEnemyDmg = _active.TakeDamage(enemyDmg);
                    AddLog($"{_wild.Name} puolė – {_active.Name} gavo {actualEnemyDmg} žalos.");

                    // 15% šansas apnuodyti arba paralyžiuoti
                    if (!_active.HasStatus(StatusEffect.Poisoned) && _rng.Next(100) < 15)
                    {
                        _active.ApplyStatus(StatusEffect.Poisoned);
                        AddLog($"{_active.Name} apnuodytas!");
                    }
                    else if (!_wild.HasStatus(StatusEffect.Paralyzed) && _rng.Next(100) < 10)
                    {
                        _wild.ApplyStatus(StatusEffect.Paralyzed);
                        AddLog($"{_wild.Name} paralyžiuotas!");
                    }
                }
            }

            checkFaint:

            if (!_active.IsAlive)
            {
                AddLog($"{_active.Name} krito!");
                var next = _roster.ActivePokemon;
                if (next == null)
                {
                    AddLog("Visi Pokemon krito...");
                    WaitKey();
                    return BattleResult.PlayerLost;
                }
                _active = next;
                AddLog($"{_active.Name} eina į kovą!");
            }
        }
    }

    private void AddLog(string msg) => _log.Add(msg);

    private void WaitKey()
    {
        _waitForKey = true;
        RenderBattle();
        _waitForKey = false;
        Console.ReadKey(true);
    }

    private static int BattlePad() =>
        Math.Max(0, (Math.Max(90, Console.WindowWidth) - TotalW) / 2);

    // ── Rendering ────────────────────────────────────────────────────

    private void RenderBattle()
    {
        int    pad  = BattlePad();
        string pStr = new string(' ', pad);
        int    fW   = TotalW - 2; // 69 – full field inner width

        ConsoleColor wildColor   = HpColor(_wild);
        ConsoleColor activeColor = HpColor(_active);

        // Wild Pokemon info (top-left of field)
        string wildNameRow = $"  ~ {_wild.Name,-15} Lv{_wild.Level}";
        string wildBarRow  = $"  HP {_wild.HpBar(30)}";

        // Player Pokemon info (bottom-right of field, right-aligned)
        string statusTag = _active.Status == StatusEffect.None ? "" :
                           _active.HasStatus(StatusEffect.Poisoned)  ? " [PSN]" : " [PAR]";
        string plName    = $"* {_active.Name,-12} Lv{_active.Level}{statusTag}";
        string plHp      = $"HP {_active.HpBar(18)} {_active.Hp}/{_active.MaxHp}";
        string plNameRow = plName.PadLeft(fW - 2).PadRight(fW);
        string plHpRow   = plHp.PadLeft(fW - 2).PadRight(fW);

        // Bottom-left message panel (3 lines)
        string line1, line2, line3;
        if (_waitForKey)
        {
            line1 = ""; line2 = "";
            line3 = "  Spausk bet kurį klavišą...";
        }
        else if (_selectingAttack)
        {
            line1 = $"  Ko imsis {_active.Name}?";
            line2 = ""; line3 = "";
        }
        else if (_selectingPokemon)
        {
            line1 = "  Kurį Pokemon siųsti į kovą?";
            line2 = ""; line3 = "  [Esc] Atgal";
        }
        else if (_log.Count == 0)
        {
            line1 = ""; line2 = $"  Ko imsis {_active.Name}?"; line3 = "";
        }
        else
        {
            // Range [^n..] – paskutinės iki 3 žurnalo žinučių
            string[] recent = _log.ToArray()[^Math.Min(_log.Count, 3)..];
            line1 = recent.Length > 2 ? $"  {recent[0]}"               : "";
            line2 = recent.Length > 1 ? $"  {recent[recent.Length - 2]}" : "";
            line3 = recent.Length > 0 ? $"  {recent[^1]}"              : "";
        }
        string Clip(string s) => s.Length > LeftW - 1 ? s[..(LeftW - 4)] + "..." : s;
        string msgRow1 = Clip(line1).PadRight(LeftW);
        string msgRow2 = Clip(line2).PadRight(LeftW);
        string msgRow3 = Clip(line3).PadRight(LeftW);

        // Bottom-right menu panel (3 rows)
        string menuR1, menuR2, menuR3;
        if (_selectingAttack)
        {
            var m = _active.Moves;
            menuR1 = MoveSlots(m, 0).PadRight(RightW);
            menuR2 = MoveSlots(m, 1).PadRight(RightW);
            menuR3 = "  [Esc] Atgal".PadRight(RightW);
        }
        else if (_selectingPokemon)
        {
            menuR1 = PartySlot(0).PadRight(RightW);
            menuR2 = PartySlot(1).PadRight(RightW);
            menuR3 = PartySlot(2).PadRight(RightW);
        }
        else
        {
            string MC(int r, int c) => (_menuRow == r && _menuCol == c) ? ">" : " ";
            menuR1 = $" {MC(0,0)}{"FIGHT",-18} {MC(0,1)}{"POKEMON",-18}".PadRight(RightW);
            menuR2 = $" {MC(1,0)}{$"BAG({_inventory.Potions})",-18} {MC(1,1)}{$"POKEBALL({_inventory.Pokeballs})",-18}".PadRight(RightW);
            menuR3 = $" {MC(2,0)}{"RUN",-38}".PadRight(RightW);
        }

        // ── Vertical centering ───────────────────────────────────────
        // field: 1blank+name+bar+2blank+name+bar+1blank = 8 rows
        // total: 1(top) + 8(field) + 1(div) + 5(bottom) + 1(bot) = 16
        const int battleRows = 16;
        int termW    = Math.Max(82, Console.WindowWidth);
        int termH    = Math.Max(battleRows + 2, Console.WindowHeight);
        int topMargin = Math.Max(0, (termH - battleRows) / 2);
        string blankLine = new string(' ', termW);
        for (int i = 0; i < topMargin; i++) _buf.WriteLine(blankLine);

        string sp    = new string(' ', fW);
        string bsL   = new string(' ', LeftW);
        string bsR   = new string(' ', RightW);

        // ── Draw ─────────────────────────────────────────────────────
        _buf.Set(ConsoleColor.DarkCyan, ConsoleColor.Black);
        _buf.WriteLine(pStr + "╔" + new string('═', fW) + "╗");

        FR(pStr, fW, sp,           ConsoleColor.White);    // blank
        FR(pStr, fW, wildNameRow,  ConsoleColor.White);
        FR(pStr, fW, wildBarRow,   wildColor);
        FR(pStr, fW, sp,           ConsoleColor.White);    // blank
        FR(pStr, fW, sp,           ConsoleColor.White);    // blank
        FR(pStr, fW, plNameRow,    ConsoleColor.Cyan);
        FR(pStr, fW, plHpRow,      activeColor);
        FR(pStr, fW, sp,           ConsoleColor.White);    // blank

        _buf.Set(ConsoleColor.DarkCyan, ConsoleColor.Black);
        _buf.WriteLine(pStr + "╠" + new string('═', LeftW) + "╦" + new string('═', RightW) + "╣");

        RowColored(pStr, bsL,     ConsoleColor.White,    bsR,    ConsoleColor.White);  // blank
        RowColored(pStr, msgRow1, ConsoleColor.DarkGray, menuR1, ConsoleColor.White);
        RowColored(pStr, msgRow2, ConsoleColor.DarkGray, menuR2, ConsoleColor.White);
        RowColored(pStr, msgRow3, ConsoleColor.White,    menuR3, ConsoleColor.White);
        RowColored(pStr, bsL,     ConsoleColor.White,    bsR,    ConsoleColor.White);  // blank

        _buf.Set(ConsoleColor.DarkCyan, ConsoleColor.Black);
        _buf.WriteLine(pStr + "╚" + new string('═', LeftW) + "╩" + new string('═', RightW) + "╝");

        // Clear leftover rows from map/HUD below the battle box
        int rendered = topMargin + battleRows;
        int fillTo   = Math.Max(rendered + 5, Console.WindowHeight);
        for (int i = rendered; i < fillTo; i++) _buf.WriteLine(blankLine);

        _buf.Flush();
    }

    // Full-width field row
    private void FR(string pStr, int fW, string content, ConsoleColor color)
    {
        string text = content.Length >= fW ? content[..fW] : content.PadRight(fW);
        _buf.Set(ConsoleColor.DarkCyan, ConsoleColor.Black);
        _buf.Write(pStr + "║");
        _buf.SetFg(color);
        _buf.Write(text);
        _buf.Set(ConsoleColor.DarkCyan, ConsoleColor.Black);
        _buf.WriteLine("║");
    }

    // Split bottom-panel row
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

    // Two move slots for a given row, cursor driven by _moveRow/_moveCol
    private string MoveSlots(IReadOnlyList<Move> m, int rowIdx)
    {
        string Slot(int col)
        {
            int i = rowIdx * 2 + col;
            string c = (_moveRow == rowIdx && _moveCol == col) ? ">" : " ";
            return i < m.Count
                ? $"{c}{m[i].Name,-14}{m[i].Power,3}"
                : $"{c}{"---",-17}";
        }
        return $" {Slot(0)}   {Slot(1)}";
    }

    private static ConsoleColor HpColor(Pokemon p) =>
        p.Hp > p.MaxHp / 2 ? ConsoleColor.Green :
        p.Hp > p.MaxHp / 4 ? ConsoleColor.Yellow : ConsoleColor.Red;

    // Party slot row for pokemon-switch panel
    private string PartySlot(int idx)
    {
        string c = (_pokemonRow == idx) ? ">" : " ";
        var p = _roster.Party[idx];
        if (p == null) return $" {c}[{idx + 1}] (tuščia)";
        string status = p == _active ? " [kovoja]" : (!p.IsAlive ? " [KO]" : "");
        return $" {c}[{idx + 1}] {p.Name,-10} Lv{p.Level,-2} HP {p.Hp}/{p.MaxHp}{status}";
    }

    private int PromptChoice()
    {
        while (true)
        {
            var key = Console.ReadKey(intercept: true).Key;
            switch (key)
            {
                case ConsoleKey.W or ConsoleKey.UpArrow:
                    _menuRow = Math.Max(0, _menuRow - 1); RenderBattle(); break;
                case ConsoleKey.S or ConsoleKey.DownArrow:
                    _menuRow = Math.Min(2, _menuRow + 1);
                    if (_menuRow == 2) _menuCol = 0; // only RUN on row 2
                    RenderBattle(); break;
                case ConsoleKey.A or ConsoleKey.LeftArrow:
                    _menuCol = Math.Max(0, _menuCol - 1); RenderBattle(); break;
                case ConsoleKey.D or ConsoleKey.RightArrow:
                    if (_menuRow < 2) _menuCol = Math.Min(1, _menuCol + 1);
                    RenderBattle(); break;
                case ConsoleKey.Enter or ConsoleKey.Spacebar:
                    return _menuRow * 2 + _menuCol + 1;
                case ConsoleKey.D1 or ConsoleKey.NumPad1: return 1;
                case ConsoleKey.D2 or ConsoleKey.NumPad2: return 2;
                case ConsoleKey.D3 or ConsoleKey.NumPad3: return 3;
                case ConsoleKey.D4 or ConsoleKey.NumPad4: return 4;
                case ConsoleKey.D5 or ConsoleKey.NumPad5: return 5;
            }
        }
    }

    private Pokemon? PromptPokemon()
    {
        while (true)
        {
            var key = Console.ReadKey(intercept: true).Key;
            switch (key)
            {
                case ConsoleKey.Escape: return null;
                case ConsoleKey.W or ConsoleKey.UpArrow:
                    _pokemonRow = Math.Max(0, _pokemonRow - 1); RenderBattle(); break;
                case ConsoleKey.S or ConsoleKey.DownArrow:
                    _pokemonRow = Math.Min(2, _pokemonRow + 1); RenderBattle(); break;
                case ConsoleKey.Enter or ConsoleKey.Spacebar:
                    return _roster.Party[_pokemonRow];
                case ConsoleKey.D1 or ConsoleKey.NumPad1: return _roster.Party[0];
                case ConsoleKey.D2 or ConsoleKey.NumPad2: return _roster.Party[1];
                case ConsoleKey.D3 or ConsoleKey.NumPad3: return _roster.Party[2];
            }
        }
    }

    private Move? PromptMove()
    {
        var moves = _active.Moves;
        while (true)
        {
            var key = Console.ReadKey(intercept: true).Key;
            switch (key)
            {
                case ConsoleKey.Escape: return null;
                case ConsoleKey.W or ConsoleKey.UpArrow:
                    _moveRow = Math.Max(0, _moveRow - 1); RenderBattle(); break;
                case ConsoleKey.S or ConsoleKey.DownArrow:
                    _moveRow = Math.Min(1, _moveRow + 1); RenderBattle(); break;
                case ConsoleKey.A or ConsoleKey.LeftArrow:
                    _moveCol = Math.Max(0, _moveCol - 1); RenderBattle(); break;
                case ConsoleKey.D or ConsoleKey.RightArrow:
                    _moveCol = Math.Min(1, _moveCol + 1); RenderBattle(); break;
                case ConsoleKey.Enter or ConsoleKey.Spacebar:
                    int idx = _moveRow * 2 + _moveCol;
                    if (idx < moves.Count) return moves[idx];
                    break;
                case ConsoleKey.D1 or ConsoleKey.NumPad1: if (moves.Count > 0) return moves[0]; break;
                case ConsoleKey.D2 or ConsoleKey.NumPad2: if (moves.Count > 1) return moves[1]; break;
                case ConsoleKey.D3 or ConsoleKey.NumPad3: if (moves.Count > 2) return moves[2]; break;
                case ConsoleKey.D4 or ConsoleKey.NumPad4: if (moves.Count > 3) return moves[3]; break;
            }
        }
    }
}
