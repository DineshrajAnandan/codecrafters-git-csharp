using codecrafters_git.Helpers;

namespace codecrafters_git.Models;

public class Head
{
    private const string HeadFile = ".git/HEAD";

    public static void Write(string content)
    {
        FileHelper.Write(
            HeadFile, 
            content);
    }
}