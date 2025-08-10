using System.Globalization;
using CsvTools.Parsers;

namespace UnitTestCsvTools.Tests;

public class GenericParserTests
{
    [Theory]
    [InlineData("123", 123)]
    [InlineData("-456", -456)]
    [InlineData("0", 0)]
    [InlineData("2147483647", 2147483647)]
    [InlineData("-2147483648", -2147483648)]
    public void Parse_WithValidIntegerInputs_ShouldParseCorrectly(string input, int expectedValue)
    {
        var parser = new GenericParser<int>();
        var culture = CultureInfo.InvariantCulture;
        var result = parser.Parse(input, culture);
        Assert.Equal(expectedValue, result);
    }

    [Theory]
    [InlineData("true", true)]
    [InlineData("false", false)]
    [InlineData("True", true)]
    [InlineData("False", false)]
    [InlineData("TRUE", true)]
    [InlineData("FALSE", false)]
    [InlineData("1", true)]
    [InlineData("0", false)]
    public void Parse_WithValidBooleanInputs_ShouldParseCorrectly(string input, bool expectedValue)
    {
        var parser = new GenericParser<bool>();
        var culture = CultureInfo.InvariantCulture;
        var result = parser.Parse(input, culture);
        Assert.Equal(expectedValue, result);
    }

    [Theory]
    [InlineData("Hello", "Hello")]
    [InlineData("", "")]
    [InlineData(" ", " ")]
    [InlineData("Special chars: äöüß", "Special chars: äöüß")]
    [InlineData("Numbers 123", "Numbers 123")]
    public void Parse_WithStringInputs_ShouldReturnSameString(string input, string expectedValue)
    {
        var parser = new GenericParser<string>();
        var culture = CultureInfo.InvariantCulture;
        var result = parser.Parse(input, culture);
        Assert.Equal(expectedValue, result);
    }

    [Theory]
    [InlineData("123.45", 123.45)]
    [InlineData("-99.9", -99.9)]
    [InlineData("0.0", 0.0)]
    [InlineData("1.7976931348623157E+308", 1.7976931348623157E+308)]
    public void Parse_WithValidDoubleInputs_ShouldParseCorrectly(string input, double expectedValue)
    {
        var parser = new GenericParser<double>();
        var culture = CultureInfo.InvariantCulture;
        var result = parser.Parse(input, culture);
        Assert.Equal(expectedValue, result, 10);
    }

    [Theory]
    [InlineData("invalid")]
    [InlineData("abc")]
    [InlineData("12abc")]
    [InlineData("abc12")]
    public void Parse_WithInvalidIntegerInput_ShouldReturnDefault(string input)
    {
        var parser = new GenericParser<int>();
        var culture = CultureInfo.InvariantCulture;
        var result = parser.Parse(input, culture);
        Assert.Equal(default(int), result);
    }

    [Fact]
    public void Parse_WithNullInput_ShouldReturnDefault()
    {
        var parser = new GenericParser<int>();
        var culture = CultureInfo.InvariantCulture;
        var result = parser.Parse(null, culture);
        Assert.Equal(default(int), result);
    }

    [Theory]
    [InlineData("en-US", "123")]
    [InlineData("en-US", "-456")]
    [InlineData("en-US", "0")]
    public void CanParse_WithValidInteger_ShouldReturnTrue(string cultureName, string input)
    {
        var parser = new GenericParser<int>();
        var culture = new CultureInfo(cultureName);
        var result = parser.CanParse(input, culture);
        Assert.True(result);
    }

    [Theory]
    [InlineData("en-US", "invalid")]
    [InlineData("en-US", "abc")]
    [InlineData("en-US", "12abc")]
    public void CanParse_WithInvalidInteger_ShouldReturnFalse(string cultureName, string input)
    {
        var parser = new GenericParser<int>();
        var culture = new CultureInfo(cultureName);
        var result = parser.CanParse(input, culture);
        Assert.False(result);
    }

    [Fact]
    public void CanParse_WithNull_ShouldReturnFalse()
    {
        var parser = new GenericParser<int>();
        var culture = CultureInfo.InvariantCulture;
        var result = parser.CanParse(null, culture);
        Assert.False(result);
    }

    [Theory]
    [InlineData("en-US", "true")]
    [InlineData("en-US", "false")]
    [InlineData("en-US", "1")]
    [InlineData("en-US", "0")]
    public void CanParse_WithValidBoolean_ShouldReturnTrue(string cultureName, string input)
    {
        var parser = new GenericParser<bool>();
        var culture = new CultureInfo(cultureName);
        var result = parser.CanParse(input, culture);
        Assert.True(result);
    }

    [Theory]
    [InlineData("en-US", "maybe")]
    [InlineData("en-US", "2")]
    [InlineData("en-US", "-1")]
    [InlineData("en-US", "yes")]
    [InlineData("en-US", "no")]
    public void CanParse_WithInvalidBoolean_ShouldReturnFalse(string cultureName, string input)
    {
        var parser = new GenericParser<bool>();
        var culture = new CultureInfo(cultureName);
        var result = parser.CanParse(input, culture);
        Assert.False(result);
    }

    [Fact]
    public void Parse_WithCultureSpecificNumber_ShouldRespectCulture()
    {
        var parser = new GenericParser<double>();
        var germanCulture = new CultureInfo("de-DE");
        const string input = "1234,56";
        var result = parser.Parse(input, germanCulture);
        Assert.Equal(1234.56, result, 2);
    }
}