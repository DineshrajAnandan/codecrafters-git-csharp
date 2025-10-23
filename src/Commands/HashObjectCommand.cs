using codecrafters_git.Models;
using codecrafters_git.Models.Objects;

namespace codecrafters_git.Commands;

public class HashObjectCommand
{
    public void Execute(string[] args)
    {
        var blobObject = BlobObject.CreateFromFile(args[1]);
        Console.WriteLine(blobObject.Sha1);
    }
}