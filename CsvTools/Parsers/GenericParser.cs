using System.ComponentModel;
using System.Globalization;

namespace CsvTools.Parsers;

public class GenericParser < T > : IValueParser< T >
{
    public T? Parse ( string? input , CultureInfo culture )
    {
        if ( input is null ) { return default; }

        try {
            var converter = TypeDescriptor.GetConverter ( typeof ( T ) );

            if ( converter.CanConvertFrom ( typeof ( string ) ) ) { return (T?) converter.ConvertFromString ( null , culture , input ); }

            return (T?) Convert.ChangeType ( input , typeof ( T ) , culture );
        }
        catch ( Exception ) { return default; }
    }

    public bool CanParse ( string? input , CultureInfo culture )
    {
        if ( input is null ) { return false; }

        try {
            var converter = TypeDescriptor.GetConverter ( typeof ( T ) );

            if ( converter.CanConvertFrom ( typeof ( string ) ) ) {
                converter.ConvertFromString ( null , culture , input );

                return true;
            }

            _ = Convert.ChangeType ( input , typeof ( T ) , culture );

            return true;
        }
        catch ( Exception ) { return false; }
    }
}
