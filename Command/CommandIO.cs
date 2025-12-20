namespace Codecrafters.Shell.Command;

internal sealed class CommandIO : IDisposable
{
    private bool disposed;
    private TextReader stdin = Console.In;
    private TextWriter stdout = Console.Out;
    private TextWriter stderr = Console.Error;

    public string[] Args { get; set; } = [];

    public static CommandIO Default => new();

    public TextReader Stdin
    {
        get => this.stdin;
        set
        {
            var current = this.stdin;
            DisposeIDisposable(current, Console.In);
            this.stdin = value;
        }
    }

    public TextWriter Stdout
    {
        get => this.stdout;
        set
        {
            var current = this.stdout;
            DisposeIDisposable(current, Console.Out);
            this.stdout = value;
        }
    }

    public TextWriter Stderr
    {
        get => this.stderr;
        set
        {
            var current = this.stderr;
            DisposeIDisposable(current, Console.Error);
            this.stderr = value;
        }
    }

    public void Dispose()
    {
        if (disposed)
        {
            return;
        }

        disposed = true;

        DisposeIDisposable(Stdin, Console.Error);
        DisposeIDisposable(Stdout, Console.Out);
        DisposeIDisposable(Stderr, Console.Error);
    }

    private static void DisposeIDisposable(
        IDisposable? objectToDispose,
        IDisposable objectToCompare)
    {
        if (objectToDispose != null && !ReferenceEquals(objectToDispose, objectToCompare))
        {
            objectToDispose.Dispose();
        }
    }
}
