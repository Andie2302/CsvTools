using System.Globalization;

// Detaillierte Analyse des Problems
var culture = new CultureInfo("en-US");
var testInput = "1,2,3,4,5";

Console.WriteLine($"Culture: {culture.Name}");
Console.WriteLine($"Number Decimal Separator: '{culture.NumberFormat.NumberDecimalSeparator}'");
Console.WriteLine($"Number Group Separator: '{culture.NumberFormat.NumberGroupSeparator}'");
Console.WriteLine($"Currency Decimal Separator: '{culture.NumberFormat.CurrencyDecimalSeparator}'");
Console.WriteLine($"Currency Group Separator: '{culture.NumberFormat.CurrencyGroupSeparator}'");
Console.WriteLine();

var numberStyles = NumberStyles.Number | NumberStyles.AllowCurrencySymbol;
Console.WriteLine($"Using NumberStyles: {numberStyles}");
Console.WriteLine();

// Test verschiedene Varianten
var testCases = new[] {
    "1,2,3,4,5",
    "1,234,567", 
    "1,23,4,5",
    "12,34,567",
    "1,234",
    "1,2",
    "12,345.67"
};

Console.WriteLine("Testing decimal.TryParse directly:");
foreach (var input in testCases)
{
    var success = decimal.TryParse(input, numberStyles, culture, out var value);
    Console.WriteLine($"'{input}' -> Success: {success}, Value: {value}");
}

Console.WriteLine();

// Test mit verschiedenen NumberStyles
var styleVariants = new[] {
    NumberStyles.Number,
    NumberStyles.Number | NumberStyles.AllowCurrencySymbol,
    NumberStyles.AllowThousands | NumberStyles.AllowDecimalPoint,
    NumberStyles.Integer,
    NumberStyles.Float
};

Console.WriteLine($"Testing '{testInput}' with different NumberStyles:");
foreach (var style in styleVariants)
{
    var success = decimal.TryParse(testInput, style, culture, out var value);
    Console.WriteLine($"{style}: Success: {success}, Value: {value}");
}

Console.WriteLine();

// Test der Parser-Implementierung
//var parser = new DecimalParser();
//var canParse = parser.CanParse(testInput, culture);
//var parseResult = parser.Parse(testInput, culture);

//Console.WriteLine($"DecimalParser Results:");
//Console.WriteLine($"CanParse('{testInput}'): {canParse}");
//Console.WriteLine($"Parse('{testInput}'): {parseResult}");

// Prüfe, ob es an der NumberStyles-Konstante liegt
var defaultStyles = NumberStyles.Number | NumberStyles.AllowCurrencySymbol;
Console.WriteLine($"\nDetailed NumberStyles analysis:");
Console.WriteLine($"NumberStyles.Number: {NumberStyles.Number}");
Console.WriteLine($"NumberStyles.AllowCurrencySymbol: {NumberStyles.AllowCurrencySymbol}");
Console.WriteLine($"Combined: {defaultStyles}");

// Einzeln testen
Console.WriteLine($"\nTesting components of NumberStyles.Number:");
var numberComponents = new[] {
    NumberStyles.AllowLeadingWhite,
    NumberStyles.AllowTrailingWhite, 
    NumberStyles.AllowLeadingSign,
    NumberStyles.AllowThousands,
    NumberStyles.AllowDecimalPoint
};

foreach (var component in numberComponents)
{
    if ((NumberStyles.Number & component) == component)
    {
        Console.WriteLine($"NumberStyles.Number includes: {component}");
    }
}