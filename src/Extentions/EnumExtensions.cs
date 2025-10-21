using System.ComponentModel;
using System.Reflection;

namespace codecrafters_git.Extentions;

public static class EnumExtensions
{
    public static string GetDescription(this Enum value)
    {
        var field = value.GetType().GetField(value.ToString());
        var attribute = field?.GetCustomAttribute<DescriptionAttribute>();
        return attribute?.Description ?? value.ToString();
    }
    
    public static T ParseFromDescription<T>(string description) where T : struct, Enum
    {
        var type = typeof(T);
        
        foreach (var field in type.GetFields())
        {
            if (Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) is DescriptionAttribute attribute)
            {
                if (attribute.Description.Equals(description, StringComparison.OrdinalIgnoreCase))
                {
                    return (T)field.GetValue(null);
                }
            }
            else
            {
                // Also check the field name itself
                if (field.Name.Equals(description, StringComparison.OrdinalIgnoreCase))
                {
                    return (T)field.GetValue(null);
                }
            }
        }
        
        throw new ArgumentException($"No enum value found with description: {description}", nameof(description));
    }
    
    public static bool TryParseFromDescription<T>(string description, out T result) where T : struct, Enum
    {
        try
        {
            result = ParseFromDescription<T>(description);
            return true;
        }
        catch
        {
            result = default;
            return false;
        }
    }
}