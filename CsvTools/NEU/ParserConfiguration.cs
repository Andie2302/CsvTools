using System.Globalization;

namespace CsvTools.NEU;

public class ParserConfiguration
{
    public DateTimeStyles DateTimeStyles { get; init; } = DateTimeStyles.None;
    public NumberStyles NumberStyles { get; init; } = NumberStyles.Number | NumberStyles.AllowCurrencySymbol;
    public static ParserConfiguration Default => new ParserConfiguration();
    public static ParserConfiguration ForCurrency => new ParserConfiguration { NumberStyles = NumberStyles.Currency };
    public static ParserConfiguration ForStrictDateTime => new ParserConfiguration { DateTimeStyles = DateTimeStyles.None };
}
