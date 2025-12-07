namespace Codecrafters.Shell.Command;

internal class CommandIO(string[] args, TextWriter stdout, TextWriter stderr) : IDisposable
{
    private bool disposed;

    public string[] Args { get; } = args;
    public TextWriter Stdout { get; } = stdout;
    public TextWriter Stderr { get; } = stderr;

    public void Dispose()
    {
        if (disposed)
        {
            return;
        }

        disposed = true;

        DisposeWriter(Stdout, Console.Out);
        DisposeWriter(Stderr, Console.Error);
    }

    private static void DisposeWriter(TextWriter writer, TextWriter consoleWriter)
    {
        if (!ReferenceEquals(writer, consoleWriter))
        {
            try
            {
                writer.Dispose();
            }
            catch
            {
            }
        }
    }

}
