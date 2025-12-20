using System.Diagnostics;

namespace Codecrafters.Shell.Command;

internal class ExternalProgramCommand(CommandIO io, string command) : AbstractCommand(io)
{
    public override void Execute()
    {
        if (command.TryFindInPath(out _))
        {
            bool redirectStandardInput = !ReferenceEquals(io.Stdin, Console.In);
            bool redirectStandardOutput = !ReferenceEquals(io.Stdout, Console.Out);
            bool redirectStandardError = !ReferenceEquals(io.Stderr, Console.Error);

            var process = new Process()
            {
                StartInfo = new ProcessStartInfo(command, io.Args)
                {
                    RedirectStandardInput = redirectStandardInput,
                    RedirectStandardOutput = redirectStandardOutput,
                    RedirectStandardError = redirectStandardError,
                }
            };

            if (redirectStandardOutput)
            {
                process.OutputDataReceived += (sender, e) =>
                {
                    if (e.Data is not null)
                    {
                        io.Stdout.WriteLine(e.Data);
                    }
                };
            }

            if (redirectStandardError)
            {
                process.ErrorDataReceived += (sender, e) =>
                {
                    if (e.Data is not null)
                    {
                        io.Stderr.WriteLine(e.Data);
                    }
                };
            }

            process.Start();

            if (redirectStandardOutput)
            {
                process.BeginOutputReadLine();
            }

            if (redirectStandardError)
            {
                process.BeginErrorReadLine();
            }

            if (redirectStandardInput)
            {
                while (io.Stdin.ReadLine() is string line)
                {
                    process.StandardInput.WriteLine(line);
                }

                process.StandardInput.Close();
            }

            process.WaitForExit();
        }
        else
        {
            io.Stderr.WriteLine($"{command}: command not found");
        }
    }
}
