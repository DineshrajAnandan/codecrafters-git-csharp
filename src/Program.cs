using System;
using System.IO;
using codecrafters_git.Helpers;

if (args.Length < 1)
{
    Console.WriteLine("Please provide a command.");
    return;
}

// You can use print statements as follows for debugging, they'll be visible when running tests.
Console.Error.WriteLine("Logs from your program will appear here!");

string command = args[0];

if (command == "init")
{
    // Uncomment this block to pass the first stage
    //
    Directory.CreateDirectory(".git");
    Directory.CreateDirectory(".git/objects");
    Directory.CreateDirectory(".git/refs");
    File.WriteAllText(".git/HEAD", "ref: refs/heads/main\n");
    Console.WriteLine("Initialized git directory");
}

if (command == "cat-file")
{
    var fileSha = args[2];
    var dirName = fileSha[..2];
    var fileName = fileSha[2..];
    var filePath = Path.Combine(".git/objects", dirName, fileName);
    var fileBytes = FileHelper.ReadAllBytes(filePath);
    var content = ZlibHelper.DecompressZlib(fileBytes);
    Console.Write(content.Split('\0').Last());
    
}
else
{
    throw new ArgumentException($"Unknown command {command}");
}