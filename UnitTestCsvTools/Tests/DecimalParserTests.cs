using System.Globalization;
using CsvTools.Parsers;

namespace UnitTestCsvTools.Tests;

public class DecimalParserTests
{
    [Theory]
    [InlineData("de-DE", "1.234,56", 1234.56)]
    [InlineData("en-US", "1,234.56", 1234.56)]
    [InlineData("fr-FR", "1 234,56", 1234.56)]
    [InlineData("de-DE", "-99,9", -99.9)]
    [InlineData("en-US", "123456", 123456)]
    [InlineData("de-DE", "0,5", 0.5)]
    [InlineData("en-US", "0.5", 0.5)]
    [InlineData("de-DE", "1000000,99", 1000000.99)]
    public void Parse_WithValidDecimalInputs_ShouldParseCorrectly(string cultureName, string input, decimal expectedValue)
    {
        // Arrange
        var parser = new DecimalParser();
        var culture = new CultureInfo(cultureName);

        // Act
        var result = parser.Parse(input, culture);

        // Assert
        Assert.Equal(expectedValue, result);
    }

    [Theory]
    [InlineData("de-DE", "€1.234,56", 1234.56)]
    [InlineData("en-US", "$1,234.56", 1234.56)]
    [InlineData("en-GB", "£1,234.56", 1234.56)]
    [InlineData("de-DE", "€-99,9", -99.9)]
    public void Parse_WithCurrencySymbols_ShouldParseCorrectly(string cultureName, string input, decimal expectedValue)
    {
        // Arrange
        var parser = new DecimalParser(NumberStyles.Number | NumberStyles.AllowCurrencySymbol);
        var culture = new CultureInfo(cultureName);

        // Act
        var result = parser.Parse(input, culture);

        // Assert
        Assert.Equal(expectedValue, result);
    }

    [Theory]
    [InlineData("de-DE", "invalid")]
    [InlineData("en-US", "abc")]
    [InlineData("en-US", "")]
    [InlineData("de-DE", "1,2,3")]
    [InlineData("en-US", "1.2.3")]
    [InlineData("de-DE", "text123")]
    [InlineData("en-US", "123text")]
    public void Parse_WithInvalidDecimalInput_ShouldReturnDefault(string cultureName, string input)
    {
        // Arrange
        var parser = new DecimalParser();
        var culture = new CultureInfo(cultureName);

        // Act
        var result = parser.Parse(input, culture);

        // Assert
        Assert.Equal(default(decimal), result);
    }

    [Fact]
    public void Parse_WithNullInput_ShouldReturnDefault()
    {
        // Arrange
        var parser = new DecimalParser();
        var culture = CultureInfo.InvariantCulture;

        // Act
        var result = parser.Parse(null, culture);

        // Assert
        Assert.Equal(default(decimal), result);
    }

    [Theory]
    [InlineData("1234.56")]
    [InlineData("1,234.56")]
    [InlineData("-99.9")]
    [InlineData("0.5")]
    [InlineData("123456")]
    [InlineData("$1,234.56")]
    [InlineData("€1.234,56")]
    public void CanParse_WithValidDecimal_ShouldReturnTrue(string input)
    {
        // Arrange
        var parser = new DecimalParser();

        // Act
        var result = parser.CanParse(input);

        // Assert
        Assert.True(result);
    }

    [Theory]
    [InlineData("invalid")]
    [InlineData("abc")]
    [InlineData("")]
    [InlineData("1,2,3")]
    [InlineData("text123")]
    [InlineData("123text456")]
    public void CanParse_WithInvalidDecimal_ShouldReturnFalse(string input)
    {
        // Arrange
        var parser = new DecimalParser();

        // Act
        var result = parser.CanParse(input);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void CanParse_WithNull_ShouldReturnFalse()
    {
        // Arrange
        var parser = new DecimalParser();

        // Act
        var result = parser.CanParse(null);

        // Assert
        Assert.False(result);
    }

    [Theory]
    [InlineData("1.234567890123456789012345678", 1.234567890123456789012345678)]
    [InlineData("0.0000000000000000000000000001", 0.0000000000000000000000000001)]
    [InlineData("79228162514264337593543950335", 79228162514264337593543950335M)]
    public void Parse_WithHighPrecisionDecimals_ShouldParseCorrectly(string input, decimal expectedValue)
    {
        // Arrange
        var parser = new DecimalParser();
        var culture = CultureInfo.InvariantCulture;

        // Act
        var result = parser.Parse(input, culture);

        // Assert
        Assert.Equal(expectedValue, result);
    }

    [Theory]
    [InlineData(NumberStyles.Number)]
    [InlineData(NumberStyles.Currency)]
    [InlineData(NumberStyles.Float)]
    [InlineData(NumberStyles.Integer)]
    public void Constructor_WithDifferentNumberStyles_ShouldWorkCorrectly(NumberStyles styles)
    {
        // Arrange & Act
        var parser = new DecimalParser(styles);

        // Assert
        Assert.NotNull(parser);
    }
}