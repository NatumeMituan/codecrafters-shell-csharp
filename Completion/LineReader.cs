using System.Text;

namespace Codecrafters.Shell.Completion;

internal static class LineReader
{
    public static string ReadLine(string prompt)
    {
        const char BellCharacter = '\x07';
        Console.Write(prompt);
        var line = new StringBuilder();
        bool wasMultiCompletions = false;
        ConsoleKeyInfo keyInfo;

        do
        {
            bool isMultiCompletions = false;
            keyInfo = Console.ReadKey(intercept: true);
            if (keyInfo.Key == ConsoleKey.Backspace && line.Length > 0)
            {
                line.Length--;
                Console.Write("\b \b");
            }
            else if (keyInfo.Key == ConsoleKey.Tab)
            {
                var currentLine = line.ToString();
                var completions = CompletionProvider.GetCompletions(currentLine);
                var longestCommonPrefix = GetLongestCommonPrefix(currentLine.Length, completions);

                if (completions.Count == 0)
                {
                    Console.Write(BellCharacter);
                }
                else if (completions.Count == 1)
                {
                    var completion = $"{completions[0][currentLine.Length..]} ";
                    Write(line, completion);
                }
                else
                {
                    if (wasMultiCompletions)
                    {
                        Console.WriteLine();
                        Console.WriteLine(string.Join("  ", completions.OrderBy(c => c)));
                        Console.Write($"{prompt}{line}");
                    }
                    else if (longestCommonPrefix?.Length > currentLine.Length)
                    {
                        var completion = longestCommonPrefix[currentLine.Length..];
                        Write(line, completion);
                    }
                    else
                    {
                        isMultiCompletions = true;
                        Console.Write(BellCharacter);
                    }
                }
            }
            else if (!char.IsControl(keyInfo.KeyChar))
            {
                line.Append(keyInfo.KeyChar);
                Console.Write(keyInfo.KeyChar);
            }

            wasMultiCompletions = isMultiCompletions;
        } while (keyInfo.Key != ConsoleKey.Enter);

        Console.WriteLine();
        return line.ToString();
    }

    private static void Write(StringBuilder line, string value)
    {
        line.Append(value);
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
}
