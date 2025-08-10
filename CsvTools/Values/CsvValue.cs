namespace CsvTools.Values;

public interface ICsvValue < T > : ICsvValue
{
    new T? InitialValue { get; }
    new T? CurrentValue { get; }
    CsvValue< T > WithNewValue ( T? newValue );
    CsvValue< T > ResetToOriginal();
    bool HasValue ( T? value );
}

public readonly record struct CsvValue < T > : ICsvValue< T >
{
    public string? OriginalString { get; init; }
    public T? InitialValue { get; init; }
    public T? CurrentValue { get; init; }
    public bool IsModified { get; init; }
    object? ICsvValue.InitialValue => InitialValue;
    object? ICsvValue.CurrentValue => CurrentValue;

    private CsvValue ( string? originalString , T? initialValue , T? currentValue )
    {
        OriginalString = originalString;
        InitialValue = initialValue;
        CurrentValue = currentValue;
        IsModified = !EqualityComparer< T >.Default.Equals ( initialValue , currentValue );
    }

    public static CsvValue< T > CreateInitial ( string? originalString , T? parsedValue ) { return new CsvValue< T > ( originalString , parsedValue , parsedValue ); }
    public CsvValue< T > WithNewValue ( T? newValue ) { return new CsvValue< T > ( OriginalString , InitialValue , newValue ); }
    public CsvValue< T > ResetToOriginal() { return new CsvValue< T > ( OriginalString , InitialValue , InitialValue ); }
    public bool HasValue ( T? value ) { return EqualityComparer< T >.Default.Equals ( CurrentValue , value ); }
    public override string ToString() { return CurrentValue?.ToString() ?? string.Empty; }
}
