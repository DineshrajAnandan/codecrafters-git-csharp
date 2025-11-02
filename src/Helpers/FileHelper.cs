namespace codecrafters_git.Helpers;

public class FileHelper
{
    public static bool Exists(string filePath)
    {
        return File.Exists(filePath);
    }
    
    public static byte[] GetContentAsBytes(string filePath)
    {
        return File.ReadAllBytes(filePath);
    }
    
    public static string GetContent(string filePath)
    {
        return File.ReadAllText(filePath);
    }

    public static void CreateDirectories(params string[] paths)
    {
        foreach (var path in paths)
        {
            Directory.CreateDirectory(path);
        }
    }

    public static void Write(string path, string content)
    {
        File.WriteAllText(path, content);
    }
    
    public static void Write(string path, byte[] content)
    {
        File.WriteAllBytes(path, content);
    }

    public static void WriteAsZLib(string path, string content)
    {
        ZlibHelper.CompressToFile(path, content);
    }
    
    public static void WriteAsZLib(string path, byte[] inputBytes)
    {
        ZlibHelper.CompressToFile(path, inputBytes);
    }
    
    public static string GetContentFromZLib(string path)
    {
        return ZlibHelper.DecompressFile(path);
    }
    
    public static byte[] GetBytesFromZLib(string path)
    {
        return ZlibHelper.DecompressFileAsBytes(path);
    }
}