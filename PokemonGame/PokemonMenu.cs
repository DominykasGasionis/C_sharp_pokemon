namespace PokemonGame;

public class PokemonMenu
{
    private readonly PokemonRoster _roster;
    private readonly ScreenBuffer  _buf = new();
    private int     _selected = 0;
    private string  _message  = "Pasirinkite Pokemon.";

    // When party is full and user chose a box Pokemon to equip
    private bool     _pickingSlot   = false;
    private Pokemon? _pendingPokemon = null;

    private const int W = 70; // inner width

    public PokemonMenu(PokemonRoster roster) => _roster = roster;

    public void Run()
    {
        ClearScreen();
        while (true)
        {
            Render();
            var key = Console.ReadKey(intercept: true).Key;

            if (_pickingSlot)
            {
                HandleSlotPick(key);
            }
            else
            {
                if (key == ConsoleKey.Escape) { ClearScreen(); return; }
                HandleNavigation(key);
            }
        }
    }

    // ── Input ─────────────────────────────────────────────────────────

    private void HandleNavigation(ConsoleKey key)
    {
        var box = BoxPokemon();
        int total = 3 + box.Count;

        switch (key)
        {
            case ConsoleKey.W or ConsoleKey.UpArrow:
                _selected = Math.Max(0, _selected - 1);
                _message  = "";
                break;

            case ConsoleKey.S or ConsoleKey.DownArrow:
                _selected = Math.Min(total - 1, _selected + 1);
                _message  = "";
                break;

            case ConsoleKey.Enter or ConsoleKey.Spacebar:
                HandleSelect(box);
                break;
        }
    }

    private void HandleSelect(List<Pokemon> box)
    {
        if (_selected < 3)
        {
            // Party slot
            var pokemon = _roster.Party[_selected];
            if (pokemon == null) { _message = "Šis lizdas tuščias."; return; }

            // Prevent emptying the entire party
            bool wouldBeEmpty = _roster.Party.Count(p => p != null) <= 1;
            if (wouldBeEmpty) { _message = "Negalima pašalinti paskutinio Pokemon!"; return; }

            _roster.Party[_selected] = null;
            _message = $"{pokemon.Name} pašalintas iš partijos.";
        }
        else
        {
            // Box Pokemon – equip
            var pokemon = box[_selected - 3];
            int freeSlot = Array.IndexOf(_roster.Party, null);

            if (freeSlot >= 0)
            {
                _roster.Party[freeSlot] = pokemon;
                _message = $"{pokemon.Name} ekipuotas į poziciją {freeSlot + 1}.";
            }
            else
            {
                _pendingPokemon = pokemon;
                _pickingSlot    = true;
                _message = $"Partija pilna! [1][2][3] – kurią poziciją pakeisti? [Esc] atšaukti";
            }
        }
    }

    private void HandleSlotPick(ConsoleKey key)
    {
        int slot = key switch
        {
            ConsoleKey.D1 or ConsoleKey.NumPad1 => 0,
            ConsoleKey.D2 or ConsoleKey.NumPad2 => 1,
            ConsoleKey.D3 or ConsoleKey.NumPad3 => 2,
            _ => -1,
        };

        if (key == ConsoleKey.Escape)
        {
            _pickingSlot    = false;
            _pendingPokemon = null;
            _message = "Atšaukta.";
            return;
        }

        if (slot < 0) return;

        _roster.Party[slot] = _pendingPokemon;
        _message = $"{_pendingPokemon!.Name} ekipuotas į poziciją {slot + 1}.";
        _pickingSlot    = false;
        _pendingPokemon = null;
    }

    // ── Helpers ───────────────────────────────────────────────────────

    private List<Pokemon> BoxPokemon()
    {
        // LINQ Where: filtruoja tik tuos Pokemon kurie nėra party nariai (dėžutės Pokemon)
        var box = _roster.All.Where(p => !_roster.Party.Contains(p)).ToList();
        box.Sort(); // naudoja Pokemon.CompareTo – rūšiuoja pagal lygį
        return box;
    }

