using System.Globalization;
using CsvTools.Values;
using CsvTools.Values.Extensions;

namespace CsvTools.Rest;

public static class VisitorExamples
{
    public static void Example()
    {
        var germanCulture = new CultureInfo("de-DE");
        var usCulture = new CultureInfo("en-US");
        var decimalValue = new CsvValue<decimal>("1.234,56", 1234.56m).WithNewValue(9999.99m);
        var dateValue = new CsvValue<DateTime>("2023-12-31", new DateTime(2023, 12, 31));
        var stringValue = new CsvValue<string>("Hello", "Hello").WithNewValue("World");
        var germanStringVisitor = new CsvValueStringVisitor(germanCulture, "F2");

        Console.WriteLine("Deutsche Formatierung:");
        Console.WriteLine($"Decimal: {decimalValue.Accept(germanStringVisitor)}");
        Console.WriteLine($"Date: {dateValue.Accept(germanStringVisitor)}");
        Console.WriteLine($"String: {stringValue.Accept(germanStringVisitor)}");
        var exportVisitor = new CsvExportVisitor(usCulture);

        Console.WriteLine("\nUS Export-Format:");
        Console.WriteLine($"Decimal: {decimalValue.Accept(exportVisitor)}");
        Console.WriteLine($"Date: {dateValue.Accept(exportVisitor)}");
        Console.WriteLine($"String: {stringValue.Accept(exportVisitor)}");
        var statsVisitor = new CsvStatisticsVisitor();
        decimalValue.Accept(statsVisitor);
        dateValue.Accept(statsVisitor);
        stringValue.Accept(statsVisitor);

        Console.WriteLine($"\nStatistiken:");
        Console.WriteLine($"Total: {statsVisitor.TotalValues}");
        Console.WriteLine($"Modified: {statsVisitor.ModifiedValues}");
        Console.WriteLine($"Null: {statsVisitor.NullValues}");
    }
}