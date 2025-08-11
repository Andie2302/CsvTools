using CsvTools.Scraps;

namespace CsvTools.Values.Extensions;

public static class CsvValueExtensionsCollection
{
    public static IEnumerable<TResult> Accept<TResult>(
        this IEnumerable<CsvValue<object>> values,
        ICsvValueVisitor<TResult> visitor)
    {
        return values.Select(v => v.Accept(visitor));
    }

    public static void Accept(
        this IEnumerable<CsvValue<object>> values,
        ICsvValueVisitor visitor)
    {
        foreach (var value in values)
        {
            value.Accept(visitor);
        }
    }
}