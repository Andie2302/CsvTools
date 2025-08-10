using System.Globalization;

namespace CsvTools.Values;

public readonly record struct CsvValue < T >
{
    public CsvCreationContext< T > Original { get; init; }
    public InvariantValue< T > Current { get; init; }
    public string? ToString ( IFormatProvider cultureInfo ) => string.Format ( cultureInfo , "{0}" , Current.Value );
    public override string? ToString() => ToString ( CultureInfo.InvariantCulture );
}

//public bool IsModified => !EqualityComparer< InvariantValue< T > >.Default.Equals ( InitialValue , CurrentValue );
//public CsvValue< T > WithNewValue ( T? newValue ) { return this with { CurrentValue = new InvariantValue< T > ( newValue ) }; }
//public CsvValue< T > ResetToOriginal() { return this with { CurrentValue = InitialValue }; }


public static class CsvValueExtensions
{
    //public static CsvValue< T > WithNewValue< T > ( this CsvValue< T > value , T? newValue ) { return value.WithNewValue ( newValue ); }
    //public string? OriginalString => Context.OriginalString;
    //public InvariantValue< T > InitialValue => Context.InitialValue;
}