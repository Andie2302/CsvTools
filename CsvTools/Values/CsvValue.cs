using System.Globalization;

namespace CsvTools.Values;

public readonly record struct InvariantValue < T > ( T? Value );
public readonly record struct CsvCreationContext < T > ( string? OriginalString , InvariantValue< T > InitialValue );

public readonly record struct CsvValue < T >
{
    public CsvCreationContext< T > Context { get; init; }
    public InvariantValue< T > CurrentValue { get; init; }
    public string? OriginalString => Context.OriginalString;
    public InvariantValue< T > InitialValue => Context.InitialValue;
    public bool IsModified => !EqualityComparer< InvariantValue< T > >.Default.Equals ( InitialValue , CurrentValue );
    public CsvValue< T > WithNewValue ( T? newValue ) { return this with { CurrentValue = new InvariantValue< T > ( newValue ) }; }
    public CsvValue< T > ResetToOriginal() { return this with { CurrentValue = InitialValue }; }
    public string? ToString ( IFormatProvider cultureInfo ) { return string.Format ( cultureInfo , "{0}" , CurrentValue.Value ); }
    public override string? ToString() { return ToString ( CultureInfo.InvariantCulture ); }
}

public class CsvValueBuilder < T > ( string? originalString )
{
    private CultureInfo _culture = CultureInfo.InvariantCulture;

    public CsvValueBuilder< T > WithCulture ( CultureInfo culture )
    {
        _culture = culture;

        return this;
    }

    public CsvValue< T > Build()
    {
        var parsedValue = ParseString ( originalString , _culture );
        var invariantInitial = new InvariantValue< T > ( parsedValue );
        var context = new CsvCreationContext< T > ( originalString , invariantInitial );

        return new CsvValue< T > { Context = context , CurrentValue = invariantInitial };
    }

    private static T? ParseString ( string? s , CultureInfo c )
    {
        if ( s is null ) {
            return default;
        }

        try {
            var converter = System.ComponentModel.TypeDescriptor.GetConverter ( typeof ( T ) );

            if ( converter.CanConvertFrom ( typeof ( string ) ) ) { return (T?) converter.ConvertFromString ( null , c , s ); }

            return (T?) Convert.ChangeType ( s , typeof ( T ) , c );
        }
        catch ( Exception ) { return default; }
    }
}
