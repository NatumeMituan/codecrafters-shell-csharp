namespace Codecrafters.Shell.Parser;

internal static class PipelineParser
{
    public static List<List<string>> Parse(List<string> tokens)
    {
        List<List<string>> res = [];
        List<string> current = [];

        foreach (var token in tokens)
        {
            if (token == "|")
            {
                if (current.Count > 0)
                {
                    res.Add(current);
                    current = [];
                }
            }
            else
            {
                current.Add(token);
            }
        }

        if (current.Count > 0)
        {
            res.Add(current);
        }

        return res;
    }
}