using System.Globalization;
using CsvTools.Builders;

namespace UnitTestCsvTools;

public class CsvValueBuilderTests
{
    [ Fact ]
    public void Build_WithGermanCulture_ShouldParseDecimalCorrectly()
    {
        const string inputString = "1.234,56";
        var germanCulture = new CultureInfo ( "de-DE" );
        const decimal expectedValue = 1234.56m;
        var builder = new CsvValueBuilder< decimal > ( inputString );
        var csvValue = builder.WithCulture ( germanCulture ).Build();
        Assert.False ( csvValue.IsModified );
        Assert.Equal ( expectedValue , csvValue.CurrentValue.Value );
        Assert.Equal ( inputString , csvValue.OriginalString );
    }
}
