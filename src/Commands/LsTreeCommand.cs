using System.ComponentModel;
using codecrafters_git.Extentions;
using codecrafters_git.Models;

namespace codecrafters_git.Commands;

public class LsTreeCommand
{
    public void Execute(string[] args)
    {
        var (sha1, option) = ParseArgs(args);
        var treeObject = new TreeObject(sha1);
        var entries = treeObject.GetContent();
        if (option == LsTreeOption.NameOnly)
        {
            foreach (var entry in entries)
            {
                Console.WriteLine(entry.Name);
            }
        }
    }

    private (ShaOne sha1, LsTreeOption option) ParseArgs(string[] args)
    {
        if(args.Length < 1) 
        {
            throw new ArgumentException($"Invalid argument: {nameof(args)}");
        }

        if (ShaOne.IsSha1(args[0]))
        {
            return (new ShaOne(args[0]), default);
        }

        EnumExtensions.TryParseFromDescription<LsTreeOption>(args[0], out var option);
        return (new ShaOne(args[1]), option);
    }
}

enum LsTreeOption
{
    [Description("--name-only")]
   NameOnly, 
}

