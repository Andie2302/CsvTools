namespace CsvTools.Values.Extensions;

public static class CsvValueExtensionsStatic
{
    public static bool TryGetCurrentValue<T>(this CsvValue<T> element, out T? currentValue)
    {
        currentValue = element.CurrentValue;
        return currentValue != null;
    }

    public static bool TryGetOriginalValue<T>(this CsvValue<T> element, out T? originalValue)
    {
        originalValue = element.OriginalValue;
        return originalValue != null;
    }

    public static bool TryGetOriginalString<T>(this CsvValue<T> element, out string? originalString)
    {
        originalString = element.OriginalString;
        return originalString != null;
    }
}