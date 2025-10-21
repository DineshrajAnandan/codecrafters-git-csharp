using codecrafters_git.Helpers;

namespace codecrafters_git.Models;

public class BlobObject
{
    public ShaOne ShaOne { get; private set; }
    public string DirName { get; private set; }
    public string FileName { get; private set; }
    public string FilePath => Path.Combine(".git/objects", DirName, FileName);
    public string DirPath => Path.Combine(".git/objects", DirName);

    public BlobObject(ShaOne shaOne)
    {
        ShaOne = shaOne;
        var shaString = shaOne.ToString();
        DirName = shaString[..2];
        FileName = shaString[2..];
    }

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
        var sha1 = ShaHelper.CalculateSha1(objectContent);
        var blobObject = new BlobObject(sha1);
        FileHelper.CreateDirectories(blobObject.DirPath);
        FileHelper.WriteAsZLib(blobObject.DirPath, objectContent);
        return blobObject;
    }
    
}