using System.Globalization;

namespace CsvTools.Values;

public readonly record struct CsvValue < T >
{
    public CsvCreationContext< T > Context { get; init; }
    public InvariantValue< T > CurrentValue { get; init; }
    public string? OriginalString => Context.OriginalString;
    public InvariantValue< T > InitialValue => Context.InitialValue;
    public bool IsModified => !EqualityComparer< InvariantValue< T > >.Default.Equals ( InitialValue , CurrentValue );
    public CsvValue< T > WithNewValue ( T? newValue ) { return this with { CurrentValue = new InvariantValue< T > ( newValue ) }; }
    public CsvValue< T > ResetToOriginal() { return this with { CurrentValue = InitialValue }; }
    public string? ToString ( IFormatProvider cultureInfo ) { return string.Format ( cultureInfo , "{0}" , CurrentValue.Value ); }
    public override string? ToString() { return ToString ( CultureInfo.InvariantCulture ); }
}