    // ── Rendering ────────────────────────────────────────────────────

    private void Render()
    {
        int termW = Math.Max(82, Console.WindowWidth);
        int termH = Math.Max(30, Console.WindowHeight);
        int pad   = Math.Max(0, (termW - W - 2) / 2);
        string p  = new string(' ', pad);

        var box      = BoxPokemon();
        int lineCount = 0;

        void HBorder(char l, char fill, char r)
        {
            _buf.Set(ConsoleColor.DarkCyan, ConsoleColor.Black);
            _buf.WriteLine(p + l + new string(fill, W) + r);
            lineCount++;
        }

        void HRow(string text, ConsoleColor color)
        {
            if (text.Length > W) text = text[..W];
            _buf.Set(ConsoleColor.DarkCyan, ConsoleColor.Black);
            _buf.Write(p + "║");
            _buf.SetFg(color);
            _buf.Write(text.PadRight(W));
            _buf.Set(ConsoleColor.DarkCyan, ConsoleColor.Black);
            _buf.WriteLine("║");
            lineCount++;
        }

        string PokemonRow(string slotTag, bool selected, Pokemon? poke)
        {
            string cursor = selected ? "►" : " ";
            if (poke == null)
                return $"  {slotTag} {cursor} (tuščia)";

            var (pokeName, hp, maxHp) = poke; // Deconstruct
            string name   = pokeName.PadRight(13);
            int    xpLeft = poke.ExperienceToNextLevel - poke.Experience;
            return $"  {slotTag} {cursor} {name} Lv{poke.Level,-2}  HP: {hp,3}/{maxHp,-3}  XP: {poke.Experience}/{poke.ExperienceToNextLevel} (liko {xpLeft})";
        }

        // Header
        HBorder('╔', '═', '╗');
        HRow($"{"POKEMON VALDYMAS",W / 2 + 8}".PadRight(W), ConsoleColor.Yellow);
        HBorder('╠', '─', '╣');

        // Party
        HRow("  PARTIJA", ConsoleColor.DarkGray);
        for (int i = 0; i < 3; i++)
        {
            bool sel    = !_pickingSlot && _selected == i;
            var  poke   = _roster.Party[i];
            string row  = PokemonRow($"[{i + 1}]", sel, poke);
            ConsoleColor color = sel ? ConsoleColor.Cyan
                : (poke == null || !poke.IsAlive) ? ConsoleColor.DarkGray : ConsoleColor.White;
            HRow(row, color);
        }

        HBorder('╠', '─', '╣');

        // Box
        HRow("  DĖŽĖ", ConsoleColor.DarkGray);

        if (box.Count == 0)
        {
            HRow("  (dėžė tuščia)", ConsoleColor.DarkGray);
        }
        else
        {
            for (int i = 0; i < box.Count; i++)
            {
                bool sel    = !_pickingSlot && _selected == 3 + i;
                string row  = PokemonRow("   ", sel, box[i]);
                ConsoleColor color = sel ? ConsoleColor.Cyan
                    : (!box[i].IsAlive ? ConsoleColor.DarkGray : ConsoleColor.White);
                HRow(row, color);
            }
        }

        HBorder('╠', '═', '╣');
        HRow($"  {_message}", ConsoleColor.White);
        HBorder('╠', '─', '╣');
        HRow("  [↑↓/WS] Naršyti   [Enter] Ekipuoti/Pašalinti   [Esc] Atgal", ConsoleColor.DarkGray);
        HBorder('╚', '═', '╝');

        // Clear leftover lines from previous (longer) renders
        string blankLine = new string(' ', termW);
        for (int i = lineCount; i < termH - 1; i++)
            _buf.WriteLine(blankLine);

        _buf.Flush();
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
}
