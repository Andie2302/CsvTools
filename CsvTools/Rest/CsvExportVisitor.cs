using CsvTools.Values;

namespace CsvTools.Rest;

public class CsvExportVisitor ( IFormatProvider formatProvider , bool useOriginal = false ) : ICsvValueVisitor< string >
{
    public string Visit<T>(CsvValue<T> csvValue)
    {
        var valueToUse = useOriginal ? csvValue.OriginalValue : csvValue.CurrentValue;

        if (valueToUse == null) {
            return string.Empty;
        }

        return valueToUse switch
        {
            DateTime dt => dt.ToString("yyyy-MM-dd", formatProvider),
            decimal dec => dec.ToString("F2", formatProvider),
            double dbl => dbl.ToString("F2", formatProvider),
            IFormattable formattable => formattable.ToString(null, formatProvider),
            _ => valueToUse.ToString() ?? string.Empty
        };
    }
}