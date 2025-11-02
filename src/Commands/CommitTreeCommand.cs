using System.Text;
using codecrafters_git.Commands.Base;
using codecrafters_git.Helpers;
using codecrafters_git.Models;
using codecrafters_git.Models.Objects;

namespace codecrafters_git.Commands;

[Command("commit-tree")]
public class CommitTreeCommand: ICommand
{
    private const string AuthorName =  "John";
    private const string AuthorEmail = "john@email.com";
    private const string CommitterName =  "Smith";
    private const string CommitterEmail =  "smith@email.com";
    
    public void Execute(string[] args)
    {
        var (tree, parent, message) = Parse(args);
        if (tree == null)
        {
            throw new ArgumentException(@"Tree hash must be provided", nameof(tree));
        }

        var sb = new StringBuilder();
        sb.Append($"tree {tree}");

        if (parent != null)
        {
            sb.Append($"\nparent {parent}");
        }
        var secondsSinceEpoch = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds();
        sb.Append($"\nauthor {AuthorName} <{AuthorEmail}> {secondsSinceEpoch} +0000");
        sb.Append($"\ncommitter {CommitterName} <{CommitterEmail}> {secondsSinceEpoch} +0000");
        sb.Append($"\n\n{message}\n");
        
        var body = sb.ToString();
        var content = $"commit {body.Length}\0{body}";
        
        var shaOne = ShaOne.CalculateFromText(content);
        var commitObject = new CommitObject(shaOne);
        FileHelper.CreateDirectories(commitObject.DirPath);
        FileHelper.WriteAsZLib(commitObject.FilePath, content);
        Console.WriteLine(shaOne);
    }

    private (ShaOne tree, ShaOne? parent, string message) Parse(string[] args)
    {
        (ShaOne tree, ShaOne parent, string message) result = (null, null, null)!;
        for (int i = 0; i < args.Length; i++)
        {
            if (i == 0)
            {
                result.tree = new ShaOne(args[i]);
                continue;
            }

            if (args[i] == "-p")
            {
                result.parent = new ShaOne(args[++i]);
            }

            if (args[i] == "-m")
            {
                result.message = args[++i];
            }
        }
        return result;
    } 
}
