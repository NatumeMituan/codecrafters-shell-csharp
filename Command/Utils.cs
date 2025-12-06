using static Codecrafters.Shell.Constants;

namespace Codecrafters.Shell.Command;

internal static class Utils
{
    public static bool IsBuiltInCommand(this string command) => BuiltInCommands.ContainsKey(command);

    public static bool TryGetFullPathOfExecuatable(this string executable, out string fullPath)
    {
        const UnixFileMode ExecuteMods = UnixFileMode.UserExecute | UnixFileMode.GroupExecute | UnixFileMode.OtherExecute;

        fullPath = string.Empty;
        var paths = Environment.GetEnvironmentVariable("PATH")?.Split(Path.PathSeparator) ?? [];

        foreach (var path in paths)
        {
            fullPath = Path.Combine(path, executable);
            if (File.Exists(fullPath) && (File.GetUnixFileMode(fullPath) & ExecuteMods) != 0)
            {
                return true;
            }
        }

        return false;
    }
}
