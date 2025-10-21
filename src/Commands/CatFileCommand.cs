using codecrafters_git.Models;

namespace codecrafters_git.Commands;

public class CatFileCommand
{
    public void Execute(string[] args)
    {
        var blobObject = new BlobObject(args[1]);
        Console.Write(blobObject.GetContent());
    }
}