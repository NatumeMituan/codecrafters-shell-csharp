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
        int cnt = history.Count;
        if (io.Args.Length > 0 && int.TryParse(io.Args[0], out int n) && n > 0)
        {
            cnt = Math.Min(n, cnt);
        }

        for (int i = history.Count - cnt; i < history.Count; i++)
        {
            io.Stdout.WriteLine($"{i + 1}  {history[i]}");
        }
    }
}
