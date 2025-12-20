using Codecrafters.Shell.Completion;

namespace Codecrafters.Shell;

public static class Program
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Blocker Bug", "S2190:Loops and recursions should not be infinite", Justification = "By design")]
    public static void Main()
    {
        while (true)
        {
            var input = LineReader.ReadLine("$ ");
            if (!string.IsNullOrEmpty(input))
            {
                Shell.Execute(input);
            }
        }
    }
}