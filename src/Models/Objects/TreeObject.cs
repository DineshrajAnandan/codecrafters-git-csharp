using System.Text;
using codecrafters_git.Enums;
using codecrafters_git.Extentions;
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

    public static TreeObject Create(string currentDirectory)
    {
        var entries = CreateEntries(currentDirectory);
        
        using var treeObjectBody = new MemoryStream();
        foreach (var treeObjectEntry in entries)
        {
            var treeObjectEntryRow = Encoding.ASCII.GetBytes($"{(int)treeObjectEntry.Mode} {treeObjectEntry.Name}\0");
            var treeObjectEntrySha = treeObjectEntry.Sha1.AsBytes();
            byte[] rowBytes = [..treeObjectEntryRow, ..treeObjectEntrySha];
            treeObjectBody.Write(rowBytes, 0, rowBytes.Length);
        }
        
        var fileContentAsBytes = 
            Encoding.ASCII.GetBytes($"tree {treeObjectBody.Length}\0")
                .Concat(treeObjectBody.ToArray())
                .ToArray();
        
        var shaOne = ShaOne.CalculateFromBytes(fileContentAsBytes);
        var treeObject = new TreeObject(shaOne);
        FileHelper.CreateDirectories(treeObject.DirPath);
        FileHelper.WriteAsZLib(treeObject.FilePath, fileContentAsBytes);
        return treeObject;
    }
    
    private static List<TreeObjectEntry> CreateEntries(string currentDirectory)
    {
        var resultEntries = new List<TreeObjectEntry>();

        foreach (var entry in FileHelper.GetFileSystemInfos(currentDirectory))
        {
            if (entry.IsDirectory())
            {
                if(entry.IsGitFolder())
                    continue;
                
                var subDirectoryResult = Create(entry.FullName);
                resultEntries.Add(new TreeObjectEntry
                {
                    Mode = TreeObjectMode.Directory,
                    Name = Path.GetFileName(entry.Name),
                    Sha1 = subDirectoryResult.Sha1
                });
            }
            else
            {
                var fileResult = BlobObject.Create(entry.FullName);
                resultEntries.Add(new TreeObjectEntry
                {
                    Mode = TreeObjectMode.File,
                    Name = Path.GetFileName(entry.Name),
                    Sha1 = fileResult.Sha1
                });
            }
        }
        
        return resultEntries;
    }

    private void ValidateFileExists()
    {
        if (!FileHelper.Exists(FilePath))
            throw new FileNotFoundException("File not found", FilePath);
    }
}