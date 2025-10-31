using System.Text;
using codecrafters_git.Enums;
using codecrafters_git.Models;
using codecrafters_git.Models.Objects;

namespace codecrafters_git.Commands;

public class WriteTreeCommand
{
    public void Execute(string[] _)
    {
        var result = CreateTreeObject(Directory.GetCurrentDirectory());
        Console.WriteLine(result);
    }

    private ShaOne CreateTreeObject(string currentDirectory)
    {
        var files = Directory.GetFiles(currentDirectory);
        var subDirectories = Directory.GetDirectories(currentDirectory);
        var treeObjectEntries = new List<TreeObjectEntry>();
        
        foreach (var subDirectory in subDirectories)
        {
            var subDirectoryResult = CreateTreeObject(subDirectory);
            treeObjectEntries.Add(new TreeObjectEntry
            {
                Mode = TreeObjectMode.Directory,
                Name = subDirectory,
                Sha1 = subDirectoryResult
            });
        }
        
        foreach (var file in files)
        {
            var fileResult = CreateBlobObject(file);
            treeObjectEntries.Add(new TreeObjectEntry
            {
                Mode = TreeObjectMode.File,
                Name = file,
                Sha1 = fileResult
            });
        }
        
        var data = GetTreeObjectFileData(treeObjectEntries);
        return ShaOne.CalculateFromText(data);
    }

    private ShaOne CreateBlobObject(string file)
    {
        var blobObject = BlobObject.CreateFromFile(file);
        return blobObject.Sha1;
    }

    private string GetTreeObjectFileData(List<TreeObjectEntry>  treeObjectEntries)
    {
        var sb = new StringBuilder();
        foreach (var treeObjectEntry in treeObjectEntries)
        {
            sb.Append(treeObjectEntry.Mode);
            sb.Append(' ');
            sb.Append(treeObjectEntry.Name);
            sb.Append('\0');
            sb.Append(treeObjectEntry.Sha1.AsBase64());
        }
        var data = sb.ToString();
        
        return $"tree {data.Length}\0{data}";
    }
}