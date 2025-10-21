using System.Security.Cryptography;
using System.Text;

namespace codecrafters_git.Models;

public class ShaOne
{
    private const int ValueLength = 40;
    private const int Base64ValueLength = 20;
    private readonly string _value;
    
    public ShaOne(string value)
    {
        if(value.Length != ValueLength)
            throw new ArgumentException($"Sha1: value must be {ValueLength} characters long");
        _value = value;
    }

    public override string ToString()
    {
        return _value;
    }

    public static bool IsSha1(string value)
    {
        return value.Length == ValueLength;
    }
    
    public static ShaOne CalculateFromText(string text)
    {
        var data = Encoding.UTF8.GetBytes(text);
        using var sha1 = SHA1.Create();
        var hashBytes = sha1.ComputeHash(data);
        return Parse(hashBytes);
    }

    public static ShaOne ParseFromBytes(byte[] bytes)
    {
        return Parse(bytes);
    }

    private static ShaOne Parse(byte[] bytes)
    {
        if (bytes.Length != Base64ValueLength)
        {
            throw new ArgumentException("Not a valid Sha1");
        }
            
        var sb = new StringBuilder(ValueLength);
        foreach (var b in bytes)
        {
            sb.Append(b.ToString("x2"));
        }
        var shaString = sb.ToString();
        return new ShaOne(shaString);
    }
}