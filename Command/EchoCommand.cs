namespace Codecrafters.Shell.Command;

internal class EchoCommand(CommandIO io) : AbstractCommand(io)
{
    public override void Execute()
    {
        var output = string.Join(' ', io.Args);
        io.Stdout.WriteLine(output);
    }
}
