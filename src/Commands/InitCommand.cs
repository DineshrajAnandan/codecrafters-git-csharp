using codecrafters_git.Commands.Base;
using codecrafters_git.Helpers;
using codecrafters_git.Models;

namespace codecrafters_git.Commands;

[Command("init")]
public class InitCommand: ICommand
{
    private static readonly string[] DirectoriesToCreate = [
        ".git",
        ".git/objects",
        ".git/refs"
    ];

    public void Execute(string[] _)
    {
        FileHelper.CreateDirectories(DirectoriesToCreate);
        Head.Write("ref: refs/heads/main\n");
        Console.WriteLine("Initialized git directory");
    }
}