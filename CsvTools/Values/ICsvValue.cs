namespace CsvTools.Values;

public interface ICsvValue
{
    string? OriginalString { get; }
    object? InitialValue { get; }
    object? CurrentValue { get; }
    bool IsModified { get; }
}