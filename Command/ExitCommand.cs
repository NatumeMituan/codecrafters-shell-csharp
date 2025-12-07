namespace Codecrafters.Shell.Command;

internal class ExitCommand(CommandIO io) : AbstractCommand(io)
{
    public override void Execute()
    {
        Environment.Exit(0);
    }
}
