using CsvTools.Values;

namespace UnitTestCsvTools.Tests;

public class CsvOriginalValuesTests
{
    [Fact]
    public void Constructor_SetsProperties()
    {
        var stringValue = "123";
        var invariantValue = new CsvInvariantValue<int>(123);
        var originalValues = new CsvOriginalValues<int>(stringValue, invariantValue);
        Assert.Equal(stringValue, originalValues.String);
        Assert.Equal(invariantValue, originalValues.Value);
    }

    [Fact]
    public void Constructor_WithNullString_SetsStringToNull()
    {
        var invariantValue = new CsvInvariantValue<int>(123);
        var originalValues = new CsvOriginalValues<int>(null, invariantValue);
        Assert.Null(originalValues.String);
        Assert.Equal(invariantValue, originalValues.Value);
    }

    [Fact]
    public void Equals_WithSameValues_ReturnsTrue()
    {
        var invariantValue = new CsvInvariantValue<int>(123);
        var original1 = new CsvOriginalValues<int>("123", invariantValue);
        var original2 = new CsvOriginalValues<int>("123", invariantValue);
        Assert.Equal(original1, original2);
    }

    [Fact]
    public void Equals_WithDifferentValues_ReturnsFalse()
    {
        var invariantValue1 = new CsvInvariantValue<int>(123);
        var invariantValue2 = new CsvInvariantValue<int>(456);
        var original1 = new CsvOriginalValues<int>("123", invariantValue1);
        var original2 = new CsvOriginalValues<int>("456", invariantValue2);
        Assert.NotEqual(original1, original2);
    }
}