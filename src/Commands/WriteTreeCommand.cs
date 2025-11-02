using codecrafters_git.Models.Objects;

namespace codecrafters_git.Commands;

public class WriteTreeCommand
{
    public void Execute(string[] _)
    {
        var result = TreeObject.Create(Directory.GetCurrentDirectory());
        Console.WriteLine(result.Sha1);
    }
}