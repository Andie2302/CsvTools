using System.Diagnostics.CodeAnalysis;

namespace CsvTools.Values;

public readonly record struct CsvValue < T >
{
    public CsvOriginal< T > Original { get; init; }
    public Invariant< T > Current { get; init; }
}

public static class CsvValueXxxExtensions
{
    public static CsvValue< T > WithNewValue < T > ( this CsvValue< T > current , T? newValue )
    {
        return current;
    }
}

public static class CsvValueExtensions2
{
    public static bool TryGetValue < T > ( this CsvValue< T > element , [ NotNullWhen ( true ) ] out T? currentValue )
    {
        currentValue = element.Current.Value;
        return currentValue != null;
    }
    public static T? GetValueOrDefault < T > ( this CsvValue< T > element , T? defaultValue ) => element.Current.Value ?? defaultValue;
}

// public string? ToString ( IFormatProvider cultureInfo ) => string.Format ( cultureInfo , "{0}" , Current.Value );
// public override string? ToString() => ToString ( CultureInfo.InvariantCulture );

// public bool IsModified => !EqualityComparer< InvariantValue< T > >.Default.Equals ( InitialValue , CurrentValue );
// 
// public CsvValue< T > ResetToOriginal() { return this with { CurrentValue = InitialValue }; }

public static class CsvValueExtensions
{
    // public static CsvValue< T > WithNewValue< T > ( this CsvValue< T > value , T? newValue ) { return value.WithNewValue ( newValue ); }
    // public string? OriginalString => Context.OriginalString;
    // public InvariantValue< T > InitialValue => Context.InitialValue;
}
