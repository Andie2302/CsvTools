namespace CsvTools.Values;

public readonly record struct CsvValue < T >
{
    public CsvOriginal< T > Original { get; init; }
    public Invariant< T > Current { get; init; }
}

public static class CsvValueExtensionsFluentApi
{
    public static CsvValue< T > WithNewValue < T > ( this CsvValue< T > csvValue , T? newValue ) => csvValue with { Current = new Invariant< T > ( newValue ) };
}


public static class CsvValueExtensionsStatic
{
    public static bool TryGetCurrentValue < T > ( this CsvValue< T > element , out T? currentValue )
    {
        currentValue = element.Current.Value;
        return currentValue != null;
    }

    public static bool TryGetOriginalValue < T > ( this CsvValue< T > element , out T? currentValue )
    {
        currentValue = element.Current.Value;
        return currentValue != null;
    }
    
    public static bool TryGetOriginalString < T > ( this CsvValue< T > element , out T? currentValue )
    {
        currentValue = element.Original.Value.Value;
        return currentValue != null;
    }

}


// public string? ToString ( IFormatProvider cultureInfo ) => string.Format ( cultureInfo , "{0}" , Current.Value );
// public override string? ToString() => ToString ( CultureInfo.InvariantCulture );

// public bool IsModified => !EqualityComparer< InvariantValue< T > >.Default.Equals ( InitialValue , CurrentValue );
// 
// public CsvValue< T > ResetToOriginal() { return this with { CurrentValue = InitialValue }; }
