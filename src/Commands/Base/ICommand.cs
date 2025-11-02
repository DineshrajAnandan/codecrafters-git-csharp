namespace codecrafters_git.Commands.Base;

public interface ICommand
{
    void Execute(string[] args);
}