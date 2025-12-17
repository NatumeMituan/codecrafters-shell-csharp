
namespace Codecrafters.Shell.Completion;

internal class Trie
{
    private sealed class TrieNode
    {
        public Dictionary<char, TrieNode> Children { get; } = [];
        public string? Word { get; set; }
    }

    private readonly TrieNode root = new();

    public void Add(string word)
    {
        var node = root;
        foreach (var ch in word)
        {
            if (!node.Children.TryGetValue(ch, out var value))
            {
                value = new TrieNode();
                node.Children[ch] = value;
            }

            node = value;
        }

        node.Word = word;
    }

    public IEnumerable<string> GetWordsWithPrefix(string prefix)
    {
        var node = root;
        foreach (var ch in prefix)
        {
            if (!node.Children.TryGetValue(ch, out var value))
            {
                return [];
            }

            node = value;
        }

        return GetWordsFromNode(node);
    }

    private IEnumerable<string> GetWordsFromNode(TrieNode node)
    {
        if (node.Word != null)
        {
            yield return node.Word;
        }

        foreach (var child in node.Children.Values)
        {
            foreach (var word in GetWordsFromNode(child))
            {
                yield return word;
            }
        }
    }
}
