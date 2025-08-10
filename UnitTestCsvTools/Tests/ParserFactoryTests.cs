using System.Globalization;
using CsvTools.Parsers;

namespace UnitTestCsvTools.Tests;

public class ParserFactoryTests
{
    [Fact]
    public void GetParser_ForDateTime_ShouldReturnDateTimeParser()
    {
        // Act
        var parser = ParserFactory.GetParser<DateTime>();

        // Assert
        Assert.IsType<DateTimeParser>(parser);
    }

    [Fact]
    public void GetParser_ForNullableDateTime_ShouldReturnDateTimeParser()
    {
        // Act
        var parser = ParserFactory.GetParser<DateTime?>();

        // Assert
        Assert.IsType<DateTimeParser>(parser);
    }

    [Fact]
    public void GetParser_ForDecimal_ShouldReturnDecimalParser()
    {
        // Act
        var parser = ParserFactory.GetParser<decimal>();

        // Assert
        Assert.IsType<DecimalParser>(parser);
    }

    [Fact]
    public void GetParser_ForNullableDecimal_ShouldReturnDecimalParser()
    {
        // Act
        var parser = ParserFactory.GetParser<decimal?>();

        // Assert
        Assert.IsType<DecimalParser>(parser);
    }

    [Fact]
    public void GetParser_ForInt_ShouldReturnGenericParser()
    {
        // Act
        var parser = ParserFactory.GetParser<int>();

        // Assert
        Assert.IsType<GenericParser<int>>(parser);
    }

    [Fact]
    public void GetParser_ForString_ShouldReturnGenericParser()
    {
        // Act
        var parser = ParserFactory.GetParser<string>();

        // Assert
        Assert.IsType<GenericParser<string>>(parser);
    }

    [Fact]
    public void GetParser_CalledTwiceForSameType_ShouldReturnSameInstance()
    {
        // Act
        var parser1 = ParserFactory.GetParser<int>();
        var parser2 = ParserFactory.GetParser<int>();

        // Assert
        Assert.Same(parser1, parser2);
    }

    [Fact]
    public void GetParser_WithConfiguration_ShouldCreateNewInstance()
    {
        // Arrange
        var configuration = new ParserConfiguration
        {
            DateTimeStyles = DateTimeStyles.AssumeLocal
        };

        // Act
        var parser1 = ParserFactory.GetParser<DateTime>(configuration);
        var parser2 = ParserFactory.GetParser<DateTime>(configuration);

        // Assert
        Assert.NotSame(parser1, parser2);
        Assert.IsType<DateTimeParser>(parser1);
        Assert.IsType<DateTimeParser>(parser2);
    }

    [Fact]
    public void GetParser_WithCurrencyConfiguration_ShouldCreateDecimalParserWithCurrencyStyles()
    {
        // Arrange
        var configuration = ParserConfiguration.ForCurrency;

        // Act
        var parser = ParserFactory.GetParser<decimal>(configuration);
        var decimalParser = Assert.IsType<DecimalParser>(parser);

        // Test that it can parse currency
        var result = decimalParser.Parse("$123.45", new CultureInfo("en-US"));

        // Assert
        Assert.Equal(123.45m, result);
    }

    [Fact]
    public void ClearCache_ShouldRemoveAllCachedParsers()
    {
        // Arrange
        var parser1 = ParserFactory.GetParser<int>();

        // Act
        ParserFactory.ClearCache();
        var parser2 = ParserFactory.GetParser<int>();

        // Assert
        Assert.NotSame(parser1, parser2);
    }
}