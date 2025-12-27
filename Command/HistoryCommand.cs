namespace Codecrafters.Shell.Command;

internal class HistoryCommand(CommandIO io) : AbstractCommand(io)
{
    public override void Execute()
    {
        if (io.Args.Length == 2)
        {
            if (io.Args[0] == "-r" && File.Exists(io.Args[1]))
            {
                this.ReadFromFile();
                return;
            }
            else if (io.Args[0] == "-w")
            {
                this.WriteToFile();
                return;
            }
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

    private void ReadFromFile()
    {
        using var reader = new StreamReader(io.Args[1]);
        while (reader.ReadLine() is string line)
        {
            Shell.AppendHistory(line);
        }
    }

    private void WriteToFile()
    {
        using var writer = new StreamWriter(io.Args[1]);
        foreach (var entry in Shell.History)
        {
            writer.WriteLine(entry);
        }
    }
}
