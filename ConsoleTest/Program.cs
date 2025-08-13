// Statt int für "1.234,56€"

using System.Globalization;

var result1 = CsvAdvancedParser.For< double >().WithCulture ( CultureInfo.GetCultureInfo ( "de-DE" ) ).WithNullValues ( "N/A" , "NULL" , "-" ).WithPreprocessor ( s => s.Replace ( "€" , "" ) ).Parse ( "1.234,56€" );
Console.WriteLine ( result1.Value );
Console.WriteLine ( result1.ErrorMessage );
Console.WriteLine ( result1.Success );
Console.WriteLine();
Console.WriteLine ( result1.ToString() );
Console.WriteLine();

// Verschiedene numerische Typen
var intResult = CsvAdvancedParser.For< int >().WithCulture ( CultureInfo.GetCultureInfo ( "de-DE" ) ).Parse ( "1.234" );
var decimalResult = CsvAdvancedParser.For< decimal >().WithCulture ( CultureInfo.GetCultureInfo ( "en-US" ) ).WithPreprocessor ( s => s.Replace ( "$" , "" ) ).Parse ( "$1,234.56" );

// Mit Null-Werten
var nullResult = CsvAdvancedParser.For< double >().WithNullValues ( "N/A" , "NULL" , "-" ).Parse ( "N/A" ); // Sollte null zurückgeben
Console.WriteLine ( intResult.Value );
Console.WriteLine ( decimalResult.Value );
Console.WriteLine ( nullResult.Value );
Console.WriteLine();
Console.WriteLine ( "intResult:"+intResult );
Console.WriteLine ( "decimalResult:"+decimalResult );
Console.WriteLine ( "nullResult:"+nullResult );
Console.WriteLine ( intResult.Value == null ? "intResult: null" : "intResult: not null" );
Console.WriteLine ( decimalResult.Value == null ? "decimalResult: null" : "decimalResult: not null" );
Console.WriteLine ( nullResult.Value == null ? "nullResult: null" : "nullResult: not null" );
Console.WriteLine();

