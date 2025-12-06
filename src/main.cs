class Program
{
    private const string Echo = "echo";
    private const string Exit = "exit";
    private const string Type = "type";

    private static readonly HashSet<string> BuiltInCommands = [Echo, Exit, Type];

    public static void Main()
    {
        while (true)
        {
            Console.Write("$ ");
            var command = Console.ReadLine();
            if (string.IsNullOrEmpty(command))
            {
                continue;
            }

            if (command == Exit)
            {
                break;
            }

            var inputs = command.Split(' ');
            switch (inputs[0])
            {
                case Echo:
                    ExecuateEcho(inputs[1..]);
                    break;
                case Type:
                    ExecuteType(inputs[1..]);
                    break;
                default:
                    Console.WriteLine($"{command}: command not found");
                    break;
            }
        }
    }

    private static void ExecuateEcho(string[] inputs)
    {
        var output = string.Join(' ', inputs);
        Console.WriteLine(output);
    }

    private static void ExecuteType(string[] inputs)
    {
        foreach (var input in inputs)
        {
            if (BuiltInCommands.Contains(input))
            {
                Console.WriteLine($"{input} is a shell builtin");
            }
            else
            {
                Console.WriteLine($"{input}: not found");
            }
        }
    }
}