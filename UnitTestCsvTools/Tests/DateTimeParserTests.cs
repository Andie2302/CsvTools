using System.Globalization;
using CsvTools.Parsers;

namespace UnitTestCsvTools.Tests;

public class DateTimeParserTests
{
    [Theory]
    [InlineData("de-DE", "24.12.2025", 2025, 12, 24)]
    [InlineData("en-US", "12/24/2025", 2025, 12, 24)]
    [InlineData("en-GB", "24/12/2025", 2025, 12, 24)]
    [InlineData("fr-FR", "24/12/2025", 2025, 12, 24)]
    [InlineData("ja-JP", "2025/12/24", 2025, 12, 24)]
    [InlineData("sv-SE", "2025-12-24", 2025, 12, 24)]
    public void Parse_WithValidDateTimeInputs_ShouldParseCorrectly(string cultureName, string input, int year, int month, int day)
    {
        // Arrange
        var parser = new DateTimeParser();
        var culture = new CultureInfo(cultureName);
        var expectedValue = new DateTime(year, month, day);

        // Act
        var result = parser.Parse(input, culture);

        // Assert
        Assert.Equal(expectedValue, result);
    }

    [Theory]
    [InlineData("de-DE", "ungültig")]
    [InlineData("en-US", "invalid")]
    [InlineData("en-US", "")]
    [InlineData("en-US", "32/12/2025")] // Invalid day
    [InlineData("en-US", "12/32/2025")] // Invalid month
    [InlineData("de-DE", "24.13.2025")] // Invalid month
    public void Parse_WithInvalidDateTimeInput_ShouldReturnDefault(string cultureName, string input)
    {
        // Arrange
        var parser = new DateTimeParser();
        var culture = new CultureInfo(cultureName);

        // Act
        var result = parser.Parse(input, culture);

        // Assert
        Assert.Equal(default(DateTime), result);
    }

    [Fact]
    public void Parse_WithNullInput_ShouldReturnDefault()
    {
        // Arrange
        var parser = new DateTimeParser();
        var culture = CultureInfo.InvariantCulture;

        // Act
        var result = parser.Parse(null, culture);

        // Assert
        Assert.Equal(default(DateTime), result);
    }

    [Theory]
    [InlineData("2025-12-24T14:30:00", 2025, 12, 24, 14, 30, 0)]
    [InlineData("2025-12-24 14:30:00", 2025, 12, 24, 14, 30, 0)]
    [InlineData("24.12.2025 14:30", 2025, 12, 24, 14, 30, 0)]
    public void Parse_WithDateTime_ShouldParseTimeComponent(string input, int year, int month, int day, int hour, int minute, int second)
    {
        // Arrange
        var parser = new DateTimeParser();
        var culture = new CultureInfo("de-DE");
        var expectedValue = new DateTime(year, month, day, hour, minute, second);

        // Act
        var result = parser.Parse(input, culture);

        // Assert
        Assert.Equal(expectedValue, result);
    }

    [Theory]
    [InlineData("2025-12-24")]
    [InlineData("24.12.2025")]
    [InlineData("12/24/2025")]
    [InlineData("2025/12/24")]
    public void CanParse_WithValidDateTime_ShouldReturnTrue(string input)
    {
        // Arrange
        var parser = new DateTimeParser();

        // Act
        var result = parser.CanParse(input);

        // Assert
        Assert.True(result);
    }

    [Theory]
    [InlineData("invalid")]
    [InlineData("")]
    [InlineData("32/12/2025")]
    [InlineData("abc123")]
    public void CanParse_WithInvalidDateTime_ShouldReturnFalse(string input)
    {
        // Arrange
        var parser = new DateTimeParser();

        // Act
        var result = parser.CanParse(input);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void CanParse_WithNull_ShouldReturnFalse()
    {
        // Arrange
        var parser = new DateTimeParser();

        // Act
        var result = parser.CanParse(null);

        // Assert
        Assert.False(result);
    }

    [Theory]
    [InlineData(DateTimeStyles.None)]
    [InlineData(DateTimeStyles.AllowWhiteSpaces)]
    [InlineData(DateTimeStyles.AssumeLocal)]
    [InlineData(DateTimeStyles.AssumeUniversal)]
    public void Constructor_WithDifferentDateTimeStyles_ShouldWorkCorrectly(DateTimeStyles styles)
    {
        // Arrange & Act
        var parser = new DateTimeParser(styles);

        // Assert
        Assert.NotNull(parser);
    }
}