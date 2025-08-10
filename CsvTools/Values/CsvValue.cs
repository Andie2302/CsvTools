namespace CsvTools.Values;

public readonly record struct CsvValue < T >
{
    public CsvCreationContext< T > Context { get; init; }
    public T? CurrentValue { get; init; }
    public string? OriginalString => Context.OriginalString;
    public T? InitialValue => Context.InitialValue;
    public bool IsModified => !EqualityComparer< T >.Default.Equals ( InitialValue , CurrentValue );

    private CsvValue ( CsvCreationContext< T > context , T? currentValue )
    {
        Context = context;
        CurrentValue = currentValue;
    }

    public static CsvValue< T > CreateInitial ( string? originalString , T? parsedValue )
    {
        var context = new CsvCreationContext< T > ( originalString , parsedValue );

        return new CsvValue< T > ( context , parsedValue );
    }

    public CsvValue< T > WithNewValue ( T? newValue ) { return new CsvValue< T > ( Context , newValue ); }
    public CsvValue< T > ResetToOriginal() { return new CsvValue< T > ( Context , Context.InitialValue ); }
}