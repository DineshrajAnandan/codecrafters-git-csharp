using codecrafters_git;

if (args.Length < 1)
{
    Console.WriteLine("Please provide a command.");
    return;
}

var command = args[0];
var commandArgs = args[1..];

CommandExecutor.ExecuteCommand(command, commandArgs);