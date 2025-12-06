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
                CommandParser.Parse(input)?.Execute();
            }
        }
    }
}