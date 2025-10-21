using System.Security.Cryptography;
using System.Text;

namespace codecrafters_git.Helpers;

public class ShaHelper
{
    
    public static string CalculateSha1(string text)
    {
        var data = Encoding.UTF8.GetBytes(text);
        using var sha1 = SHA1.Create();
        var hashBytes = sha1.ComputeHash(data);
        var sb = new StringBuilder();
        foreach (var b in hashBytes)
        {
            sb.Append(b.ToString("x2")); // "x2" formats the byte as a two-digit hexadecimal number
        }
        return sb.ToString();
    }
}