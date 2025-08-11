using System.Globalization;
using CsvTools.Values;

namespace UnitTestCsvTools.Tests;

public class CsvValueTests
{
    [Fact]
    public void Constructor_WithStringAndValue_SetsProperties()
    {
        var originalString = "123.45";
        var originalValue = 123.45m;
        var csvValue = new CsvValue<decimal>(originalString, originalValue);
        Assert.Equal(originalString, csvValue.OriginalString);
        Assert.Equal(originalValue, csvValue.OriginalValue);
        Assert.Equal(originalValue, csvValue.CurrentValue);
    }

    [Fact]
    public void Constructor_DefaultValues_SetsToNull()
    {
        var csvValue = new CsvValue<string>();
        Assert.Null(csvValue.OriginalString);
        Assert.Null(csvValue.OriginalValue);
        Assert.Null(csvValue.CurrentValue);
    }

    [Fact]
    public void IsModified_WhenValuesEqual_ReturnsFalse()
    {
        var csvValue = new CsvValue<int>("123", 123);
        Assert.False(csvValue.IsModified);
    }

    [Fact]
    public void IsModified_WhenValueChanged_ReturnsTrue()
    {
        var csvValue = new CsvValue<int>("123", 123);
        var modifiedValue = csvValue with { CurrentValue = 456 };
        Assert.True(modifiedValue.IsModified);
    }

    [Fact]
    public void IsModified_WithNullValues_ReturnsFalse()
    {
        var csvValue = new CsvValue<string?>(null, null);
        Assert.False(csvValue.IsModified);
    }

    [Fact]
    public void IsModified_WhenChangedFromNullToValue_ReturnsTrue()
    {
        var csvValue = new CsvValue<string?>(null, null);
        var modifiedValue = csvValue with { CurrentValue = "test" };
        Assert.True(modifiedValue.IsModified);
    }

    [Fact]
    public void IsModified_WhenChangedFromValueToNull_ReturnsTrue()
    {
        var csvValue = new CsvValue<string?>("test", "test");
        var modifiedValue = csvValue with { CurrentValue = null };
        Assert.True(modifiedValue.IsModified);
    }

    [Theory]
    [InlineData("123", "123")]
    [InlineData("Hello", "Hello")]
    [InlineData("", "")]
    public void ToString_WithStringValue_ReturnsCurrentValueString(string value, string expected)
    {
        var csvValue = new CsvValue<string>("original", value);
        var result = csvValue.ToString();
        Assert.Equal(expected, result);
    }

    [Fact]
    public void ToString_WithNullValue_ReturnsEmptyString()
    {
        var csvValue = new CsvValue<string?>(null, null);
        var result = csvValue.ToString();
        Assert.Equal(string.Empty, result);
    }

    [Fact]
    public void ToString_WithFormatProvider_UsesProvider()
    {
        var value = 1234.56m;
        var csvValue = new CsvValue<decimal>("1234.56", value);
        var culture = new CultureInfo("de-DE");
        var result = csvValue.ToString(culture);
        Assert.Equal("1234,56", result);
    }

    [Fact]
    public void ToString_WithFormatAndProvider_UsesFormatting()
    {
        var value = 1234.56m;
        var csvValue = new CsvValue<decimal>("1234.56", value);
        var culture = new CultureInfo("en-US");
        var result = csvValue.ToString("C", culture);
        Assert.StartsWith("$1,234.56", result);
    }

    [Fact]
    public void ToString_WithNonFormattableType_ReturnsToString()
    {
        var value = new object();
        var csvValue = new CsvValue<object>("test", value);
        var result = csvValue.ToString(CultureInfo.InvariantCulture);
        Assert.Equal(value.ToString(), result);
    }
}