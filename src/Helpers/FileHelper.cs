namespace codecrafters_git.Helpers;

public class FileHelper
{
    public static byte[] ReadAllBytes(string filePath)
    {
        return File.ReadAllBytes(filePath);
    }
}