using System.ComponentModel;

namespace codecrafters_git.Enums;

public enum TreeObjectMode
{
    [Description("blob")]
    File = 100644,
    [Description("tree")]
    Directory = 40000,
    Executable = 100755,
    SymbolicLink = 120000,
}