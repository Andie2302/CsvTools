namespace CsvTools.Values;

public readonly record struct CsvOriginalValues < T > ( string? String , CsvInvariantValue< T > Value );