using System.Collections;
using System.Reflection;
using System.Text;

namespace Helpers;

/// <summary>
/// Internal helper tools for BL layer.
/// </summary>
internal static class Tools
{
    /// <summary>
    /// Generic extension method that creates a string representation
    /// of an object using Reflection.
    /// Supports nested objects and collections.
    /// </summary>
    public static string ToStringProperty<T>(this T obj)
    {
        if (obj == null)
            return string.Empty;

        StringBuilder sb = new();
        Type type = obj.GetType();

        sb.AppendLine(type.Name + ":");

        foreach (PropertyInfo prop in type.GetProperties())
        {
            object? value = prop.GetValue(obj);
            sb.Append($"  {prop.Name}: ");

            if (value == null)
            {
                sb.AppendLine("null");
            }
            else if (value is string)
            {
                sb.AppendLine(value.ToString());
            }
            else if (value is IEnumerable enumerable)
            {
                sb.AppendLine();
                foreach (var item in enumerable)
                {
                    sb.AppendLine($"    - {item?.ToStringProperty()}");
                }
            }
            else
            {
                sb.AppendLine(value.ToString());
            }
        }

        return sb.ToString();
    }
}
