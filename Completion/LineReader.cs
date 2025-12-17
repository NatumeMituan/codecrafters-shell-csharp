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
                var commands = CompletionProvider.GetCompletions(currentLine).ToList();
                if (wasMultiCompletions)
                {
                    Console.WriteLine();
                    Console.WriteLine(string.Join("  ", commands.OrderBy(c => c)));
                    Console.Write($"{prompt}{line}");
                }
                else if (commands.Count > 1)
                {
                    isMultiCompletions = true;
                    Console.Write(BellCharacter);
                }
                else if (commands.Count == 1)
                {
                    var completion = $"{commands[0][currentLine.Length..]} ";
                    Write(line, completion);
                }
                else
                {
                    Console.Write(BellCharacter);
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
}
