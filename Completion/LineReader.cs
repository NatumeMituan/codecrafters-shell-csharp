using System.Text;

namespace Codecrafters.Shell.Completion;

internal static class LineReader
{
    private const char BellCharacter = '\x07';
    private static readonly StringBuilder Line = new();
    private static readonly Context ctx = new();
    private static IReadOnlyList<string> history = [];
    private static int historyIndex;

    public static string ReadLine(string prompt, IReadOnlyList<string> history)
    {
        Line.Clear();
        ctx.WasMultiCompletions = false;
        LineReader.history = history;
        historyIndex = history.Count;

        Console.Write(prompt);
        ConsoleKeyInfo keyInfo;

        do
        {
            ctx.IsMultiCompletions = false;
            keyInfo = Console.ReadKey(intercept: true);

            if (keyInfo.Key == ConsoleKey.Backspace && Line.Length > 0)
            {
                HandleBackspace();
            }
            else if (keyInfo.Key == ConsoleKey.Tab)
            {
                HandleTab(prompt);
            }
            else if (keyInfo.Key == ConsoleKey.UpArrow)
            {
                HandleUpArrow();
            }
            else if (keyInfo.Key == ConsoleKey.DownArrow)
            {
                HandleDownArrow();
            }
            else if (!char.IsControl(keyInfo.KeyChar))
            {
                HandleCharacter(keyInfo.KeyChar);
            }

            ctx.WasMultiCompletions = ctx.IsMultiCompletions;
        } while (keyInfo.Key != ConsoleKey.Enter);

        Console.WriteLine();
        return Line.ToString();
    }

    private static void HandleBackspace()
    {
        if (Line.Length > 0)
        {
            Line.Length--;
            Console.Write("\b \b");
        }
    }

    private static void HandleTab(string prompt)
    {
        var currentLine = Line.ToString();
        var completions = CompletionProvider.GetCompletions(currentLine);

        if (completions.Count == 0)
        {
            Console.Write(BellCharacter);
        }
        else if (completions.Count == 1)
        {
            var completion = $"{completions[0][currentLine.Length..]} ";
            Write(completion);
        }
        else
        {
            HandleMultipleCompletions(prompt, currentLine, completions);
        }
    }

    private static void HandleMultipleCompletions(
        string prompt,
        string currentLine,
        List<string> completions)
    {

        if (ctx.WasMultiCompletions)
        {
            Console.WriteLine();
            Console.WriteLine(string.Join("  ", completions.OrderBy(c => c)));
            Console.Write($"{prompt}{Line}");
        }
        else
        {
            var longestCommonPrefix = GetLongestCommonPrefix(currentLine.Length, completions);
            if (longestCommonPrefix?.Length > currentLine.Length)
            {
                var completion = longestCommonPrefix[currentLine.Length..];
                Write(completion);
            }
            else
            {
                ctx.IsMultiCompletions = true;
                Console.Write(BellCharacter);
            }
        }
    }

    private static void HandleCharacter(char c)
    {
        Line.Append(c);
        Console.Write(c);
    }

    private static void HandleUpArrow()
    {
        if (history.Count > 0)
        {
            while (Line.Length > 0)
            {
                HandleBackspace();
            }

            historyIndex = (historyIndex + history.Count - 1) % history.Count;
            var previousHistory = history[historyIndex];
            Write(previousHistory);
        }
    }

    private static void HandleDownArrow()
    {
        if (history.Count > 0)
        {
            while (Line.Length > 0)
            {
                HandleBackspace();
            }

            historyIndex = (historyIndex + 1) % history.Count;
            var nextHistory = history[historyIndex];
            Write(nextHistory);
        }
    }

    private static void Write(string value)
    {
        Line.Append(value);
        Console.Write(value);
    }

    private static string? GetLongestCommonPrefix(int prefixLength, List<string> completions)
    {
        if (completions.Count == 0)
        {
            return null;
        }

        if (completions.Count == 1)
        {
            return completions[0];
        }

        int l = completions.Min(c => c.Length);
        for (int i = prefixLength; i < l; ++i)
        {
            char currentChar = completions[0][i];
            if (completions.Any(c => c[i] != currentChar))
            {
                return completions[0][..i];
            }
        }

        return completions[0][..l];
    }

    private sealed class Context
    {
        public bool WasMultiCompletions { get; set; }
        public bool IsMultiCompletions { get; set; }
    }
}
