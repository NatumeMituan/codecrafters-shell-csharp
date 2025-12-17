using System.Text;

namespace Codecrafters.Shell.Completion;

internal static class LineReader
{
    public static string ReadLine(string prompt)
    {
        Console.Write(prompt);
        var line = new StringBuilder();
        ConsoleKeyInfo keyInfo;

        do
        {
            keyInfo = Console.ReadKey(intercept: true);
            if (keyInfo.Key == ConsoleKey.Backspace && line.Length > 0)
            {
                line.Length--;
                Console.Write("\b \b");
            }
            else if (keyInfo.Key == ConsoleKey.Tab)
            {
                var currentLine = line.ToString();
                var command = CompletionProvider.GetCompletions(currentLine).FirstOrDefault();
                if (command != null)
                {
                    var completion = $"{command[currentLine.Length..]} ";
                    Write(line, completion);
                }
                else
                {
                    Console.Write('\x07'); // Bell character
                }
            }
            else if (!char.IsControl(keyInfo.KeyChar))
            {
                line.Append(keyInfo.KeyChar);
                Console.Write(keyInfo.KeyChar);
            }
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
