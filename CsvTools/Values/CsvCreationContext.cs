namespace CsvTools.Values;

public readonly record struct CsvCreationContext < T > ( string? String , InvariantValue< T > Value );