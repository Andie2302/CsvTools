namespace CsvTools.Values;

public readonly record struct CsvOriginal < T > ( string? String , Invariant< T > Value );