using codecrafters_git.Enums;
using codecrafters_git.Helpers;

namespace codecrafters_git.Models;

public class TreeObject(ShaOne sha1) : GitObject(sha1)
{
    private const int SHA_BYTE_LENGTH = 20;
    
    public List<TreeObjectEntry> GetContent()
    {
        ValidateFileExists();
        
        var rawTreeData = FileHelper.GetContentFromZLib(FilePath);
        Console.WriteLine("TREE DATA:");
        Console.WriteLine(rawTreeData);
        var parts = SplitTreeData(rawTreeData);
        
        var entries =  ParseTreeEntries(parts);
        return entries.OrderBy(entry => entry.Name).ToList();
    }

    private void ValidateFileExists()
    {
        if (!FileHelper.Exists(FilePath))
            throw new FileNotFoundException("File not found", FilePath);
    }

    private static string[] SplitTreeData(string rawTreeData)
    {
        // Skip the first element (header) and return the rest
        return rawTreeData.Split('\0').Skip(1).ToArray();
    }

    private static List<TreeObjectEntry> ParseTreeEntries(string[] parts)
    {
        var entries = new List<TreeObjectEntry>();

        for (var index = 0; index < parts.Length - 1; index++)
        {
            var entry = ParseSingleEntry(parts, index);
            if (entry != null)
                entries.Add(entry);
        }

        return entries;
    }

    private static TreeObjectEntry? ParseSingleEntry(string[] parts, int index)
    {
        var modeAndName = parts[index];
        var shaBytes = parts[index + 1];

        var tokens = modeAndName.Split(' ');
        if (tokens.Length < 2)
            return null;

        var mode = ParseMode(tokens[0], index);
        var name = tokens[1];
        var sha = ExtractSha(shaBytes);

        return new TreeObjectEntry
        {
            Name = name,
            Mode = mode,
            Sha1 = sha
        };
    }

    private static TreeObjectMode ParseMode(string rawModeToken, int index)
    {
        // Special handling for first entry: strip SHA prefix if present
        var modeToken = index == 0
            ? rawModeToken
            : (rawModeToken.Length > SHA_BYTE_LENGTH 
                ? rawModeToken[SHA_BYTE_LENGTH..] 
                : rawModeToken);

        Enum.TryParse(modeToken, out TreeObjectMode parsedMode);
        return parsedMode;
    }

    private static ShaOne ExtractSha(string blobPart)
    {
        var shaBase64Segment = blobPart.Length >= SHA_BYTE_LENGTH 
            ? blobPart[..SHA_BYTE_LENGTH] 
            : blobPart;
            
        return ShaOne.ParseFromBase64(shaBase64Segment);
    }
}
