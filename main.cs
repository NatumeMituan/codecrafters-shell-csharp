using Codecrafters.Shell.Parser;

namespace Codecrafters.Shell;

public static class Program
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Blocker Bug", "S2190:Loops and recursions should not be infinite", Justification = "By design")]
    public static void Main()
    {
        while (true)
        {
            Console.Write("$ ");
            var input = Console.ReadLine();
            if (!string.IsNullOrEmpty(input))
            {
                using var command = CommandParser.Parse(input);
                command?.Execute();
            }
        }
    }
}