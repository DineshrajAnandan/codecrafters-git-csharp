using codecrafters_git.Helpers;

namespace codecrafters_git.Commands;

public class HashObjectCommand
{
    public void Execute(string[] args)
    {
        var filePath = args[1];
        var data = FileHelper.GetContent(filePath);
        var objectContent = $"blob {data.Length}\0{data}";
        var sha1 = ShaHelper.CalculateSha1(objectContent);
        var objectDirectory = sha1[..2];
        var objectFileName = sha1[2..];
        var objectFilePath = Path.Combine(".git/objects", objectDirectory, objectFileName);
        FileHelper.CreateDirectories(Path.Combine(".git/objects", objectDirectory));
        
        FileHelper.WriteAsZLib(objectFilePath, objectContent);
        Console.WriteLine(sha1);
    }
}