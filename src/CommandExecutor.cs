using System.Reflection;
using codecrafters_git.Commands.Base;

namespace codecrafters_git;

public class CommandExecutor
{
    public static void ExecuteCommand(string commandName, string[] args)
    {
        var assembly = Assembly.GetExecutingAssembly();
        
        var commandType = assembly.GetTypes()
            .FirstOrDefault(type => 
            {
                var attribute = type.GetCustomAttribute<CommandAttribute>();
                return attribute != null && attribute.CommandName == commandName;
            });
        
        if (commandType == null)
        {
            Console.WriteLine($"Unknown command {commandName}");
            return;
        }
        
        if (!typeof(ICommand).IsAssignableFrom(commandType))
        {
            Console.WriteLine($"Type '{commandType.Name}' does not implement ICommand.");
            return;
        }
        
        var commandInstance = Activator.CreateInstance(commandType) as ICommand;
        
        commandInstance?.Execute(args);
    }
}