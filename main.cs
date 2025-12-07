namespace Codecrafters.Shell;

class Program
{
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