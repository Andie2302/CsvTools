using CsvTools.Values;
using CsvTools.Values.Extensions;

namespace UnitTestCsvTools.Tests;

public class CsvValueExtensionsStaticTests
{
    [Fact]
    public void TryGetCurrentValue_WithValue_ReturnsTrue()
    {
        var csvValue = new CsvValue<int>("123", 123);
        var result = csvValue.TryGetCurrentValue(out var currentValue);
        Assert.True(result);
        Assert.Equal(123, currentValue);
    }

    [Fact]
    public void TryGetCurrentValue_WithNull_ReturnsFalse()
    {
        var csvValue = new CsvValue<string?>(null, null);
        var result = csvValue.TryGetCurrentValue(out var currentValue);
        Assert.False(result);
        Assert.Null(currentValue);
    }

    [Fact]
    public void TryGetCurrentValue_WithModifiedValue_ReturnsModifiedValue()
    {
        var csvValue = new CsvValue<int>("123", 123);
        var modifiedValue = csvValue.WithNewValue(456);
        var result = modifiedValue.TryGetCurrentValue(out var currentValue);
        Assert.True(result);
        Assert.Equal(456, currentValue);
    }

    [Fact]
    public void TryGetOriginalValue_WithValue_ReturnsTrue()
    {
        var csvValue = new CsvValue<int>("123", 123);
        var result = csvValue.TryGetOriginalValue(out var originalValue);
        Assert.True(result);
        Assert.Equal(123, originalValue);
    }

    [Fact]
    public void TryGetOriginalValue_WithNull_ReturnsFalse()
    {
        var csvValue = new CsvValue<string?>(null, null);
        var result = csvValue.TryGetOriginalValue(out var originalValue);
        Assert.False(result);
        Assert.Null(originalValue);
    }

    [Fact]
    public void TryGetOriginalValue_WithModifiedValue_ReturnsOriginalValue()
    {
        var csvValue = new CsvValue<int>("123", 123);
        var modifiedValue = csvValue.WithNewValue(456);
        var result = modifiedValue.TryGetOriginalValue(out var originalValue);
        Assert.True(result);
        Assert.Equal(123, originalValue);
    }

    [Fact]
    public void TryGetOriginalString_WithString_ReturnsTrue()
    {
        var csvValue = new CsvValue<int>("123", 123);
        var result = csvValue.TryGetOriginalString(out var originalString);
        Assert.True(result);
        Assert.Equal("123", originalString);
    }

    [Fact]
    public void TryGetOriginalString_WithNull_ReturnsFalse()
    {
        var csvValue = new CsvValue<int>();
        var result = csvValue.TryGetOriginalString(out var originalString);
        Assert.False(result);
        Assert.Null(originalString);
    }

    [Fact]
    public void TryGetOriginalString_WithEmptyString_ReturnsTrue()
    {
        var csvValue = new CsvValue<int>("", 0);
        var result = csvValue.TryGetOriginalString(out var originalString);
        Assert.True(result);
        Assert.Equal("", originalString);
    }
}