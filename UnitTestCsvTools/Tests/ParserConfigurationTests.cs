using System.Globalization;
using CsvTools.Parsers;

namespace UnitTestCsvTools.Tests;

public class ParserConfigurationTests
{
    [Fact]
    public void Default_ShouldHaveCorrectDefaultValues()
    {
        // Act
        var config = ParserConfiguration.Default;

        // Assert
        Assert.Equal(DateTimeStyles.None, config.DateTimeStyles);
        Assert.Equal(NumberStyles.Number | NumberStyles.AllowCurrencySymbol, config.NumberStyles);
    }

    [Fact]
    public void ForCurrency_ShouldHaveCurrencyNumberStyles()
    {
        // Act
        var config = ParserConfiguration.ForCurrency;

        // Assert
        Assert.Equal(NumberStyles.Currency, config.NumberStyles);
    }

    [Fact]
    public void ForStrictDateTime_ShouldHaveStrictDateTimeStyles()
    {
        // Act
        var config = ParserConfiguration.ForStrictDateTime;

        // Assert
        Assert.Equal(DateTimeStyles.None, config.DateTimeStyles);
    }

    [Fact]
    public void Constructor_ShouldAllowCustomConfiguration()
    {
        // Arrange
        var customConfig = new ParserConfiguration
        {
            DateTimeStyles = DateTimeStyles.AssumeLocal | DateTimeStyles.AllowWhiteSpaces,
            NumberStyles = NumberStyles.Float | NumberStyles.AllowThousands
        };

        // Assert
        Assert.Equal(DateTimeStyles.AssumeLocal | DateTimeStyles.AllowWhiteSpaces, customConfig.DateTimeStyles);
        Assert.Equal(NumberStyles.Float | NumberStyles.AllowThousands, customConfig.NumberStyles);
    }
}