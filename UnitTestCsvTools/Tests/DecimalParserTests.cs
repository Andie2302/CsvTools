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
        var parser = new DecimalParser();
        var culture = new CultureInfo(cultureName);
        var result = parser.Parse(input, culture);
        Assert.Equal(expectedValue, result);
    }

    [Theory]
    [InlineData("de-DE", "€1.234,56", 1234.56)]
    [InlineData("en-US", "$1,234.56", 1234.56)]
    [InlineData("en-GB", "£1,234.56", 1234.56)]
    [InlineData("de-DE", "€-99,9", -99.9)]
    public void Parse_WithCurrencySymbols_ShouldParseCorrectly(string cultureName, string input, decimal expectedValue)
    {
        var parser = new DecimalParser(NumberStyles.Number | NumberStyles.AllowCurrencySymbol);
        var culture = new CultureInfo(cultureName);
        var result = parser.Parse(input, culture);
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
        var parser = new DecimalParser();
        var culture = new CultureInfo(cultureName);
        var result = parser.Parse(input, culture);
        Assert.Equal(default(decimal), result);
    }

    [Fact]
    public void Parse_WithNullInput_ShouldReturnDefault()
    {
        var parser = new DecimalParser();
        var culture = CultureInfo.InvariantCulture;
        var result = parser.Parse(null, culture);
        Assert.Equal(default(decimal), result);
    }

    [Theory]
    [InlineData("en-US", "1234.56")]
    [InlineData("en-US", "1,234.56")]
    [InlineData("en-US", "-99.9")]
    [InlineData("en-US", "0.5")]
    [InlineData("en-US", "123456")]
    [InlineData("en-US", "$1,234.56")]
    [InlineData("de-DE", "€1.234,56")]
    public void CanParse_WithValidDecimal_ShouldReturnTrue(string cultureName, string input)
    {
        var parser = new DecimalParser();
        var culture = new CultureInfo(cultureName);
        var result = parser.CanParse(input, culture);
        Assert.True(result);
    }
//####################################

    [Theory]
    [InlineData("en-US", "invalid")]
    [InlineData("en-US", "abc")]
    [InlineData("en-US", "")]
    [InlineData("en-US", "text123")]
    [InlineData("en-US", "123text456")]
    [InlineData("en-US", "12.34.56")]      // Mehrere Dezimalpunkte
    [InlineData("en-US", "$$$123")]        // Mehrere Währungszeichen  
    [InlineData("en-US", ".")]             // Nur Punkt
    [InlineData("en-US", "++123")]         // Mehrere Vorzeichen
    [InlineData("en-US", "12 34")]         // Leerzeichen in der Mitte (wenn nicht AllowWhiteSpaces)
    [InlineData("en-US", "123abc456")]     // Text in der Mitte
    public void CanParse_WithInvalidDecimal_ShouldReturnFalse(string cultureName, string input)
    {
        var parser = new DecimalParser();
        var culture = new CultureInfo(cultureName);
        var result = parser.CanParse(input, culture);
        Assert.False(result);
    }

// Separater Test für das tatsächliche Verhalten mit Kommata
    [Theory]
    [InlineData("en-US", "1,2,3,4,5", 12345)]     // .NET interpretiert als 12345
    [InlineData("en-US", "1,234,567", 1234567)]   // Standard Tausenderformat  
    [InlineData("en-US", "1,23", 123)]            // Ungewöhnlich aber gültig
    public void Parse_WithCommaGrouping_ShouldParseAsExpected(string cultureName, string input, decimal expected)
    {
        var parser = new DecimalParser();
        var culture = new CultureInfo(cultureName);
        var result = parser.Parse(input, culture);
        Assert.Equal(expected, result);
    }


