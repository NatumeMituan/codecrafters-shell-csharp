namespace Codecrafters.Shell.Command;

internal class PwdCommand : ICommand
{
    public void Execute()
    {
        var output = Environment.CurrentDirectory;
        Console.WriteLine(output);
    }
}
