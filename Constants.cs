namespace Codecrafters.Shell;

using Codecrafters.Shell.Command;

internal static class Constants
{
    public static readonly Dictionary<string, Func<string, string[], ICommand>> BuiltInCommands = new()
    {
        ["cd"] = (command, args) => new CdCommand(args),
        ["echo"] = (command, args) => new EchoCommand(args),
        ["exit"] = (command, args) => new ExitCommand(),
        ["pwd"] = (command, args) => new PwdCommand(),
        ["type"] = (command, args) => new TypeCommand(args),
    };
}
