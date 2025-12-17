using static Codecrafters.Shell.Constants;

namespace Codecrafters.Shell.Completion;

internal static class CompletionProvider
{
    private static readonly Trie commandTrie = BuildCompletionTrie();

    public static List<string> GetCompletions(string prefix)
    {
        return [.. commandTrie.GetWordsWithPrefix(prefix)];
    }

    private static Trie BuildCompletionTrie()
    {
        var trie = new Trie();
        foreach (var command in Constants.BuiltInCommands.Keys)
        {
            trie.Add(command);
        }

        foreach (var exe in GetExecutablesFromPath())
        {
            trie.Add(exe);
        }

        return trie;
    }

    private static IEnumerable<string> GetExecutablesFromPath()
    {
        var dirs = Environment.GetEnvironmentVariable("PATH")?.Split(Path.PathSeparator, StringSplitOptions.RemoveEmptyEntries) ?? [];
        return from dir in dirs
               where Directory.Exists(dir)
               let files = Directory.GetFiles(dir)
               from file in files
               where IsExecutable(file)
               select Path.GetFileNameWithoutExtension(file);
    }

    private static bool IsExecutable(string fullPath)
    {
        if (OperatingSystem.IsWindows())
        {
            return ExecutableExtensions.Contains(Path.GetExtension(fullPath), StringComparer.OrdinalIgnoreCase);
        }

        return (File.GetUnixFileMode(fullPath) & ExecuteMods) != 0;

    }
}
