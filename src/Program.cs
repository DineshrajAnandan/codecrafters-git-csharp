using codecrafters_git.Commands;

if (args.Length < 1)
{
    Console.WriteLine("Please provide a command.");
    return;
}

var command = args[0];
var commandArgs = args[1..];

switch (command)
{
    case "init":
        new InitCommand().Execute(commandArgs);
        break;
    case "cat-file":
        new CatFileCommand().Execute(commandArgs);
        break;
    case "hash-object":
        new HashObjectCommand().Execute(commandArgs);
        break;
    case "ls-tree":
        new LsTreeCommand().Execute(commandArgs);
        break;
    case "write-tree":
        new WriteTreeCommand().Execute(commandArgs);
        break;
    default:
        throw new ArgumentException($"Unknown command {command}");
}