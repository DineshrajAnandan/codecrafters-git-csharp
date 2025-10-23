using codecrafters_git.Helpers;

namespace codecrafters_git.Models.Objects;

public class TreeObject(ShaOne sha1) : GitObject(sha1)
{
    public List<TreeObjectEntry> GetContent()
    {
        ValidateFileExists();
        var rawBytes = FileHelper.GetBytesFromZLib(FilePath);
        return TreeObjectEntryHelper.ParseTreeEntries(rawBytes);
    }

    private void ValidateFileExists()
    {
        if (!FileHelper.Exists(FilePath))
            throw new FileNotFoundException("File not found", FilePath);
    }
}