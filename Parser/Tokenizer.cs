using System.Text;

namespace Codecrafters.Shell.Parser;

internal static class Tokenizer
{
    private const char SingleQuote = '\'';
    private const char DoubleQuote = '\"';
    private const char Backslash = '\\';

    public static List<string> Tokenize(string input)
    {
        var result = new List<string>();
        var sb = new StringBuilder();
        int i = 0;

        while (i < input.Length)
        {
            var c = input[i++];
            switch (c)
            {
                case SingleQuote or DoubleQuote:
                    HandleQuote(input, ref i, sb, c);
                    break;
                case char ch when char.IsWhiteSpace(ch):
                    HandleWhiteSpace(input, ref i, sb, result);
                    break;
                default:
                    HandleNormalChar(input, ref i, sb);
                    break;
            }
        }

        if (sb.Length > 0)
        {
            result.Add(sb.ToString());
            sb.Clear();
        }

        return result;
    }

    private static void HandleQuote(string input, ref int i, StringBuilder sb, char quote)
    {
        while (i < input.Length && input[i] != quote)
        {
            if (quote == DoubleQuote
                && input[i] == Backslash
                && i + 1 < input.Length
                && (input[i + 1] == DoubleQuote || input[i + 1] == Backslash))
            {
                i++;
            }

            sb.Append(input[i++]);
        }

        i++;
    }

    private static void HandleWhiteSpace(string input, ref int i, StringBuilder sb, List<string> result)
    {
        if (sb.Length > 0)
        {
            result.Add(sb.ToString());
            sb.Clear();
        }

        while (i < input.Length && char.IsWhiteSpace(input[i]))
        {
            i++;
        }
    }

    private static void HandleNormalChar(string input, ref int i, StringBuilder sb)
    {
        --i;
        do
        {
            if (input[i] == Backslash && i + 1 < input.Length)
            {
                i++;
            }

            sb.Append(input[i++]);
        }
        while (i < input.Length && input[i] != SingleQuote && input[i] != DoubleQuote && !char.IsWhiteSpace(input[i]));
    }
}
