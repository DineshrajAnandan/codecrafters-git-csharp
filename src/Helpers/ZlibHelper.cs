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
}