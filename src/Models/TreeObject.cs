using codecrafters_git.Enums;
using codecrafters_git.Helpers;

namespace codecrafters_git.Models;

public class TreeObject(ShaOne sha1) : GitObject(sha1)
{
    private const int SHA_BYTE_LENGTH = 20;
    
    public List<TreeObjectEntry> GetContent()
    {
        ValidateFileExists();
        
        var rawBytes = FileHelper.GetBytesFromZLib(FilePath);
        
        return ParseTreeEntries(rawBytes);
    }

    private void ValidateFileExists()
    {
        if (!FileHelper.Exists(FilePath))
            throw new FileNotFoundException("File not found", FilePath);
    }

    private static List<TreeObjectEntry> ParseTreeEntries(byte[] rawBytes)
    {
        var entries = new List<TreeObjectEntry>();
        
        // Skip past "tree <size>\0" header
        var position = FindHeaderEnd(rawBytes);

        while (position < rawBytes.Length)
        {
            var entry = ParseSingleEntry(rawBytes, ref position);
            if (entry != null)
                entries.Add(entry);
        }

        return entries;
    }

    private static int FindHeaderEnd(byte[] rawBytes)
    {
        // Find the first null byte (end of "tree <size>" header)
        for (var i = 0; i < rawBytes.Length; i++)
        {
            if (rawBytes[i] == 0)
                return i + 1;
        }
        return 0;
    }

    private static TreeObjectEntry? ParseSingleEntry(byte[] rawBytes, ref int position)
    {
        if (position >= rawBytes.Length)
            return null;

        // Read mode and name until null byte
        var modeAndNameEnd = FindNextNullByte(rawBytes, position);
        if (modeAndNameEnd == -1 || modeAndNameEnd + SHA_BYTE_LENGTH > rawBytes.Length)
            return null;

        var modeAndNameBytes = rawBytes[position..modeAndNameEnd];
        var modeAndName = System.Text.Encoding.UTF8.GetString(modeAndNameBytes);
        
        var tokens = modeAndName.Split(' ', 2);
        if (tokens.Length < 2)
            return null;

        var mode = ParseMode(tokens[0]);
        var name = tokens[1];

        // Read the 20-byte SHA immediately after the null byte
        var shaStart = modeAndNameEnd + 1;
        var shaBytes = rawBytes[shaStart..(shaStart + SHA_BYTE_LENGTH)];
        var sha = ShaOne.ParseFromBytes(shaBytes);

        // Move position past this entry
        position = shaStart + SHA_BYTE_LENGTH;

        return new TreeObjectEntry
        {
            Name = name,
            Mode = mode,
            Sha1 = sha
        };
    }

    private static int FindNextNullByte(byte[] bytes, int startPosition)
    {
        for (var i = startPosition; i < bytes.Length; i++)
        {
            if (bytes[i] == 0)
                return i;
        }
        return -1;
    }

    private static TreeObjectMode ParseMode(string modeToken)
    {
        Enum.TryParse(modeToken, out TreeObjectMode parsedMode);
        return parsedMode;
    }
}