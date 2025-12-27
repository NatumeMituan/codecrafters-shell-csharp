namespace Codecrafters.Shell.Command;

internal class HistoryCommand(CommandIO io) : AbstractCommand(io)
{
    public override void Execute()
    {
        if (io.Args.Length == 2 && io.Args[0] == "-r" && File.Exists(io.Args[1]))
        {
            using var fs = File.OpenRead(io.Args[1]);
            using var reader = new StreamReader(fs);
            while (reader.ReadLine() is string line)
            {
                Shell.AppendHistory(line);
            }

            return;
        }

        var history = Shell.History;
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
