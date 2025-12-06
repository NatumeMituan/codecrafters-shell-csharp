using Codecrafters.Shell.Command;
using System.Text;
using static Codecrafters.Shell.Constants;

namespace Codecrafters.Shell;

internal static class CommandParser
{
    private const char SingleQuote = '\'';
    private const char DoubleQuote = '\"';
    private const char Backslash = '\\';

    public static ICommand? Parse(string input)
    {
        var inputs = Split(input);

        var command = inputs[0];
        var args = inputs[1..];

        if (!BuiltInCommands.TryGetValue(command, out var commandFactory))
        {
            commandFactory = (command, args) => new ExternalProgramCommand(command, args);
        }

        return commandFactory(command, args);
    }

    private static string[] Split(string input)
    {
        var result = new List<string>();
        var sb = new StringBuilder();
        int idx = 0;

        while (idx < input.Length)
        {
            var c = input[idx++];
            switch (c)
            {
                case SingleQuote or DoubleQuote:
                    HandleQuote(input, ref idx, sb, c);
                    break;
                case char ch when char.IsWhiteSpace(ch):
                    HandleWhiteSpace(input, ref idx, sb, result);
                    break;
                default:
                    HandleNormalChar(input, ref idx, sb);
                    break;
            }
        }

        if (sb.Length > 0)
        {
            result.Add(sb.ToString());
            sb.Clear();
        }

        return [.. result];
    }

    private static void HandleQuote(string input, ref int idx, StringBuilder sb, char quote)
    {
        while (idx < input.Length && input[idx] != quote)
        {
            sb.Append(input[idx++]);
        }

        idx++;
    }

    private static void HandleWhiteSpace(string input, ref int idx, StringBuilder sb, List<string> result)
    {
        if (sb.Length > 0)
        {
            result.Add(sb.ToString());
            sb.Clear();
        }

        while (idx < input.Length && char.IsWhiteSpace(input[idx]))
        {
            idx++;
        }
    }

    private static void HandleNormalChar(string input, ref int idx, StringBuilder sb)
    {
        --idx;
        do
        {
            if (input[idx] != Backslash || ++idx < input.Length)
            {
                sb.Append(input[idx++]);
            }
        }
        while (idx < input.Length && input[idx] != SingleQuote && input[idx] != DoubleQuote && !char.IsWhiteSpace(input[idx]));
    }
}
