namespace Codecrafters.Shell.Command;

internal class CdCommand(string[] args) : ICommand
{
    public void Execute()
    {
        if (args.Length > 0)
        {
            var path = args[0];
            if (path == "~")
            {
                Environment.CurrentDirectory = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
                return;
            }

            path = Path.GetFullPath(path);
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