using codecrafters_git.Commands.Base;
using codecrafters_git.Models.Objects;

namespace codecrafters_git.Commands;

[Command("write-tree")]
public class WriteTreeCommand: ICommand
{
    public void Execute(string[] _)
    {
        var result = TreeObject.Create(Directory.GetCurrentDirectory());
        Console.WriteLine(result.Sha1);
    }
}