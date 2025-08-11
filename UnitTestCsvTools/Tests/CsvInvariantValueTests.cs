using CsvTools.Values;

namespace UnitTestCsvTools.Tests;

public class CsvInvariantValueTests
{
    [Fact]
    public void Constructor_SetsValue()
    {
        var value = "test";
        var invariantValue = new CsvInvariantValue<string>(value);
        Assert.Equal(value, invariantValue.Value);
    }

    [Fact]
    public void Constructor_WithNull_SetsValueToNull()
    {
        var invariantValue = new CsvInvariantValue<string?>(null);
        Assert.Null(invariantValue.Value);
    }

    [Fact]
    public void Equals_WithSameValue_ReturnsTrue()
    {
        var value1 = new CsvInvariantValue<int>(123);
        var value2 = new CsvInvariantValue<int>(123);
        Assert.Equal(value1, value2);
    }

    [Fact]
    public void Equals_WithDifferentValue_ReturnsFalse()
    {
        var value1 = new CsvInvariantValue<int>(123);
        var value2 = new CsvInvariantValue<int>(456);
        Assert.NotEqual(value1, value2);
    }
}