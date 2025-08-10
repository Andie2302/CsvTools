using System.Globalization;


namespace UnitTestCsvTools;

public class CsvValueBuilderTests
{
    [ Fact ]
    public void Build_WithValidGermanDecimal_ShouldParseCorrectly()
    {
        const string inputString = "1.234,56";
        var germanCulture = new CultureInfo ( "de-DE" );
        const decimal expectedValue = 1234.56m;
        var builder = new CsvTools.NEU.CsvValueBuilder< decimal > ( inputString );
        var csvValue = builder.WithCulture ( germanCulture ).Build();
        Assert.Equal ( expectedValue , csvValue.CurrentValue.Value );
    }

    [ Fact ]
    public void Build_WithInvalidString_ShouldReturnDefaultValue()
    {
        const string inputString = "abc";
        var builder = new CsvTools.NEU.CsvValueBuilder< decimal > ( inputString );
        var csvValue = builder.Build();
        Assert.Equal ( 0 , csvValue.CurrentValue.Value );
    }

    [ Theory ]
    [ InlineData ( "de-DE" , "1.234,56" , 1234.56 ) ]
    [ InlineData ( "en-US" , "1,234.56" , 1234.56 ) ]
    [ InlineData ( "fr-FR" , "1 234,56" , 1234.56 ) ]
    [ InlineData ( "de-DE" , "-99,9" , -99.9 ) ]
    [ InlineData ( "en-US" , "123456" , 123456 ) ]
    [ InlineData ( "en-US" , "invalid" , 0 ) ]
    [ InlineData ( "de-DE" , "" , 0 ) ]
    [ InlineData ( null , null , 0 ) ]
    public void Build_WithVariousDecimalInputs_ShouldParseCorrectly ( string? cultureName , string? inputString , decimal expectedValue )
    {
        var culture = cultureName != null ? new CultureInfo ( cultureName ) : CultureInfo.InvariantCulture;
        var builder = new CsvTools.NEU.CsvValueBuilder< decimal > ( inputString );
        var csvValue = builder.WithCulture ( culture ).Build();
        Assert.Equal ( expectedValue , csvValue.CurrentValue.Value );
    }

    [ Theory ]
    [ InlineData ( "de-DE" , "24.12.2025" , 2025 , 12 , 24 ) ]
    [ InlineData ( "en-US" , "12/24/2025" , 2025 , 12 , 24 ) ]
    [ InlineData ( "en-GB" , "24/12/2025" , 2025 , 12 , 24 ) ]
    [ InlineData ( "fr-FR" , "24/12/2025" , 2025 , 12 , 24 ) ]
    [ InlineData ( "ja-JP" , "2025/12/24" , 2025 , 12 , 24 ) ]
    public void Build_WithVariousValidDateTimeInputs_ShouldParseCorrectly ( string cultureName , string inputString , int year , int month , int day )
    {
        var expectedValue = new DateTime ( year , month , day );
        var culture = new CultureInfo ( cultureName );
        var builder = new CsvTools.NEU.CsvValueBuilder< DateTime > ( inputString );
        var csvValue = builder.WithCulture ( culture ).Build();
        Assert.Equal ( expectedValue , csvValue.CurrentValue.Value );
    }

    [ Theory ]
    [ InlineData ( "de-DE" , "ungültig" ) ]
    [ InlineData ( "en-US" , "" ) ]
    [ InlineData ( "en-US" , null ) ]
    public void Build_WithInvalidDateTimeInput_ShouldReturnDefault ( string cultureName , string? inputString )
    {
        var expectedValue = default ( DateTime );
        var culture = new CultureInfo ( cultureName );
        var builder = new CsvTools.NEU.CsvValueBuilder< DateTime > ( inputString );
        var csvValue = builder.WithCulture ( culture ).Build();
        Assert.Equal ( expectedValue , csvValue.CurrentValue.Value );
    }
}
