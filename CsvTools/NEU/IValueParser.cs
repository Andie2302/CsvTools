using System.Globalization;

namespace CsvTools.NEU;

public interface IValueParser< out T>
{
    T? Parse(string? input, CultureInfo culture);
    bool CanParse(string? input);
}