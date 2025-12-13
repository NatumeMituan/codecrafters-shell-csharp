namespace Codecrafters.Shell.Parser;

internal static class RedirectionParser
{
    public static RedirectionInfo Parse(IReadOnlyList<string> tokens, out string[] cleanedTokens)
    {
        var cleanedTokenList = new List<string>();
        string? stdoutFile = null;
        string? stderrFile = null;
        bool appendStdout = false;
        bool appendStderr = false;

        int i = 0;
        for (; i < tokens.Count - 1; ++i)
        {
            switch (tokens[i])
            {
                case ">":
                case "1>":
                    stdoutFile = tokens[++i];
                    appendStdout = false;
                    break;
                case ">>":
                case "1>>":
                    stdoutFile = tokens[++i];
                    appendStdout = true;
                    break;
                case "2>":
                    stderrFile = tokens[++i];
                    appendStderr = false;
                    break;
                case "2>>":
                    stderrFile = tokens[++i];
                    appendStderr = true;
                    break;
                default:
                    cleanedTokenList.Add(tokens[i]);
                    break;
            }
        }

        if (i == tokens.Count - 1)
        {
            cleanedTokenList.Add(tokens[i]);
        }

        cleanedTokens = [.. cleanedTokenList];
        return new RedirectionInfo(
            stdoutFile,
            stderrFile,
            appendStdout,
            appendStderr
        );
    }
}