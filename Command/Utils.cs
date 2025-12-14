using static Codecrafters.Shell.Constants;

namespace Codecrafters.Shell.Command;

internal static class Utils
{
    public static bool IsBuiltInCommand(this string command) => BuiltInCommands.ContainsKey(command);

    public static bool TryFindInPath(this string command, out string fullPath)
    {
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
    }

    private static bool IsExecutable(string fullPath)
    {
        const UnixFileMode ExecuteMods = UnixFileMode.UserExecute | UnixFileMode.GroupExecute | UnixFileMode.OtherExecute;
        return OperatingSystem.IsWindows() || (File.GetUnixFileMode(fullPath) & ExecuteMods) != 0;
    }

    private static IEnumerable<string> GetFileNameCandidates(string command)
    {
        yield return command;

        if (!OperatingSystem.IsWindows() ||
            !string.IsNullOrEmpty(Path.GetExtension(command)))
        {
            yield break;
        }

        var extensions = Environment.GetEnvironmentVariable("PATHEXT")?
            .Split(';', StringSplitOptions.RemoveEmptyEntries) ?? [];

        foreach (var ext in extensions)
        {
            yield return command + ext;
        }
    }
}
