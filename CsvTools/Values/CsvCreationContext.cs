namespace CsvTools.Values;

public readonly record struct CsvCreationContext < T > ( string? OriginalString , InvariantValue< T > InitialValue );