using System.Text;
using codecrafters_git.Enums;
using codecrafters_git.Helpers;
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
        var directoryEntries = new List<TreeObjectEntry>();
        
        foreach (var subDirectory in subDirectories)
        {
            if(subDirectory.EndsWith(".git"))
                continue;
            var subDirectoryResult = CreateTreeObject(subDirectory);
            directoryEntries.Add(new TreeObjectEntry
            {
                Mode = TreeObjectMode.Directory,
                Name = Path.GetFileName(subDirectory),
                Sha1 = subDirectoryResult
            });
            // Console.WriteLine(subDirectory);
        }
        var fileEntries = new List<TreeObjectEntry>();
        foreach (var file in files)
        {
            var fileResult = CreateBlobObject(file);
            fileEntries.Add(new TreeObjectEntry
            {
                Mode = TreeObjectMode.File,
                Name = Path.GetFileName(file),
                Sha1 = fileResult
            });
            // Console.WriteLine(file);
        }
        directoryEntries.Sort((a, b) => String.Compare(a.Name, b.Name, StringComparison.Ordinal));
        fileEntries.Sort((a, b) => string.Compare(a.Name, b.Name, StringComparison.Ordinal));
        return GetTreeObjectFileData([..directoryEntries,..fileEntries]);
    }

    private ShaOne CreateBlobObject(string file)
    {
        var blobObject = BlobObject.CreateFromFile(file);
        return blobObject.Sha1;
    }

    private ShaOne GetTreeObjectFileData(List<TreeObjectEntry>  treeObjectEntries)
    {
        var bytes = new List<byte>();
        foreach (var treeObjectEntry in treeObjectEntries)
        {
            var mode = BitConverter.GetBytes((int)treeObjectEntry.Mode);
            var name = Encoding.UTF8.GetBytes($" {treeObjectEntry.Name}\0");
            var sha = treeObjectEntry.Sha1.AsBytes();
            bytes.AddRange(mode);
            bytes.AddRange(name);
            bytes.AddRange(sha);
        }
        var data = bytes.ToArray();
        
        var fileContent = Encoding.UTF8.GetBytes($"tree {data.Length}\0");
        
        
        
        var shaOne = ShaOne.CalculateFromBytes(data);
        var treeObject = new TreeObject(shaOne);
        FileHelper.CreateDirectories(treeObject.DirPath);
        FileHelper.WriteAsZLib(treeObject.FilePath, fileContent.Concat(data).ToArray());
        return shaOne;
    }
    
    // private ShaOne GetTreeObjectFileData(List<TreeObjectEntry>  treeObjectEntries)
    // {
    //     var sb = new StringBuilder();
    //     foreach (var treeObjectEntry in treeObjectEntries)
    //     {
    //         sb.Append((int)treeObjectEntry.Mode);
    //         sb.Append(' ');
    //         sb.Append(treeObjectEntry.Name);
    //         sb.Append('\0');
    //         sb.Append(treeObjectEntry.Sha1.AsBase64());
    //     }
    //     var data = sb.ToString();
    //     
    //     var fileContent =  $"tree {data.Length}\0{data}";
    //     var shaOne = ShaOne.CalculateFromText(data);
    //     var treeObject = new TreeObject(shaOne);
    //     FileHelper.CreateDirectories(treeObject.DirPath);
    //     FileHelper.WriteAsZLib(treeObject.FilePath, fileContent);
    //     return shaOne;
    // }
}