using System.Globalization;
using CsvTools.Parsers;

public class ParserFactoryTests
{
    [ Fact ]
    public void GetParser_ForDateTime_ShouldReturnDateTimeParser()
    {
        var parser = ParserFactory.GetParser< DateTime >();
        Assert.IsType< DateTimeParser > ( parser );
    }

    [ Fact ]
    public void GetParser_ForNullableDateTime_ShouldReturnDateTimeParser()
    {
        var parser = ParserFactory.GetParser< DateTime? >();
        Assert.IsType< DateTimeParser > ( parser );
    }

    [ Fact ]
    public void GetParser_ForDecimal_ShouldReturnDecimalParser()
    {
        var parser = ParserFactory.GetParser< decimal >();
        Assert.IsType< DecimalParser > ( parser );
    }

    [ Fact ]
    public void GetParser_ForNullableDecimal_ShouldReturnDecimalParser()
    {
        var parser = ParserFactory.GetParser< decimal? >();
        Assert.IsType< DecimalParser > ( parser );
    }

    [ Fact ]
    public void GetParser_ForInt_ShouldReturnGenericParser()
    {
        var parser = ParserFactory.GetParser< int >();
        Assert.IsType< GenericParser< int > > ( parser );
    }

    [ Fact ]
    public void GetParser_ForString_ShouldReturnGenericParser()
    {
        var parser = ParserFactory.GetParser< string >();
        Assert.IsType< GenericParser< string > > ( parser );
    }

    [ Fact ]
    public void GetParser_CalledTwiceForSameType_ShouldReturnSameInstance()
    {
        var parser1 = ParserFactory.GetParser< int >();
        var parser2 = ParserFactory.GetParser< int >();
        Assert.Same ( parser1 , parser2 );
    }

    [ Fact ]
    public void GetParser_WithConfiguration_ShouldCreateNewInstance()
    {
        var configuration = new ParserConfiguration { DateTimeStyles = DateTimeStyles.AssumeLocal };
        var parser1 = ParserFactory.GetParser< DateTime > ( configuration );
        var parser2 = ParserFactory.GetParser< DateTime > ( configuration );
        Assert.NotSame ( parser1 , parser2 );
        Assert.IsType< DateTimeParser > ( parser1 );
        Assert.IsType< DateTimeParser > ( parser2 );
    }

    [ Fact ]
    public void GetParser_WithCurrencyConfiguration_ShouldCreateDecimalParserWithCurrencyStyles()
    {
        var configuration = ParserConfiguration.ForCurrency;
        var parser = ParserFactory.GetParser< decimal > ( configuration );
        var decimalParser = Assert.IsType< DecimalParser > ( parser );
        var result = decimalParser.Parse ( "$123.45" , new CultureInfo ( "en-US" ) );
        Assert.Equal ( 123.45m , result );
    }

    [ Fact ]
    public void ClearCache_ShouldRemoveAllCachedParsers()
    {
        var parser1 = ParserFactory.GetParser< int >();
        ParserFactory.ClearCache();
        var parser2 = ParserFactory.GetParser< int >();
        Assert.NotSame ( parser1 , parser2 );
    }
}