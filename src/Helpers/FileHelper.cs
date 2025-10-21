namespace codecrafters_git.Helpers;

public class FileHelper
{
    public static byte[] ReadAllBytes(string filePath)
    {
        return File.ReadAllBytes(filePath);
    }

    public static void CreateDirectories(params string[] paths)
    {
        foreach (var path in paths)
        {
            Directory.CreateDirectory(path);
        }
    }

    public static void WriteAllText(string path, string content)
    {
        File.WriteAllText(path, content);
    }
}