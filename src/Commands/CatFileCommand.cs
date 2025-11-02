using codecrafters_git.Commands.Base;
using codecrafters_git.Models;
using codecrafters_git.Models.Objects;

namespace codecrafters_git.Commands;

[Command("cat-file")]
public class CatFileCommand: ICommand
{
    public void Execute(string[] args)
    {
        var filePath = args[1];
        var blobObject = new BlobObject(new ShaOne(filePath));
        Console.Write(blobObject.GetContent());
    }
}