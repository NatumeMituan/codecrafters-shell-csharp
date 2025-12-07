using Codecrafters.Shell.Command;
using System.Text;
using static Codecrafters.Shell.Constants;

namespace Codecrafters.Shell;

internal static class CommandParser
{
    private const char SingleQuote = '\'';
    private const char DoubleQuote = '\"';
    private const char Backslash = '\\';

    public static AbstractCommand? Parse(string input)
    {
        var inputs = Split(input);

        var command = inputs[0];
        var args = inputs[1..];

        GetOutputRedirect(Console.Out, out var stdout, ref args, a => a == ">" || a == "1>");
        GetOutputRedirect(Console.Error, out var stderr, ref args, a => a == "2>");

        var io = new CommandIO(args, stdout, stderr);
        if (!BuiltInCommands.TryGetValue(command, out var commandFactory))
        {
            commandFactory = (command, io) => new ExternalProgramCommand(io, command);
        }

        return commandFactory(command, io);
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
            if (quote == DoubleQuote
                && input[idx] == Backslash
                && idx + 1 < input.Length
                && (input[idx + 1] == DoubleQuote || input[idx + 1] == Backslash))
            {
                idx++;
            }

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
            if (input[idx] == Backslash && idx + 1 < input.Length)
            {
                idx++;
            }

            sb.Append(input[idx++]);
        }
        while (idx < input.Length && input[idx] != SingleQuote && input[idx] != DoubleQuote && !char.IsWhiteSpace(input[idx]));
    }

    private static void GetOutputRedirect(TextWriter defaultWriter, out TextWriter textWriter, ref string[] args, Predicate<string> predicate)
    {
        textWriter = defaultWriter;
        int idx = Array.FindIndex(args, predicate);
        if (idx >= 0 && idx < args.Length - 1)
        {
            textWriter = new StreamWriter(File.OpenWrite(args[idx + 1]));
            args = args[..idx];
        }
    }
}
