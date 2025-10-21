using System.IO.Compression;
using System.Text;

namespace codecrafters_git.Helpers;

public class ZlibHelper
{
    public static string DecompressZlib(byte[] compressedData)
    {
        using var compressedStream = new MemoryStream(compressedData);
        using var zlibStream = new ZLibStream(compressedStream, CompressionMode.Decompress);
        using var decompressedStream = new MemoryStream();
        zlibStream.CopyTo(decompressedStream);
        var decompressedBytes = decompressedStream.ToArray();
        return Encoding.UTF8.GetString(decompressedBytes);
    }
    
    public static string DecompressFile(string filePath)
    {
        using var fileStream = new FileStream(filePath, FileMode.Open);
        using var zlibStream = new ZLibStream(fileStream, CompressionMode.Decompress);
        using var memoryStream = new MemoryStream();
        zlibStream.CopyTo(memoryStream);
        var decompressedBytes = memoryStream.ToArray();
        return Encoding.UTF8.GetString(decompressedBytes);
    }
    
    public static void CompressToFile(string filePath, string textToCompress)
    {
        var inputBytes = Encoding.UTF8.GetBytes(textToCompress);
        using var fileStream = new FileStream(filePath, FileMode.Create);
        using var compressionStream = new ZLibStream(fileStream, CompressionMode.Compress) ;
        compressionStream.Write(inputBytes, 0, inputBytes.Length);
    }
}