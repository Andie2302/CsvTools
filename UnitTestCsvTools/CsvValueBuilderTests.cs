// CsvTools.Tests/CsvValueBuilderTests.cs

using System.Globalization;
using CsvTools.Builders;

// <- Wichtig: using für xUnit hinzufügen

namespace UnitTestCsvTools;

public class CsvValueBuilderTests
{
    [Fact]
    public void Build_WithValidGermanDecimal_ShouldParseCorrectly()
    {
        // ARRANGE
        var inputString = "1.234,56";
        var germanCulture = new CultureInfo("de-DE");
        var expectedValue = 1234.56m;

        // ACT
        var builder = new CsvValueBuilder<decimal>(inputString);
        var csvValue = builder.WithCulture(germanCulture).Build();

        // ASSERT
        Assert.Equal(expectedValue, csvValue.CurrentValue.Value);
    }

    [Fact]
    public void Build_WithInvalidString_ShouldReturnDefaultValue()
    {
        // ARRANGE
        // "abc" kann definitiv nicht in eine Zahl umgewandelt werden.
        var inputString = "abc";

        // ACT
        var builder = new CsvValueBuilder<decimal>(inputString);
        var csvValue = builder.Build();

        // ASSERT
        // Wir erwarten, dass der Wert 'default' (also 0m) ist, weil die Umwandlung fehlschlägt.
        Assert.Equal(default, csvValue.CurrentValue.Value);
    }
}
