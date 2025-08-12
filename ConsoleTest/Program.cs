// Statt int für "1.234,56€"

using System.Globalization;
using CsvTools.Parsers;

var result1 = CsvAdvancedParser.For<double>()
    .WithCulture(CultureInfo.GetCultureInfo("de-DE"))
    .WithNullValues("N/A", "NULL", "-")
    .WithPreprocessor(s => s.Replace("€", ""))
    .Parse("1.234,56€");

Console.WriteLine(result1.Value);
Console.WriteLine(result1.ErrorMessage);
Console.WriteLine(result1.Success);
Console.WriteLine();
Console.WriteLine(result1.ToString());
Console.WriteLine();