using Codecrafters.Shell.Command;
using Codecrafters.Shell.Completion;
using Codecrafters.Shell.Parser;
using System.IO.Pipelines;

namespace Codecrafters.Shell;

internal static class Shell
{
    private const string Prompt = "$ ";
    private static readonly List<string> history = [];
    private static int loadedHistoryCount = 0;

    public static IReadOnlyList<string> History => history;

    public static void AppendHistory(string command)
    {
        if (!string.IsNullOrEmpty(command))
        {
            history.Add(command);
        }
    }

    public static void SaveHistory()
    {
        var histFile = Environment.GetEnvironmentVariable("HISTFILE");
        if (!string.IsNullOrEmpty(histFile))
        {
            using var writer = new StreamWriter(histFile, true);
            for (int i = loadedHistoryCount; i < history.Count; i++)
            {
                writer.WriteLine(history[i]);
            }
        }
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Blocker Bug", "S2190:Loops and recursions should not be infinite", Justification = "By design")]
    public static void Run()
    {
        LoadHistory();
        while (true)
        {
            var input = LineReader.ReadLine(Prompt, history);
            if (!string.IsNullOrEmpty(input))
            {
                Execute(input);
            }
        }
    }

    private static void LoadHistory()
    {
        var histFile = Environment.GetEnvironmentVariable("HISTFILE");
        if (File.Exists(histFile))
        {
            using var reader = new StreamReader(histFile);
            while (reader.ReadLine() is string line)
            {
                ++loadedHistoryCount;
                AppendHistory(line);
            }
        }
    }

    public static void Execute(string input)
    {
        AppendHistory(input);

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
