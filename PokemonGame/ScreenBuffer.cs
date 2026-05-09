namespace PokemonGame;

using System.Text;

/// <summary>
/// Kaupia visą kadrą StringBuilder'yje su ANSI spalvų kodais
/// ir išveda vienu Console.Write – be mirksėjimo.
/// </summary>
// Uždaryta (sealed) klasė – negali būti paveldima
public sealed class ScreenBuffer
{
    private readonly StringBuilder _sb = new(32768);
    private int _lastFg = -1;
    private int _lastBg = -1;

    // ConsoleColor (int) indeksas → ANSI fg kodas
    private static readonly string[] FgCodes =
    {
        "\x1B[30m",  // Black       = 0
        "\x1B[34m",  // DarkBlue    = 1
        "\x1B[32m",  // DarkGreen   = 2
        "\x1B[36m",  // DarkCyan    = 3
        "\x1B[31m",  // DarkRed     = 4
        "\x1B[35m",  // DarkMagenta = 5
        "\x1B[33m",  // DarkYellow  = 6
        "\x1B[37m",  // Gray        = 7
        "\x1B[90m",  // DarkGray    = 8
        "\x1B[94m",  // Blue        = 9
        "\x1B[92m",  // Green       = 10
        "\x1B[96m",  // Cyan        = 11
        "\x1B[91m",  // Red         = 12
        "\x1B[95m",  // Magenta     = 13
        "\x1B[93m",  // Yellow      = 14
        "\x1B[97m",  // White       = 15
    };

    // ConsoleColor (int) indeksas → ANSI bg kodas
    private static readonly string[] BgCodes =
    {
        "\x1B[40m",   // Black       = 0
        "\x1B[44m",   // DarkBlue    = 1
        "\x1B[42m",   // DarkGreen   = 2
        "\x1B[46m",   // DarkCyan    = 3
        "\x1B[41m",   // DarkRed     = 4
        "\x1B[45m",   // DarkMagenta = 5
        "\x1B[43m",   // DarkYellow  = 6
        "\x1B[47m",   // Gray        = 7
        "\x1B[100m",  // DarkGray    = 8
        "\x1B[104m",  // Blue        = 9
        "\x1B[102m",  // Green       = 10
        "\x1B[106m",  // Cyan        = 11
        "\x1B[101m",  // Red         = 12
        "\x1B[105m",  // Magenta     = 13
        "\x1B[103m",  // Yellow      = 14
        "\x1B[107m",  // White       = 15
    };

    public void SetFg(ConsoleColor c)
    {
        int i = (int)c;
        if (i != _lastFg) { _sb.Append(FgCodes[i]); _lastFg = i; }
    }

    public void SetBg(ConsoleColor c)
    {
        int i = (int)c;
        if (i != _lastBg) { _sb.Append(BgCodes[i]); _lastBg = i; }
    }

    public void Set(ConsoleColor fg, ConsoleColor bg = ConsoleColor.Black)
    {
        SetFg(fg);
        SetBg(bg);
    }

    public void Write(string text) => _sb.Append(text);
    public void Write(char c)      => _sb.Append(c);

    public void WriteLine(string text)
    {
        _sb.Append(text);
        _sb.Append('\n');
        _lastBg = -1; // kai kurie terminalai reset'ina bg po newline
    }

    public void WriteLine() { _sb.Append('\n'); _lastBg = -1; }

    public void Reset()
    {
        _sb.Append("\x1B[0m");
        _lastFg = -1;
        _lastBg = -1;
    }

    /// <summary>Perkelia kursorių į (0,0) ir vienu Write išveda visą kadrą.</summary>
    public void Flush()
    {
        _sb.Append("\x1B[0m"); // reset spalvų pabaigoje
        Console.SetCursorPosition(0, 0);
        Console.Write(_sb);
        _sb.Clear();
        _lastFg = -1;
        _lastBg = -1;
    }
}
