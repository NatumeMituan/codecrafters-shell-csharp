namespace Codecrafters.Shell.Command;

internal class CdCommand(string[] args) : ICommand
{
    public void Execute()
    {
        if (args.Length > 0)
        {
            var path = Path.GetFullPath(args[0]);

            if (Directory.Exists(path))
            {
                Environment.CurrentDirectory = path;
            }
            else
            {
                Console.WriteLine($"cd: {path}: No such file or directory");
            }
        }
    }
}