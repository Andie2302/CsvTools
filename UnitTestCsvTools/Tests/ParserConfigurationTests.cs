using System.Globalization;
using CsvTools.Parsers;

public class ParserConfigurationTests
{
    [ Fact ]
    public void Default_ShouldHaveCorrectDefaultValues()
    {
        var config = ParserConfiguration.Default;
        Assert.Equal ( DateTimeStyles.None , config.DateTimeStyles );
        Assert.Equal ( NumberStyles.Number | NumberStyles.AllowCurrencySymbol , config.NumberStyles );
    }

    [ Fact ]
    public void ForCurrency_ShouldHaveCurrencyNumberStyles()
    {
        var config = ParserConfiguration.ForCurrency;
        Assert.Equal ( NumberStyles.Currency , config.NumberStyles );
    }

    [ Fact ]
    public void ForStrictDateTime_ShouldHaveStrictDateTimeStyles()
    {
        var config = ParserConfiguration.ForStrictDateTime;
        Assert.Equal ( DateTimeStyles.None , config.DateTimeStyles );
    }

    [ Fact ]
    public void Constructor_ShouldAllowCustomConfiguration()
    {
        var customConfig = new ParserConfiguration { DateTimeStyles = DateTimeStyles.AssumeLocal | DateTimeStyles.AllowWhiteSpaces , NumberStyles = NumberStyles.Float | NumberStyles.AllowThousands };
        Assert.Equal ( DateTimeStyles.AssumeLocal | DateTimeStyles.AllowWhiteSpaces , customConfig.DateTimeStyles );
        Assert.Equal ( NumberStyles.Float | NumberStyles.AllowThousands , customConfig.NumberStyles );
    }
}