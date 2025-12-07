namespace Codecrafters.Shell.Command;

internal abstract class AbstractCommand(CommandIO io) : IDisposable
{
    private bool disposed;
    protected readonly CommandIO io = io;

    public void Dispose()
    {
        if (disposed)
        {
            return;
        }

        disposed = true;
        io.Dispose();
    }

    public abstract void Execute();
}
