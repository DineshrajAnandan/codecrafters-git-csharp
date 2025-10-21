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
    
    public static byte[] Compress(string textToCompress)
    {
        var inputBytes = Encoding.UTF8.GetBytes(textToCompress);
        using var outputStream = new MemoryStream();
        using var compressionStream = new ZLibStream(outputStream, CompressionMode.Compress) ;
        compressionStream.Write(inputBytes, 0, inputBytes.Length);
        return outputStream.ToArray();
    }
}