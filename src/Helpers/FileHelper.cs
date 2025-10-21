using System.Security.Cryptography;
using System.Text;

namespace codecrafters_git.Helpers;

public class FileHelper
{
    public static string CalculateSha1(string filePath)
    {
        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException($"File not found: {filePath}");
        }

        using var stream = File.OpenRead(filePath);
        using var sha1 = SHA1.Create();
        var hashBytes = sha1.ComputeHash(stream);
        var sb = new StringBuilder();
        foreach (var b in hashBytes)
        {
            sb.Append(b.ToString("x2")); // "x2" formats the byte as a two-digit hexadecimal number
        }
        return sb.ToString();
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
}