using System.Globalization;
using CsvTools.Parsers;

namespace UnitTestCsvTools.Tests;

public class GenericParserTests
{
    [Theory]
    [InlineData("123", 123)]
    [InlineData("-456", -456)]
    [InlineData("0", 0)]
    [InlineData("2147483647", 2147483647)] // Int32.MaxValue
    [InlineData("-2147483648", -2147483648)] // Int32.MinValue
    public void Parse_WithValidIntegerInputs_ShouldParseCorrectly(string input, int expectedValue)
    {
        // Arrange
        var parser = new GenericParser<int>();
        var culture = CultureInfo.InvariantCulture;

        // Act
        var result = parser.Parse(input, culture);

        // Assert
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
        // Arrange
        var parser = new GenericParser<bool>();
        var culture = CultureInfo.InvariantCulture;

        // Act
        var result = parser.Parse(input, culture);

        // Assert
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
        // Arrange
        var parser = new GenericParser<string>();
        var culture = CultureInfo.InvariantCulture;

        // Act
        var result = parser.Parse(input, culture);

        // Assert
        Assert.Equal(expectedValue, result);
    }

    [Theory]
    [InlineData("123.45", 123.45)]
    [InlineData("-99.9", -99.9)]
    [InlineData("0.0", 0.0)]
    [InlineData("1.7976931348623157E+308", 1.7976931348623157E+308)] // Double.MaxValue approximation
    public void Parse_WithValidDoubleInputs_ShouldParseCorrectly(string input, double expectedValue)
    {
        // Arrange
        var parser = new GenericParser<double>();
        var culture = CultureInfo.InvariantCulture;

        // Act
        var result = parser.Parse(input, culture);

        // Assert
        Assert.Equal(expectedValue, result, 10); // 10 decimal places precision
    }

    [Theory]
    [InlineData("invalid")]
    [InlineData("abc")]
    [InlineData("12abc")]
    [InlineData("abc12")]
    public void Parse_WithInvalidIntegerInput_ShouldReturnDefault(string input)
    {
        // Arrange
        var parser = new GenericParser<int>();
        var culture = CultureInfo.InvariantCulture;

        // Act
        var result = parser.Parse(input, culture);

        // Assert
        Assert.Equal(default(int), result);
    }

    [Fact]
    public void Parse_WithNullInput_ShouldReturnDefault()
    {
        // Arrange
        var parser = new GenericParser<int>();
        var culture = CultureInfo.InvariantCulture;

        // Act
        var result = parser.Parse(null, culture);

        // Assert
        Assert.Equal(default(int), result);
    }

    [Theory]
    [InlineData("123")]
    [InlineData("-456")]
    [InlineData("0")]
    public void CanParse_WithValidInteger_ShouldReturnTrue(string input)
    {
        // Arrange
        var parser = new GenericParser<int>();

        // Act
        var result = parser.CanParse(input);

        // Assert
        Assert.True(result);
    }

    [Theory]
    [InlineData("invalid")]
    [InlineData("abc")]
    [InlineData("12abc")]
    public void CanParse_WithInvalidInteger_ShouldReturnFalse(string input)
    {
        // Arrange
        var parser = new GenericParser<int>();

        // Act
        var result = parser.CanParse(input);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void CanParse_WithNull_ShouldReturnFalse()
    {
        // Arrange
        var parser = new GenericParser<int>();

        // Act
        var result = parser.CanParse(null);

        // Assert
        Assert.False(result);
    }

    [Theory]
    [InlineData("true")]
    [InlineData("false")]
    [InlineData("1")]
    [InlineData("0")]
    public void CanParse_WithValidBoolean_ShouldReturnTrue(string input)
    {
        // Arrange
        var parser = new GenericParser<bool>();

        // Act
        var result = parser.CanParse(input);

        // Assert
        Assert.True(result);
    }

    [Theory]
    [InlineData("maybe")]
    [InlineData("2")]
    [InlineData("-1")]
    [InlineData("yes")]
    [InlineData("no")]
    public void CanParse_WithInvalidBoolean_ShouldReturnFalse(string input)
    {
        // Arrange
        var parser = new GenericParser<bool>();

        // Act
        var result = parser.CanParse(input);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void Parse_WithCultureSpecificNumber_ShouldRespectCulture()
    {
        // Arrange
        var parser = new GenericParser<double>();
        var germanCulture = new CultureInfo("de-DE");
        const string input = "1234,56"; // German decimal format

        // Act
        var result = parser.Parse(input, germanCulture);

        // Assert
        Assert.Equal(1234.56, result, 2);
    }
}