using System.Globalization;

namespace CsvTools.Values;

public readonly record struct CsvValue<T>
{
    public string? OriginalString { get; init; }
    public T? OriginalValue { get; init; }
    public T? CurrentValue { get; init; }
    public CsvValue(string? originalString, T? originalValue)
    {
        OriginalString = originalString;
        OriginalValue = originalValue;
        CurrentValue = originalValue;
    }
    public bool IsModified => !EqualityComparer<T?>.Default.Equals(OriginalValue, CurrentValue);
    public override string ToString() => CurrentValue?.ToString() ?? string.Empty;

    public string ToString(IFormatProvider? formatProvider)
        => CurrentValue is IFormattable formattable
            ? formattable.ToString(null, formatProvider)
            : ToString();

    public string ToString(string? format, IFormatProvider? formatProvider)
        => CurrentValue is IFormattable formattable
            ? formattable.ToString(format, formatProvider)
            : ToString();
    public TResult Accept<TResult>(ICsvValueVisitor<TResult> visitor) => visitor.Visit(this);
    public void Accept(ICsvValueVisitor visitor) => visitor.Visit(this);
}
public interface ICsvValueVisitor<out TResult>
{
    TResult Visit<T>(CsvValue<T> csvValue);
}

public interface ICsvValueVisitor
{
    void Visit<T>(CsvValue<T> csvValue);
}
public class CsvValueStringVisitor ( IFormatProvider formatProvider , string? format = null ) : ICsvValueVisitor< string >
{
    public string Visit<T>(CsvValue<T> csvValue) => csvValue.CurrentValue == null ? string.Empty : csvValue.CurrentValue switch
        {
            IFormattable formattable => formattable.ToString ( format , formatProvider ) ,
            _ => csvValue.CurrentValue.ToString() ?? string.Empty
        };
}
public class CsvValueValidationVisitor ( Func< object? , bool > validator ) : ICsvValueVisitor< bool >
{
    public bool Visit<T>(CsvValue<T> csvValue)
    {
        return validator(csvValue.CurrentValue);
    }
}
public static class CsvValueExtensionsFluentApi
{
    public static CsvValue<T> WithNewValue<T>(this CsvValue<T> csvValue, T? newValue) => csvValue with { CurrentValue = newValue };
    public static CsvValue<T> ResetToOriginal<T>(this CsvValue<T> csvValue) => csvValue with { CurrentValue = csvValue.OriginalValue };
}

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
public class CsvExportVisitor ( IFormatProvider formatProvider , bool useOriginal = false ) : ICsvValueVisitor< string >
{
    public string Visit<T>(CsvValue<T> csvValue)
    {
        var valueToUse = useOriginal ? csvValue.OriginalValue : csvValue.CurrentValue;

        if (valueToUse == null) {
            return string.Empty;
        }

        return valueToUse switch
        {
            DateTime dt => dt.ToString("yyyy-MM-dd", formatProvider),
            decimal dec => dec.ToString("F2", formatProvider),
            double dbl => dbl.ToString("F2", formatProvider),
            IFormattable formattable => formattable.ToString(null, formatProvider),
            _ => valueToUse.ToString() ?? string.Empty
        };
    }
}
public class CsvStatisticsVisitor : ICsvValueVisitor
{
    public int TotalValues { get; private set; }
    public int ModifiedValues { get; private set; }
    public int NullValues { get; private set; }

    public void Visit<T>(CsvValue<T> csvValue)
    {
        TotalValues++;

        if (csvValue.IsModified) {
            ModifiedValues++;
        }

        if (csvValue.CurrentValue == null) {
            NullValues++;
        }
    }

    public void Reset() => TotalValues = ModifiedValues = NullValues = 0;
}
public static class VisitorExamples
{
    public static void Example()
    {
        var germanCulture = new CultureInfo("de-DE");
        var usCulture = new CultureInfo("en-US");
        var decimalValue = new CsvValue<decimal>("1.234,56", 1234.56m).WithNewValue(9999.99m);
        var dateValue = new CsvValue<DateTime>("2023-12-31", new DateTime(2023, 12, 31));
        var stringValue = new CsvValue<string>("Hello", "Hello").WithNewValue("World");
        var germanStringVisitor = new CsvValueStringVisitor(germanCulture, "F2");

        Console.WriteLine("Deutsche Formatierung:");
        Console.WriteLine($"Decimal: {decimalValue.Accept(germanStringVisitor)}");
        Console.WriteLine($"Date: {dateValue.Accept(germanStringVisitor)}");
        Console.WriteLine($"String: {stringValue.Accept(germanStringVisitor)}");
        var exportVisitor = new CsvExportVisitor(usCulture);

        Console.WriteLine("\nUS Export-Format:");
        Console.WriteLine($"Decimal: {decimalValue.Accept(exportVisitor)}");
        Console.WriteLine($"Date: {dateValue.Accept(exportVisitor)}");
        Console.WriteLine($"String: {stringValue.Accept(exportVisitor)}");
        var statsVisitor = new CsvStatisticsVisitor();
        decimalValue.Accept(statsVisitor);
        dateValue.Accept(statsVisitor);
        stringValue.Accept(statsVisitor);

        Console.WriteLine($"\nStatistiken:");
        Console.WriteLine($"Total: {statsVisitor.TotalValues}");
        Console.WriteLine($"Modified: {statsVisitor.ModifiedValues}");
        Console.WriteLine($"Null: {statsVisitor.NullValues}");
    }
}
public static class CsvValueCollectionExtensions
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