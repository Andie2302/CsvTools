using System.Globalization;

namespace CsvTools.Parsers;

public class DateTimeParser ( DateTimeStyles dateTimeStyles = DateTimeStyles.None ) : IValueParser< DateTime >
{
    public DateTime Parse ( string? input , CultureInfo culture )
    {
        if ( string.IsNullOrEmpty ( input ) ) { return default; }
        try { return DateTime.TryParse ( input , culture , dateTimeStyles , out var result ) ? result : default; }
        catch ( Exception ) { return default; }
    }

    public bool CanParse ( string? input ) => !string.IsNullOrEmpty ( input ) && DateTime.TryParse ( input , CultureInfo.InvariantCulture , dateTimeStyles , out _ );
}
