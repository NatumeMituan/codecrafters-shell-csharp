namespace Codecrafters.Shell.Command;

internal class TypeCommand(string[] args) : ICommand
{
    public void Execute()
    {
        foreach (var arg in args)
        {
            if (arg.IsBuiltInCommand())
            {
                Console.WriteLine($"{arg} is a shell builtin");
            }
            else if (arg.TryGetFullPathOfExecuatable(out var fullPath))
            {
                Console.WriteLine($"{arg} is {fullPath}");
            }
            else
            {
                Console.WriteLine($"{arg}: not found");
            }
        }
    }
}
