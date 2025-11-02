namespace codecrafters_git.Extentions;

public static class FileSystemExtensions
{
    public static bool IsDirectory(this FileSystemInfo fileSystemInfo)
    {
        return fileSystemInfo is DirectoryInfo;
    }
    
    public static bool IsGitFolder(this FileSystemInfo fileSystemInfo)
    {
        return fileSystemInfo.Name.EndsWith(".git");
    }
}