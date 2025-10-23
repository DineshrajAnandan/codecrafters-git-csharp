namespace codecrafters_git.Models.Objects;

public class GitObject
{
    public ShaOne Sha1 { get; protected set; }
    public string DirName { get; protected set; }
    public string FileName { get; protected set; }
    public string FilePath => Path.Combine(".git/objects", DirName, FileName);
    public string DirPath => Path.Combine(".git/objects", DirName);
    
    public GitObject(ShaOne sha1)
    {
        Sha1 = sha1;
        var shaString = sha1.ToString();
        DirName = shaString[..2];
        FileName = shaString[2..];
    }
    
}