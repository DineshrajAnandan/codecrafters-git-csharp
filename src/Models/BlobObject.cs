using codecrafters_git.Extentions;
using codecrafters_git.Helpers;

namespace codecrafters_git.Models;

public class BlobObject(ShaOne sha1) : GitObject(sha1)
{
    public string GetContent()
    {
        if (!FileHelper.Exists(FilePath))
            throw new FileNotFoundException("File not found", FilePath);
        var data = FileHelper.GetContentFromZLib(FilePath);
        return data.Split('\0').Last();
    }
    
    public static BlobObject CreateFromFile(string filePath)
    {
        var content = FileHelper.GetContent(filePath);
        var objectContent = $"blob {content.Length}\0{content}";
        var sha1 = ShaOne.CalculateFromText(objectContent);
        var blobObject = new BlobObject(sha1);
        FileHelper.CreateDirectories(blobObject.DirPath);
        FileHelper.WriteAsZLib(blobObject.FilePath, objectContent);
        return blobObject;
    }
    
}