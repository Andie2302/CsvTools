using CsvTools.Scraps;
using CsvTools.Values;

namespace UnitTestCsvTools.Tests;

public class TestCsvValueVoidVisitor : ICsvValueVisitor
{
    public int VisitedCount { get; private set; }

    public void Visit<T>(CsvValue<T> value)
    {
        VisitedCount++;
    }
}