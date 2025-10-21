using codecrafters_git.Helpers;

namespace codecrafters_git.Models;

public class BlobObject(string sha1)
{
    public string DirName { get; set; } = sha1[..2];
    public string FileName { get; set; } = sha1[2..];
    public string FilePath => Path.Combine(".git/objects", DirName, FileName);

    public string GetContent()
    {
        var data = FileHelper.GetContentFromZLib(FilePath);
        return data.Split('\0').Last();
    }
    
    
}