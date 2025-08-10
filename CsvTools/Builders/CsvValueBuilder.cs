using System.Globalization;
using CsvTools.Values;

namespace CsvTools.Builders;

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
        if ( s is null ) { return default; }

        if ( typeof ( T ) == typeof ( decimal ) ) {
            try {
                var style = NumberStyles.Number | NumberStyles.AllowCurrencySymbol;

                if ( decimal.TryParse ( s , style , c , out var result ) ) { return (T) (object) result; }

                return default;
            }
            catch ( Exception ) { return default; }
        }

        try {
            var converter = System.ComponentModel.TypeDescriptor.GetConverter ( typeof ( T ) );

            if ( converter.CanConvertFrom ( typeof ( string ) ) ) { return (T?) converter.ConvertFromString ( null , c , s ); }

            return (T?) Convert.ChangeType ( s , typeof ( T ) , c );
        }
        catch ( Exception ) { return default; }
    }
}
