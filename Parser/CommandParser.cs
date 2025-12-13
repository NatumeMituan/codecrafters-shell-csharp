using Codecrafters.Shell.Command;
using static Codecrafters.Shell.Constants;

namespace Codecrafters.Shell.Parser;

internal static class CommandParser
{
    public static AbstractCommand? Parse(string input)
    {
        var tokens = Tokenizer.Tokenize(input);
        var command = tokens[0];

        var redirectionInfo = RedirectionParser.Parse(tokens[1..], out var args);
        var stdout = redirectionInfo.StdoutFile != null ?
            new StreamWriter(redirectionInfo.StdoutFile, redirectionInfo.AppendStdout) :
            Console.Out;
        var stderr = redirectionInfo.StderrFile != null ?
            new StreamWriter(redirectionInfo.StderrFile, redirectionInfo.AppendStderr) :
            Console.Error;
        var io = new CommandIO(args, stdout, stderr);

        if (!BuiltInCommands.TryGetValue(command, out var commandFactory))
        {
            commandFactory = (command, io) => new ExternalProgramCommand(io, command);
        }

        return commandFactory(command, io);
    }
}