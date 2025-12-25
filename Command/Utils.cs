using static Codecrafters.Shell.Constants;

namespace Codecrafters.Shell.Command;

internal static class Utils
{
    public static bool IsBuiltInCommand(this string command) => BuiltInCommands.ContainsKey(command);

    public static bool TryFindInPath(this string command, out string fullPath)
    {
        if (command.StartsWith(@"./"))
        {
            fullPath = command;
            return true;
        }

        fullPath = string.Empty;
        var dirs = Environment.GetEnvironmentVariable("PATH")?.Split(Path.PathSeparator) ?? [];

        foreach (var dir in dirs)
        {
            foreach (var candidate in GetFileNameCandidates(command))
            {
                fullPath = Path.Combine(dir, candidate);
                if (File.Exists(fullPath) && IsExecutable(fullPath))
                {
                    return true;
                }
            }
        }

        return false;

        static bool IsExecutable(string fullPath) =>
            OperatingSystem.IsWindows() ||
            (File.GetUnixFileMode(fullPath) & ExecuteMods) != 0;
    }

    private static IEnumerable<string> GetFileNameCandidates(string command)
    {
        yield return command;

        if (!OperatingSystem.IsWindows() ||
            !string.IsNullOrEmpty(Path.GetExtension(command)))
        {
            yield break;
        }

        foreach (var ext in ExecutableExtensions)
        {
            yield return $"{command}{ext.ToLowerInvariant()}";
        }
    }
}
