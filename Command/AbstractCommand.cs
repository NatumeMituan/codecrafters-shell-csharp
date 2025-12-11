namespace Codecrafters.Shell.Command;

internal abstract class AbstractCommand(CommandIO io) : IDisposable
{
    private bool disposed;
    protected readonly CommandIO io = io;

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
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
