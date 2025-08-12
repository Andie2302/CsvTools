

using System.Globalization;
using CsvTools.Parsers;

Console.WriteLine ( "Hello World!" );

// Traditionell
if (CsvUniversalParser.TryParseNullable<int>("123", out var result))
{
    // use result
}

// Mit Extensions
var value = "123".ParseOrNull<int>();
var valueWithDefault = "invalid".ParseOrDefault<int>(42);

// Mit Result Pattern
var parseResult = CsvAdvancedParser.TryParse<int>("123");
var finalValue = parseResult.Match(
    onSuccess: val => val ?? 0,
    onFailure: err => throw new Exception(err)
);

// Mit Fluent API
var result1 = CsvAdvancedParser.For<int>()
    .WithCulture(CultureInfo.GetCultureInfo("de-DE"))
    .WithNullValues("N/A", "NULL", "-")
    .WithPreprocessor(s => s.Replace("€", ""))
    .Parse("1.234,56€");
    
    
Console.WriteLine("==>"+result1.Success);
Console.WriteLine("==>"+result1.ErrorMessage);
Console.WriteLine("==>"+result1.Value);
