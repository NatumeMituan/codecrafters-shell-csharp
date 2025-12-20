using Codecrafters.Shell.Command;
using static Codecrafters.Shell.Constants;

namespace Codecrafters.Shell.Parser;

internal static class CommandParser
{
    public static AbstractCommand Parse(List<string> tokens, CommandIO io)
    {
        var command = tokens[0];

        var redirectionInfo = RedirectionParser.Parse(tokens[1..], out var args);
        io.Args = args;

        if (redirectionInfo.StdoutFile != null)
        {
            io.Stdout = new StreamWriter(redirectionInfo.StdoutFile, redirectionInfo.AppendStdout);
        }

        if (redirectionInfo.StderrFile != null)
        {
            io.Stderr = new StreamWriter(redirectionInfo.StderrFile, redirectionInfo.AppendStderr);
        }

        return Build(command, io);
    }

    private static AbstractCommand Build(string command, CommandIO io)
    {
        if (!BuiltInCommands.TryGetValue(command, out var commandFactory))
        {
            commandFactory = (command, io) => new ExternalProgramCommand(io, command);
        }

        return commandFactory(command, io);
    }
}