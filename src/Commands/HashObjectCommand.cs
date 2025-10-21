using codecrafters_git.Helpers;

namespace codecrafters_git.Commands;

public class HashObjectCommand
{
    public void Execute(string[] args)
    {
        var filePath = args[1];
        var data = FileHelper.GetContent(filePath);
        var sha1 = FileHelper.CalculateSha1(filePath);
        var objectDirectory = sha1[..2];
        var objectFileName = sha1[2..];
        var objectFilePath = Path.Combine(".git/objects", objectDirectory, objectFileName);
        var objectContent = $"blob {data.Length}\0{data}";
        FileHelper.CreateDirectories(Path.Combine(".git/objects", objectDirectory));
        
        FileHelper.Write(objectFilePath, ZlibHelper.Compress(objectContent));
    }
}