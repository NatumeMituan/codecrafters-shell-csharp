class Program
{
    static void Main()
    {
        while (true)
        {
            Console.Write("$ ");

            var command = Console.ReadLine();
            if(string.IsNullOrEmpty(command))
            {
                continue;
            }

            if(command == "exit")
            {
                break;
            }

            var inputs = command.Split(' ');
            if(inputs[0] == "echo")
            {
                var output = string.Join(' ', inputs[1..]);
                Console.WriteLine(output);
            }
            else
            {
                Console.WriteLine($"{command}: command not found");
            }
        }
    }
}