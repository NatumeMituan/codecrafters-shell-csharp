using System.Diagnostics;

namespace Codecrafters.Shell.Command;

internal class ExternalProgramCommand(CommandIO io, string command) : AbstractCommand(io)
{
    public override void Execute()
    {
        if (command.TryGetFullPathOfExecuatable(out _))
        {
            var process = new Process()
            {
                StartInfo = new ProcessStartInfo(command, io.Args) { RedirectStandardOutput = true }
            };

            process.OutputDataReceived += (sender, e) =>
            {
                if (e.Data is not null)
                {
                    io.Stdout.WriteLine(e.Data);
                }
            };

            process.Start();
            process.BeginOutputReadLine();
            process.WaitForExit();
        }
        else
        {
            io.Stderr.WriteLine($"{command}: command not found");
        }
    }
}
