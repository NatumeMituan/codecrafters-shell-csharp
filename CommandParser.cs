using Codecrafters.Shell.Command;
using System.Text;
using static Codecrafters.Shell.Constants;

namespace Codecrafters.Shell;

internal static class CommandParser
{
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
        bool inSingleQuotes = false;
        for (int i = 0; i < input.Length; ++i)
        {
            if (input[i] == '\'')
            {
                if (i + 1 < input.Length && input[i + 1] == '\'')
                {
                    ++i;
                    continue;
                }

                if (inSingleQuotes)
                {
                    inSingleQuotes = false;
                    if (sb.Length > 0)
                    {
                        result.Add(sb.ToString());
                        sb.Clear();
                    }
                }
                else
                {
                    inSingleQuotes = true;
                }
            }
            else if (input[i] == ' ' && !inSingleQuotes)
            {
                if (sb.Length > 0)
                {
                    result.Add(sb.ToString());
                    sb.Clear();
                }
            }
            else
            {
                sb.Append(input[i]);
            }
        }

        if (sb.Length > 0)
        {
            result.Add(sb.ToString());
        }

        return [.. result];
    }
}
