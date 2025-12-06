using System.Diagnostics;

namespace Codecrafters.Shell.Command;

internal class ExternalProgramCommand(string command, string[] args) : ICommand
{
    public void Execute()
    {
        if (command.TryGetFullPathOfExecuatable(out _))
        {
            Process.Start(command, args).WaitForExit();
        }
        else
        {
            Console.WriteLine($"{command}: command not found");
        }
    }
}
