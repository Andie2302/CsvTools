namespace CsvTools.Values.Extensions;

public static class CsvValueExtensionsFluentApi
{
    public static CsvValue<T> WithNewValue<T>(this CsvValue<T> csvValue, T? newValue) => csvValue with { CurrentValue = newValue };
    public static CsvValue<T> ResetToOriginal<T>(this CsvValue<T> csvValue) => csvValue with { CurrentValue = csvValue.OriginalValue };
}