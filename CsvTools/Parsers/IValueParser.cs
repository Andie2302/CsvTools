using System.Globalization;

namespace CsvTools.Parsers;

public interface IValueParser < out T >
{
    T? Parse ( string? input , CultureInfo culture );
    bool CanParse ( string? input , CultureInfo culture );
}
