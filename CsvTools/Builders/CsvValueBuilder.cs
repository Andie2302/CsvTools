using System.Globalization;
using CsvTools.Parsers;
using CsvTools.Values;

namespace CsvTools.Builders;

public class CsvValueBuilder < T > ( string? originalString )
{
    private CultureInfo _culture = CultureInfo.InvariantCulture;
    private IValueParser< T >? _customParser;
    private ParserConfiguration? _parserConfiguration;

    public CsvValueBuilder< T > WithCulture ( CultureInfo culture )
    {
        _culture = culture;

        return this;
    }

    public CsvValueBuilder< T > WithParser ( IValueParser< T > parser )
    {
        _customParser = parser;

        return this;
    }

    public CsvValueBuilder< T > WithParserConfiguration ( ParserConfiguration configuration )
    {
        _parserConfiguration = configuration;

        return this;
    }

    public CsvValue< T > Build()
    {
        var parser = GetParser();
        var parsedValue = parser.Parse ( originalString , _culture );
        var invariantInitial = new InvariantValue< T > ( parsedValue );
        var context = new CsvCreationContext< T > ( originalString , invariantInitial );

        return new CsvValue< T > { Context = context , CurrentValue = invariantInitial };
    }

    private IValueParser< T > GetParser() => _customParser ?? ( _parserConfiguration != null ? ParserFactory.GetParser< T > ( _parserConfiguration ) : ParserFactory.GetParser< T >() );
}
