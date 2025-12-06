namespace Codecrafters.Shell.Command;

internal class EchoCommand(string[] args) : ICommand
{
    public void Execute()
    {
        var output = string.Join(' ', args);
        Console.WriteLine(output);
    }
}
