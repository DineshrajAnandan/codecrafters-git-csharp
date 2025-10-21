namespace codecrafters_git.Enums;

public enum TreeObjectMode
{
    File = 100644,
    Directory = 40000,
    Executable = 100755,
    SymbolicLink = 120000,
}