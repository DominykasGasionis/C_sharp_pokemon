namespace PokemonGame;

public class PokemonSelector
{
    private readonly (string Name, int Hp, int Atk, int Def)[] _choices;
    private int _selected = 0;

    public PokemonSelector(Random rng)
    {
        // Išrinkti 3 atsitiktinius unikalius starteriusvar pool = Pokemon.StarterPool.ToList();
        var pool = Pokemon.StarterPool.ToList();
        _choices = new (string, int, int, int)[3];
        for (int i = 0; i < 3; i++)
        {
            int idx = rng.Next(pool.Count);
            _choices[i] = pool[idx];
            pool.RemoveAt(idx);
        }
    }

    // Grąžina pasirinktą Pokemon arba null jei grįžta atgal
    public Pokemon? Run()
    {
        while (true)
        {
            Render();
            var key = Console.ReadKey(intercept: true).Key;

            switch (key)
            {
                case ConsoleKey.LeftArrow or ConsoleKey.A:
                    _selected = (_selected - 1 + 3) % 3;
                    break;

                case ConsoleKey.RightArrow or ConsoleKey.D:
                    _selected = (_selected + 1) % 3;
                    break;

                case ConsoleKey.Enter or ConsoleKey.Spacebar:
                    var c = _choices[_selected];
                    return new Pokemon(c.Name, c.Hp, c.Atk, c.Def);

                case ConsoleKey.Escape or ConsoleKey.Backspace:
                    return null;
            }
        }
    }

    private void Render()
    {
        Console.Clear();
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("\n  Pasirink savo Pokemon!\n");
        Console.ResetColor();

        for (int i = 0; i < 3; i++)
        {
            var (name, hp, atk, def) = _choices[i];
            bool active = i == _selected;

            Console.ForegroundColor = active ? ConsoleColor.Yellow : ConsoleColor.DarkGray;
            string border = active ? "╔══════════════╗" : "┌──────────────┐";
            string side   = active ? "║" : "│";
            string end    = active ? "╚══════════════╝" : "└──────────────┘";

            Console.SetCursorPosition(i * 20 + 2, 3);
            Console.Write(border);
            WriteCardLine(i * 20 + 2, 4, side, $"  {name,-12}", active);
            WriteCardLine(i * 20 + 2, 5, side, $"  HP:  {hp,-8}", active);
            WriteCardLine(i * 20 + 2, 6, side, $"  ATK: {atk,-8}", active);
            WriteCardLine(i * 20 + 2, 7, side, $"  DEF: {def,-8}", active);
            Console.SetCursorPosition(i * 20 + 2, 8);
            Console.ForegroundColor = active ? ConsoleColor.Yellow : ConsoleColor.DarkGray;
            Console.Write(end);

            if (active)
            {
                Console.SetCursorPosition(i * 20 + 8, 9);
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write("▲");
            }
        }

        Console.SetCursorPosition(0, 12);
        Console.ResetColor();
        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.WriteLine("  [←/→] Pasirinkti   [Enter] Patvirtinti   [Esc] Grįžti");
        Console.ResetColor();
    }

    private static void WriteCardLine(int col, int row, string side, string content, bool active)
    {
        Console.SetCursorPosition(col, row);
        Console.ForegroundColor = active ? ConsoleColor.Yellow : ConsoleColor.DarkGray;
        Console.Write(side);
        Console.ForegroundColor = active ? ConsoleColor.White : ConsoleColor.DarkGray;
        Console.Write(content);
        Console.ForegroundColor = active ? ConsoleColor.Yellow : ConsoleColor.DarkGray;
        Console.Write(side);
    }
}
