namespace Codecrafters.Shell.Command;

internal class CdCommand(CommandIO io) : AbstractCommand(io)
{
    public override void Execute()
    {
        if (io.Args.Length > 0)
        {
            var path = io.Args[0];
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
                io.Stderr.WriteLine($"cd: {path}: No such file or directory");
            }
        }
    }
}