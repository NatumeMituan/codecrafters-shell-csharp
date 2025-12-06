using Codecrafters.Shell.Command;
using static Codecrafters.Shell.Constants;

namespace Codecrafters.Shell;

internal static class CommandParser
{
    public static ICommand? Parse(string? input)
    {
        var inputs = input?.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        if ((inputs?.Length > 0) != true)
        {
            return null;
        }

        var command = inputs[0];
        var args = inputs[1..];

        if (!BuiltInCommands.TryGetValue(command, out var commandFactory))
        {
            commandFactory = (command, args) => new ExternalProgramCommand(command, args);
        }

        return commandFactory(command, args);
    }
}
