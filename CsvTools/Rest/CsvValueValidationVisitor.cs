using CsvTools.Values;

namespace CsvTools.Rest;

public class CsvValueValidationVisitor ( Func< object? , bool > validator ) : ICsvValueVisitor< bool >
{
    public bool Visit<T>(CsvValue<T> csvValue)
    {
        return validator(csvValue.CurrentValue);
    }
}