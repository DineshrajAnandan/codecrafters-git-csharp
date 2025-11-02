using codecrafters_git.Commands.Base;
using codecrafters_git.Models.Objects;

namespace codecrafters_git.Commands;

[Command("hash-object")]
public class HashObjectCommand: ICommand
{
    public void Execute(string[] args)
    {
        var filePath = args[1];
        var blobObject = BlobObject.Create(filePath);
        Console.WriteLine(blobObject.Sha1);
    }
}