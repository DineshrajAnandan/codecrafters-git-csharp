using codecrafters_git.Models;

namespace codecrafters_git.Commands;

public class CatFileCommand
{
    public void Execute(string[] args)
    {
        var sha1 = new ShaOne(args[1]);
        var blobObject = new BlobObject(sha1);
        Console.Write(blobObject.GetContent());
    }
}