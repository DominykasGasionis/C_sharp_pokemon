namespace PokemonGame;

public enum TileType
{
    Path,
    TallGrass,
    Water,
    Tree,
    Wall,
    Building,
    Sand,
    Flower,
    HealCenter,
}

public class Map
{
    private readonly TileType[,] _tiles;
    public int Width  { get; }
    public int Height { get; }

    private static readonly Dictionary<TileType, char> TileChars;
    private static readonly Dictionary<TileType, (ConsoleColor Fg, ConsoleColor Bg)> TileColors;

    static Map()
    {
        TileChars = new Dictionary<TileType, char>
        {
            { TileType.Path,       '·' },
            { TileType.TallGrass,  '"' },
            { TileType.Water,      '~' },
            { TileType.Tree,       '^' },
            { TileType.Wall,       '█' },
            { TileType.Building,   '▪' },
            { TileType.Sand,       '·' },
            { TileType.Flower,     '✿' },
            { TileType.HealCenter, '✚' },
        };

        TileColors = new Dictionary<TileType, (ConsoleColor Fg, ConsoleColor Bg)>
        {
            { TileType.Path,       (ConsoleColor.DarkYellow, ConsoleColor.Black)   },
            { TileType.TallGrass,  (ConsoleColor.Green,      ConsoleColor.Black)   },
            { TileType.Water,      (ConsoleColor.Cyan,       ConsoleColor.DarkBlue)},
            { TileType.Tree,       (ConsoleColor.DarkGreen,  ConsoleColor.Black)   },
            { TileType.Wall,       (ConsoleColor.DarkGray,   ConsoleColor.Black)   },
            { TileType.Building,   (ConsoleColor.Gray,       ConsoleColor.DarkGray)},
            { TileType.Sand,       (ConsoleColor.Yellow,     ConsoleColor.Black)   },
            { TileType.Flower,     (ConsoleColor.Magenta,    ConsoleColor.Black)   },
            { TileType.HealCenter, (ConsoleColor.White,      ConsoleColor.DarkRed) },
        };
    }

    public bool IsPassable(int x, int y)
    {
        if (x < 0 || x >= Width || y < 0 || y >= Height)
            return false;

        return _tiles[y, x] switch
        {
            TileType.Water      => false,
            TileType.Tree       => false,
            TileType.Wall       => false,
            TileType.Building   => false,
            _                   => true,
        };
    }

    public TileType GetTile(int x, int y) => _tiles[y, x];

    public Map()
    {
        // Legenda:
        //  . = kelias   g = aukšta žolė   ~ = vanduo
        //  ^ = medis    B = pastatas      , = smėlis   * = gėlė
        string[] layout = new string[]
        {
            "^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^",
            "^.....................................................................^",
            "^.,,,,,,,,...BBB..^^^^^.........gggggggg...^^^^^........~~~~~~~~~~~..^",
            "^.,,,,,,,,..B.B...^...^.........gggggggg...^...^........~~~~~~~~~~~..^",
            "^.,,,,,,,,..BBB...^...^.........gggggggg...^...^........~~~~~~~~~~~..^",
            "^...,,*,..........^^^^^..................................~~~~~~~~~~~..^",
            "^...,,,,......................................................*.......^",
            "^...........^^^^^^^^^^....BBB.......gggggg..^^^^^^^^^...............^",
            "^...gggg....^........^...BH........gggggg..^.......^...BBB..BBB....^",
            "^...gggg....^.BBB....^...BBB.......gggggg..^.......^...B.B..B.B....^",
            "^...gggg....^.B.B....^.............gggggg..^.......^...BBB..BBB....^",
            "^...gggg....^.BBB....^.................................^.............^",
            "^...........^........^.................................^^^^^^^^^.....^",
            "^...........^^^^^^^^^^^^.........,,,,,,*,,.......................*...^",
            "^...........................................................,,,,,,,...^",
            "^.....................................................................^",
            "^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^",
        };

        Height = layout.Length;
        Width  = layout[0].Length;
        _tiles = new TileType[Height, Width];

        for (int y = 0; y < Height; y++)
        {
            for (int x = 0; x < Width && x < layout[y].Length; x++)
            {
                _tiles[y, x] = layout[y][x] switch
                {
                    'g' => TileType.TallGrass,
                    '~' => TileType.Water,
                    '^' => TileType.Tree,
                    'B' => TileType.Building,
                    '#' => TileType.Wall,
                    ',' => TileType.Sand,
                    '*' => TileType.Flower,
                    'H' => TileType.HealCenter,
                    _   => TileType.Path,
                };
            }
        }
    }

    public void Render(ScreenBuffer buf, int playerX, int playerY, int leftPad = 0, int rightPad = 0)
    {
        string pad  = new string(' ', leftPad);
        string rpad = new string(' ', rightPad);

        // Viršutinis rėmelis
        buf.Set(ConsoleColor.DarkGray, ConsoleColor.Black);
        buf.WriteLine(pad + "┌" + new string('─', Width) + "┐" + rpad);

        for (int y = 0; y < Height; y++)
        {
            buf.Set(ConsoleColor.DarkGray, ConsoleColor.Black);
            buf.Write(pad + "│");

            for (int x = 0; x < Width; x++)
            {
                if (x == playerX && y == playerY)
                {
                    buf.Set(ConsoleColor.White, ConsoleColor.Black);
                    buf.Write('@');
                }
                else
                {
                    var tile = _tiles[y, x];
                    var (fg, bg) = TileColors[tile];
                    buf.Set(fg, bg);
                    buf.Write(TileChars[tile]);
                }
            }

            buf.Set(ConsoleColor.DarkGray, ConsoleColor.Black);
            buf.WriteLine("│" + rpad);
        }

        // Apatinis rėmelis
        buf.Set(ConsoleColor.DarkGray, ConsoleColor.Black);
        buf.WriteLine(pad + "└" + new string('─', Width) + "┘" + rpad);
    }
}
