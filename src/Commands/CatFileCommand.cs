using codecrafters_git.Helpers;

namespace codecrafters_git.Commands;

public class CatFileCommand
{
    public void Execute(string[] args)
    {
        var fileSha = args[1];
        var dirName = fileSha[..1];
        var fileName = fileSha[1..];
        var filePath = Path.Combine(".git/objects", dirName, fileName);
        var fileBytes = FileHelper.ReadAllBytes(filePath);
        var content = ZlibHelper.DecompressZlib(fileBytes);
        Console.Write(content.Split('\0').Last());
    }
}