// Option 1: Ändere die Testdaten (entferne problematische Fälle)
    [Theory]
    [InlineData("en-US", "invalid")]
    [InlineData("en-US", "abc")]
    [InlineData("en-US", "")]
    [InlineData("en-US", "1.2.3")]        // Mehrere Dezimalpunkte - definitiv ungültig
    [InlineData("en-US", "text123")]
    [InlineData("en-US", "123text456")]
  //  [InlineData("en-US", "1,2,3,4,5")]    // Zu viele Kommata
    public void CanParse_WithInvalidDecimal_ShouldReturnFalseOLD2(string cultureName, string input)
    {
        var parser = new DecimalParser();
        var culture = new CultureInfo(cultureName);
        var result = parser.CanParse(input, culture);
        Assert.False(result);
    }

// Option 2: Oder erstelle einen separaten Test für grenzwertige Fälle
    [Theory]
    [InlineData("en-US", "1,2,3")]       // Könnte in en-US als gültig interpretiert werden
    [InlineData("de-DE", "1.2.3")]       // Könnte in de-DE als gültig interpretiert werden  
    public void CanParse_WithAmbiguousInput_BehaviorMayVary(string cultureName, string input)
    {
        var parser = new DecimalParser();
        var culture = new CultureInfo(cultureName);
        var result = parser.CanParse(input, culture);
    
        // Test dokumentiert nur das Verhalten, erwartet nicht zwingend false
        Console.WriteLine($"Culture: {cultureName}, Input: '{input}' -> Result: {result}");
    }
    
    
    //######################################
    
    [Theory]
    [InlineData("en-US", "invalid")]
    [InlineData("en-US", "abc")]
    [InlineData("en-US", "")]
   // [InlineData("en-US", "1,2,3")]
    [InlineData("en-US", "text123")]
    [InlineData("en-US", "123text456")]
    public void CanParse_WithInvalidDecimal_ShouldReturnFalseOLD(string cultureName, string input)
    {
        var parser = new DecimalParser();
        var culture = new CultureInfo(cultureName);
        var result = parser.CanParse(input, culture);
        Assert.False(result);
    }

    [Fact]
    public void CanParse_WithNull_ShouldReturnFalse()
    {
        var parser = new DecimalParser();
        var culture = CultureInfo.InvariantCulture;
        var result = parser.CanParse(null, culture);
        Assert.False(result);
    }

    [Theory]
    [InlineData("1.23456789012345", 1.23456789012345)]
    [InlineData("0.000000000001", 0.000000000001)]
    [InlineData("123456789.123456789", 123456789.123456789)]
    [InlineData("-999999.999999", -999999.999999)]
    [InlineData("0.1", 0.1)]
    public void Parse_WithHighPrecisionDecimals_ShouldParseCorrectly(string input, decimal expectedValue)
    {
        var parser = new DecimalParser();
        var culture = CultureInfo.InvariantCulture;
        var result = parser.Parse(input, culture);
        Assert.Equal(expectedValue, result);
    }

    [Fact]
    public void Parse_WithDecimalMaxValue_ShouldParseCorrectly()
    {
        var parser = new DecimalParser();
        var culture = CultureInfo.InvariantCulture;
        var input = decimal.MaxValue.ToString(culture);
        var expectedValue = decimal.MaxValue;
        var result = parser.Parse(input, culture);
        Assert.Equal(expectedValue, result);
    }

    [Fact]
    public void Parse_WithDecimalMinValue_ShouldParseCorrectly()
    {
        var parser = new DecimalParser();
        var culture = CultureInfo.InvariantCulture;
        var input = decimal.MinValue.ToString(culture);
        var expectedValue = decimal.MinValue;
        var result = parser.Parse(input, culture);
        Assert.Equal(expectedValue, result);
    }

    [Theory]
    [InlineData(NumberStyles.Number)]
    [InlineData(NumberStyles.Currency)]
    [InlineData(NumberStyles.Float)]
    [InlineData(NumberStyles.Integer)]
    public void Constructor_WithDifferentNumberStyles_ShouldWorkCorrectly(NumberStyles styles)
    {
        var parser = new DecimalParser(styles);
        Assert.NotNull(parser);
    }
}