using codecrafters_git.Helpers;

namespace codecrafters_git.Commands;

public class InitCommand
{
    public void Execute(string[] _)
    {
        FileHelper.CreateDirectories(
            ".git", 
            ".git/objects", 
            ".git/refs");
        FileHelper.Write(
            ".git/HEAD", 
            "ref: refs/heads/main\n");
        Console.WriteLine("Initialized git directory");
    }
}