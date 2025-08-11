using CsvTools.Scraps;

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