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
        var directoryInfo = new DirectoryInfo(currentDirectory);
        var entries = directoryInfo.GetFileSystemInfos();
        entries = entries.OrderBy(x => x.Name).ToArray();
        var resultEntries = new List<TreeObjectEntry>();

        foreach (var entry in entries)
        {
            if (entry is DirectoryInfo)
            {
                if(entry.Name.EndsWith(".git"))
                    continue;
                var subDirectoryResult = CreateTreeObject(entry.FullName);
                resultEntries.Add(new TreeObjectEntry
                {
                    Mode = TreeObjectMode.Directory,
                    Name = Path.GetFileName(entry.Name),
                    Sha1 = subDirectoryResult
                });
            }
            else
            {
                var fileResult = CreateBlobObject(entry.FullName);
                resultEntries.Add(new TreeObjectEntry
                {
                    Mode = TreeObjectMode.File,
                    Name = Path.GetFileName(entry.Name),
                    Sha1 = fileResult
                });
            }
        }
        
        
        // var files = Directory.GetFiles(currentDirectory);
        // var subDirectories = Directory.GetDirectories(currentDirectory);
        // var directoryEntries = new List<TreeObjectEntry>();
        //
        // foreach (var subDirectory in subDirectories)
        // {
        //     if(subDirectory.EndsWith(".git"))
        //         continue;
        //     var subDirectoryResult = CreateTreeObject(subDirectory);
        //     directoryEntries.Add(new TreeObjectEntry
        //     {
        //         Mode = TreeObjectMode.Directory,
        //         Name = Path.GetFileName(subDirectory),
        //         Sha1 = subDirectoryResult
        //     });
        //     // Console.WriteLine(subDirectory);
        // }
        // var fileEntries = new List<TreeObjectEntry>();
        // foreach (var file in files)
        // {
        //     var fileResult = CreateBlobObject(file);
        //     fileEntries.Add(new TreeObjectEntry
        //     {
        //         Mode = TreeObjectMode.File,
        //         Name = Path.GetFileName(file),
        //         Sha1 = fileResult
        //     });
        //     // Console.WriteLine(file);
        // }
        // directoryEntries.Sort((a, b) => String.Compare(a.Name, b.Name, StringComparison.Ordinal));
        // fileEntries.Sort((a, b) => string.Compare(a.Name, b.Name, StringComparison.Ordinal));
        return GetTreeObjectFileData(resultEntries);
    }

    private ShaOne CreateBlobObject(string file)
    {
        var blobObject = BlobObject.CreateFromFile(file);
        return blobObject.Sha1;
    }

    private ShaOne GetTreeObjectFileData(List<TreeObjectEntry>  treeObjectEntries)
    {
        using var treeObjectBody = new MemoryStream();
        foreach (var treeObjectEntry in treeObjectEntries)
        {
            var treeObjectRow = Encoding.ASCII.GetBytes($"{(int)treeObjectEntry.Mode} {treeObjectEntry.Name}\0");
            var sha = treeObjectEntry.Sha1.AsBytes();
            byte[] rowBytes = [..treeObjectRow, ..sha];
            treeObjectBody.Write(rowBytes, 0, rowBytes.Length);
        }
        
        var fileContent = Encoding.ASCII.GetBytes($"tree {treeObjectBody.Length}\0");
        var data = fileContent.Concat(treeObjectBody.ToArray()).ToArray();
        
        var shaOne = ShaOne.CalculateFromBytes(data);
        var treeObject = new TreeObject(shaOne);
        FileHelper.CreateDirectories(treeObject.DirPath);
        FileHelper.WriteAsZLib(treeObject.FilePath, data);
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