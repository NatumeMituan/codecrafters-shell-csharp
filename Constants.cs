namespace Codecrafters.Shell;

using Codecrafters.Shell.Command;

internal static class Constants
{
    public const UnixFileMode ExecuteMods =
        UnixFileMode.UserExecute |
        UnixFileMode.GroupExecute |
        UnixFileMode.OtherExecute;

    public static readonly string[] ExecutableExtensions = OperatingSystem.IsWindows()
        ? (Environment.GetEnvironmentVariable("PATHEXT")?
            .Split(';', StringSplitOptions.RemoveEmptyEntries) ?? [])
        : [];

    public static readonly Dictionary<string, Func<string, CommandIO, AbstractCommand>> BuiltInCommands = new()
    {
        ["cd"] = (command, io) => new CdCommand(io),
        ["echo"] = (command, io) => new EchoCommand(io),
        ["exit"] = (command, io) => new ExitCommand(io),
        ["pwd"] = (command, io) => new PwdCommand(io),
        ["type"] = (command, io) => new TypeCommand(io),
    };
}
