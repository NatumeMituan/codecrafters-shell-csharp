using Codecrafters.Shell.Command;
using Codecrafters.Shell.Parser;
using System.IO.Pipelines;

namespace Codecrafters.Shell;

internal static class Shell
{
    public static void Execute(string input)
    {
        HistoryCommand.AddHistory(input);

        var tokens = Tokenizer.Tokenize(input);
        if (tokens.Contains("|"))
        {
            var commandsTokens = PipelineParser.Parse(tokens);
            ExecutePipeline(commandsTokens);
        }
        else
        {
            using var command = CommandParser.Parse(tokens, CommandIO.Default);
            command.Execute();
        }
    }

    public static void ExecutePipeline(List<List<string>> commandsTokens)
    {
        TextReader stdIn = Console.In;
        List<Task> tasks = [];

        for (int i = 0; i < commandsTokens.Count; i++)
        {
            var tokens = commandsTokens[i];
            CommandIO io = new() { Stdin = stdIn };

            if (i < commandsTokens.Count - 1)
            {
                Pipe pipe = new();
                io.Stdout = new StreamWriter(pipe.Writer.AsStream())
                {
                    // Ensure data is written immediately, especially for long-running commands.
                    AutoFlush = true,
                };
                stdIn = new StreamReader(pipe.Reader.AsStream());
            }

            tasks.Add(Task.Run(() =>
            {
                using var cmd = CommandParser.Parse(tokens, io);
                cmd.Execute();
            }));
        }

        Task.WaitAll(tasks);
    }
}
