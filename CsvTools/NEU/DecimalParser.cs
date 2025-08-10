using System.Globalization;

namespace CsvTools.NEU;

public class DecimalParser ( NumberStyles numberStyles = NumberStyles.Number | NumberStyles.AllowCurrencySymbol ) : IValueParser< decimal >
{
    public decimal Parse(string? input, CultureInfo culture)
    {
        if (string.IsNullOrEmpty(input)) {
            return 0;
        }

        try {
            return decimal.TryParse(input, numberStyles, culture, out var result) ? result : 0;
        }
        catch (Exception)
        {
            return 0;
        }
    }

    public bool CanParse(string? input) => !string.IsNullOrEmpty(input) && decimal.TryParse(input, numberStyles, CultureInfo.InvariantCulture, out _);
}