namespace codecrafters_git.Commands.Base;

[AttributeUsage(AttributeTargets.Class)]
public class CommandAttribute(string commandName) : Attribute
{
    public readonly string CommandName = commandName;
}