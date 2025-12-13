namespace Codecrafters.Shell.Parser;

internal record RedirectionInfo(
    string? StdoutFile = null,
    string? StderrFile = null,
    bool AppendStdout = false,
    bool AppendStderr = false
);
