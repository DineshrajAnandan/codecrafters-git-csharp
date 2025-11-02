using codecrafters_git.Models.Objects;

namespace codecrafters_git.Commands;

public class HashObjectCommand
{
    public void Execute(string[] args)
    {
        var filePath = args[1];
        var blobObject = BlobObject.Create(filePath);
        Console.WriteLine(blobObject.Sha1);
    }
}