namespace Codecrafters.Shell.Command;

internal class ExitCommand : ICommand
{
    public void Execute()
    {
        Environment.Exit(0);
    }
}
