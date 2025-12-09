namespace BL;

internal static class Tools
{
    public static string ToStringProperty<T>(this T t)
    {
        return typeof(T).GetProperties()
                        .Select(p => $"{p.Name}: {p.GetValue(t)}")
                        .Aggregate("", (current, next) => current + next + "\n");
    }
}
