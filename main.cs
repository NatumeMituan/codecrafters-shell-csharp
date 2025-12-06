namespace Codecrafters.Shell;

class Program
{
    public static void Main()
    {
        while (true)
        {
            Console.Write("$ ");
            var input = Console.ReadLine();
            CommandParser.Parse(input)?.Execute();
        }
    }
}