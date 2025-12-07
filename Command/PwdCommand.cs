namespace Codecrafters.Shell.Command;

internal class PwdCommand(CommandIO io) : AbstractCommand(io)
{
    public override void Execute()
    {
        var output = Environment.CurrentDirectory;
        io.Stdout.WriteLine(output);
    }
}
