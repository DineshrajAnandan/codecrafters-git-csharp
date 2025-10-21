namespace codecrafters_git.Models;

public class ShaOne
{
    private const int ValueLength = 40;
    private readonly string _value;
    
    public ShaOne(string value)
    {
        if(value.Length != ValueLength)
            throw new ArgumentException($"Sha1: value must be {ValueLength} characters long");
        _value = value;
    }

    public override string ToString()
    {
        return _value;
    }
}