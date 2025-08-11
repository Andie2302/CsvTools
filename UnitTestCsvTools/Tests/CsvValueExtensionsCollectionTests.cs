using CsvTools.Values;
using CsvTools.Values.Extensions;

namespace UnitTestCsvTools.Tests;

public class CsvValueExtensionsCollectionTests
{
    [Fact]
    public void Accept_WithVisitor_CallsVisitorForEachValue()
    {
        var values = new[]
        {
            new CsvValue<object>("1", 1),
            new CsvValue<object>("2", 2),
            new CsvValue<object>("3", 3)
        };
        var visitor = new TestCsvValueVisitor<string>();
        var results = values.Accept(visitor).ToList();
        Assert.Equal(3, results.Count);
        Assert.Equal("Visited: 1", results[0]);
        Assert.Equal("Visited: 2", results[1]);
        Assert.Equal("Visited: 3", results[2]);
    }

    [Fact]
    public void Accept_WithEmptyCollection_ReturnsEmptyResults()
    {
        var values = Array.Empty<CsvValue<object>>();
        var visitor = new TestCsvValueVisitor<string>();
        var results = values.Accept(visitor).ToList();
        Assert.Empty(results);
    }

    [Fact]
    public void Accept_VoidVisitor_CallsVisitorForEachValue()
    {
        var values = new[]
        {
            new CsvValue<object>("1", 1),
            new CsvValue<object>("2", 2),
            new CsvValue<object>("3", 3)
        };
        var visitor = new TestCsvValueVoidVisitor();
        values.Accept(visitor);
        Assert.Equal(3, visitor.VisitedCount);
    }
}