using codecrafters_git.Commands.Base;
using codecrafters_git.Models;
using codecrafters_git.Models.Objects;

namespace codecrafters_git.Commands;

[Command("commit-tree")]
public class CommitTreeCommand: ICommand
{
    
    public void Execute(string[] args)
    {
        var (tree, parent, message) = Parse(args);
        if (tree == null)
        {
            throw new ArgumentException(@"Tree hash must be provided", nameof(tree));
        }
        
        var shaOne = CommitObject.Create(tree, parent, message);
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
