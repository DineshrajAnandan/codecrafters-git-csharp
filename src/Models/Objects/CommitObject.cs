using System.Text;
using codecrafters_git.Helpers;

namespace codecrafters_git.Models.Objects;

public class CommitObject(ShaOne shaOne): GitObject(shaOne)
{
    public static ShaOne Create(ShaOne tree, ShaOne? parent, string message)
    {
        var sb = new StringBuilder();
        sb.Append($"tree {tree}");

        if (parent != null)
        {
            sb.Append($"\nparent {parent}");
        }
        
        var secondsSinceEpoch = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds();
        sb.Append($"\nauthor John <john@email.com> {secondsSinceEpoch} +0000");
        sb.Append($"\ncommitter Smith <smith@email.com> {secondsSinceEpoch} +0000");
        sb.Append($"\n\n{message}\n");
        
        var body = sb.ToString();
        var content = $"commit {body.Length}\0{body}";
        
        var shaOne = ShaOne.CalculateFromText(content);
        var commitObject = new CommitObject(shaOne);
        FileHelper.CreateDirectories(commitObject.DirPath);
        FileHelper.WriteAsZLib(commitObject.FilePath, content);
        return shaOne;
    }
}