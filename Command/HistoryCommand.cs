namespace Codecrafters.Shell.Command;

internal class HistoryCommand(CommandIO io) : AbstractCommand(io)
{
    private static readonly List<string> history = [];

    public static void AddHistory(string line)
    {
        history.Add(line);
    }

    public override void Execute()
    {
        for (int i = 0; i < history.Count; i++)
        {
            io.Stdout.WriteLine($"{i + 1}  {history[i]}");
        }
    }
}
