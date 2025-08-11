using CsvTools.Scraps;
using CsvTools.Values;

namespace UnitTestCsvTools.Tests;

public class CsvTestCsvValueVisitor<TResult> : ICsvValueVisitor<TResult>
{
    public TResult Visit<T>(CsvValue<T> value)
    {
        return (TResult)(object)$"Visited: {value.CurrentValue}";
    }
}