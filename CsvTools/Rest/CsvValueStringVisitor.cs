using CsvTools.Values;

namespace CsvTools.Rest;

public class CsvValueStringVisitor ( IFormatProvider formatProvider , string? format = null ) : ICsvValueVisitor< string >
{
    public string Visit<T>(CsvValue<T> csvValue) => csvValue.CurrentValue == null ? string.Empty : csvValue.CurrentValue switch
    {
        IFormattable formattable => formattable.ToString ( format , formatProvider ) ,
        _ => csvValue.CurrentValue.ToString() ?? string.Empty
    };
}