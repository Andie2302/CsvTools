using System.Globalization;
using CsvTools.Builders;
using CsvTools.Parsers;

public class RefactoredCsvValueBuilderTests
{
    [ Theory ]
    [ InlineData ( "de-DE" , "1.234,56" , 1234.56 ) ]
    [ InlineData ( "en-US" , "1,234.56" , 1234.56 ) ]
    [ InlineData ( "fr-FR" , "1 234,56" , 1234.56 ) ]
    public void Build_WithRefactoredParser_ShouldWorkLikeOriginal ( string cultureName , string input , decimal expectedValue )
    {
        var culture = new CultureInfo ( cultureName );
        var builder = new CsvValueBuilder< decimal > ( input );
        var csvValue = builder.WithCulture ( culture ).Build();
        Assert.Equal ( expectedValue , csvValue.CurrentValue.Value );
        Assert.Equal ( input , csvValue.OriginalString );
    }

    [ Fact ]
    public void Build_WithCustomParser_ShouldUseCustomParser()
    {
        var customParser = new TestCustomParser();
        var builder = new CsvValueBuilder< int > ( "test" );
        var csvValue = builder.WithParser ( customParser ).Build();
        Assert.Equal ( 42 , csvValue.CurrentValue.Value );
    }

    [ Fact ]
    public void Build_WithParserConfiguration_ShouldUseConfiguration()
    {
        var configuration = ParserConfiguration.ForCurrency;
        var builder = new CsvValueBuilder< decimal > ( "$123.45" );
        var culture = new CultureInfo ( "en-US" );
        var csvValue = builder.WithParserConfiguration ( configuration ).WithCulture ( culture ).Build();
        Assert.Equal ( 123.45m , csvValue.CurrentValue.Value );
    }

    [ Theory ]
    [ InlineData ( "de-DE" , "24.12.2025" , 2025 , 12 , 24 ) ]
    [ InlineData ( "en-US" , "12/24/2025" , 2025 , 12 , 24 ) ]
    public void Build_WithDateTime_ShouldParseCorrectly ( string cultureName , string input , int year , int month , int day )
    {
        var expectedValue = new DateTime ( year , month , day );
        var culture = new CultureInfo ( cultureName );
        var builder = new CsvValueBuilder< DateTime > ( input );
        var csvValue = builder.WithCulture ( culture ).Build();
        Assert.Equal ( expectedValue , csvValue.CurrentValue.Value );
    }

    private class TestCustomParser : IValueParser< int >
    {
        public int Parse ( string? input , CultureInfo culture ) { return 42; }
        public bool CanParse ( string? input ) { return true; }
    }
}