namespace Codecrafters.Shell.Command;

internal class TypeCommand(CommandIO io) : AbstractCommand(io)
{
    public override void Execute()
    {
        foreach (var arg in io.Args)
        {
            if (arg.IsBuiltInCommand())
            {
                io.Stdout.WriteLine($"{arg} is a shell builtin");
            }
            else if (arg.TryGetFullPathOfExecuatable(out var fullPath))
            {
                io.Stdout.WriteLine($"{arg} is {fullPath}");
            }
            else
            {
                io.Stderr.WriteLine($"{arg}: not found");
            }
        }
    }
}
