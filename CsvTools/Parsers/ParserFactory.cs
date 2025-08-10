using System.Globalization;

namespace CsvTools.Parsers;

public static class ParserFactory
{
    private static readonly Dictionary< Type , object > CachedParsers = new Dictionary< Type , object >();

    public static IValueParser< T > GetParser < T >()
    {
        var type = typeof ( T );

        if ( CachedParsers.TryGetValue ( type , out var cachedParser ) ) { return (IValueParser< T >) cachedParser; }

        var parser = CreateParser< T >();
        CachedParsers[type] = parser;

        return parser;
    }

    public static IValueParser< T > GetParser < T > ( ParserConfiguration configuration ) { return CreateParser< T > ( configuration ); }

    private static IValueParser< T > CreateParser < T > ( ParserConfiguration? configuration = null )
    {
        var type = typeof ( T );

        if ( type == typeof ( DateTime ) || type == typeof ( DateTime? ) ) {
            var dateTimeStyles = configuration?.DateTimeStyles ?? DateTimeStyles.None;

            return (IValueParser< T >) new DateTimeParser ( dateTimeStyles );
        }

        if ( type != typeof ( decimal ) && type != typeof ( decimal? ) ) { return new GenericParser< T >(); }

        var numberStyles = configuration?.NumberStyles ?? NumberStyles.Number | NumberStyles.AllowCurrencySymbol;

        return (IValueParser< T >) new DecimalParser ( numberStyles );
    }

    public static void ClearCache() { CachedParsers.Clear(); }
}
