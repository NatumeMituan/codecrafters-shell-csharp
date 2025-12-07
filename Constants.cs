namespace Codecrafters.Shell;

using Codecrafters.Shell.Command;

internal static class Constants
{
    public static readonly Dictionary<string, Func<string, CommandIO, AbstractCommand>> BuiltInCommands = new()
    {
        ["cd"] = (command, io) => new CdCommand(io),
        ["echo"] = (command, io) => new EchoCommand(io),
        ["exit"] = (command, io) => new ExitCommand(io),
        ["pwd"] = (command, io) => new PwdCommand(io),
        ["type"] = (command, io) => new TypeCommand(io),
    };
}
