namespace CsvTools.Values;

public readonly record struct CsvCreationContext < T > ( string? OriginalString , T? InitialValue );