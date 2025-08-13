using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Numerics;
using System.Runtime.CompilerServices;
using static StaticCsvUniversalParserMethods;

public static class StaticCsvUniversalParserMethods
{
    public static readonly DateTimeStyles DefaultDateTimeStyles = DateTimeStyles.None;
    public static readonly CultureInfo DefaultCulture = CultureInfo.InvariantCulture;
    public static readonly NumberStyles DefaultNumberStyles = NumberStyles.Any;

    #region Numeric Types (INumber<T>)
    [ MethodImpl ( MethodImplOptions.AggressiveInlining ) ]
    public static bool TryParseNullable < T > ( string? cellToken , CultureInfo culture , NumberStyles numberStyles , [ NotNullWhen ( true ) ] out T? result ) where T : INumber< T >
    {
        if ( string.IsNullOrWhiteSpace ( cellToken ) ) {
            result = default;

            return result is not null;
        }

        if ( T.TryParse ( cellToken , numberStyles , culture , out var value ) ) {
            result = value;

            return result is not null;
        }

        result = default;

        return result is not null;
    }

    [ MethodImpl ( MethodImplOptions.AggressiveInlining ) ]
    public static bool TryParseNullable < T > ( string? cellToken , CultureInfo culture , [ NotNullWhen ( true ) ] out T? result ) where T : INumber< T > => TryParseNullable ( cellToken , culture , DefaultNumberStyles , out result );

    [ MethodImpl ( MethodImplOptions.AggressiveInlining ) ]
    public static bool TryParseNullable < T > ( string? cellToken , [ NotNullWhen ( true ) ] out T? result ) where T : INumber< T > => TryParseNullable ( cellToken , DefaultCulture , DefaultNumberStyles , out result );
    #endregion

    #region DateTime Types
    public static bool TryParseNullable ( string? cellToken , CultureInfo culture , DateTimeStyles dateTimeStyles , [ NotNullWhen ( true ) ] out DateTime? result ) => TryParseNullableCore ( cellToken , token => DateTime.TryParse ( token , culture , dateTimeStyles , out var value ) ? value : null , out result );
    public static bool TryParseNullable ( string? cellToken , CultureInfo culture , [ NotNullWhen ( true ) ] out DateTime? result ) => TryParseNullable ( cellToken , culture , DefaultDateTimeStyles , out result );
    public static bool TryParseNullable ( string? cellToken , [ NotNullWhen ( true ) ] out DateTime? result ) => TryParseNullable ( cellToken , DefaultCulture , DefaultDateTimeStyles , out result );
#if NET6_0_OR_GREATER
    public static bool TryParseNullable ( string? cellToken , CultureInfo culture , DateTimeStyles dateTimeStyles , [ NotNullWhen ( true ) ] out DateOnly? result ) => TryParseNullableCore ( cellToken , token => DateOnly.TryParse ( token , culture , dateTimeStyles , out var value ) ? value : null , out result );
    public static bool TryParseNullable ( string? cellToken , CultureInfo culture , [ NotNullWhen ( true ) ] out DateOnly? result ) => TryParseNullable ( cellToken , culture , DefaultDateTimeStyles , out result );
    public static bool TryParseNullable ( string? cellToken , [ NotNullWhen ( true ) ] out DateOnly? result ) => TryParseNullable ( cellToken , DefaultCulture , DefaultDateTimeStyles , out result );
    public static bool TryParseNullable ( string? cellToken , CultureInfo culture , DateTimeStyles dateTimeStyles , [ NotNullWhen ( true ) ] out TimeOnly? result ) => TryParseNullableCore ( cellToken , token => TimeOnly.TryParse ( token , culture , dateTimeStyles , out var value ) ? value : null , out result );
    public static bool TryParseNullable ( string? cellToken , CultureInfo culture , [ NotNullWhen ( true ) ] out TimeOnly? result ) => TryParseNullable ( cellToken , culture , DefaultDateTimeStyles , out result );
    public static bool TryParseNullable ( string? cellToken , [ NotNullWhen ( true ) ] out TimeOnly? result ) => TryParseNullable ( cellToken , DefaultCulture , DefaultDateTimeStyles , out result );
#endif
    #endregion

    #region Special Types
    public static bool TryParseNullable ( string? cellToken , [ NotNullWhen ( true ) ] out Guid? result ) => TryParseNullableCore ( cellToken , token => Guid.TryParse ( token , out var value ) ? value : null , out result );
    public static bool TryParseNullable ( string? cellToken , [ NotNullWhen ( true ) ] out bool? result ) => TryParseNullableCore ( cellToken , token => bool.TryParse ( token , out var value ) ? value : null , out result );
    public static bool TryParseNullable ( string? cellToken , [ NotNullWhen ( true ) ] out char? result ) => TryParseNullableCore ( cellToken , token => char.TryParse ( token , out var value ) ? value : null , out result );
    #endregion

    #region Enum Types
    public static bool TryParseNullableEnum < T > ( string? cellToken , bool ignoreCase , [ NotNullWhen ( true ) ] out T? result ) where T : struct , Enum => TryParseNullableCore ( cellToken , token => Enum.TryParse ( token , ignoreCase , out T value ) ? value : null , out result );
    public static bool TryParseNullableEnum < T > ( string? cellToken , [ NotNullWhen ( true ) ] out T? result ) where T : struct , Enum => TryParseNullableEnum ( cellToken , false , out result );
    #endregion

    #region Core Helper Methods
    [ MethodImpl ( MethodImplOptions.AggressiveInlining ) ]
    private static bool TryParseNullableCore < T > ( string? cellToken , Func< string , T? > parseFunc , [ NotNullWhen ( true ) ] out T? result ) where T : struct
    {
        result = new T();

        if ( IsNullOrEmpty ( cellToken ) ) { return true; }

        result = parseFunc ( cellToken );

        return result.HasValue;
    }

    [ MethodImpl ( MethodImplOptions.AggressiveInlining ) ]
    private static bool IsNullOrEmpty ( [ NotNullWhen ( false ) ] string? value ) { return string.IsNullOrWhiteSpace ( value ); }
    #endregion

    public static string DefaultParseErrorMessage < T > ( string token ) => $"Cannot parse '{token}' as {typeof ( T ).Name}";
    public static ParseResult< T > Succeeded < T > ( T? value ) where T : struct => new ParseResult< T > ( true , value );
    public static ParseResult< T > Failed < T > ( string? errorMessage = null ) where T : struct => new ParseResult< T > ( false , default , errorMessage );
    public static ParseResult< T > ParseFailed < T > ( string token ) where T : struct => Failed< T > ( DefaultParseErrorMessage< T > ( token ) );

    public static ParseResult< T > TryGenericParse < T > ( string token ) where T : struct , INumber< T >
    {
        try { return TryParseNullable< T > ( token , DefaultCulture , out var result ) ? Succeeded< T > ( result ) : Failed< T > ( $"Cannot parse '{token}' as {typeof ( T ).Name}" ); }
        catch { return Failed< T > ( $"Type {typeof ( T ).Name} is not supported by fluent parser" ); }
    }

    public static ParseResult< int > ParseInt32 ( string token ) => int.TryParse ( token , DefaultNumberStyles , DefaultCulture , out var value ) ? Succeeded< int > ( value ) : ParseFailed< int > ( token );
    public static ParseResult< long > ParseInt64 ( string token ) => long.TryParse ( token , DefaultNumberStyles , DefaultCulture , out var value ) ? Succeeded< long > ( value ) : ParseFailed< long > ( token );
    public static ParseResult< short > ParseInt16 ( string token ) => short.TryParse ( token , DefaultNumberStyles , DefaultCulture , out var value ) ? Succeeded< short > ( value ) : ParseFailed< short > ( token );
    public static ParseResult< byte > ParseByte ( string token ) => byte.TryParse ( token , DefaultNumberStyles , DefaultCulture , out var value ) ? Succeeded< byte > ( value ) : ParseFailed< byte > ( token );
    public static ParseResult< sbyte > ParseSByte ( string token ) => sbyte.TryParse ( token , DefaultNumberStyles , DefaultCulture , out var value ) ? Succeeded< sbyte > ( value ) : ParseFailed< sbyte > ( token );
    public static ParseResult< uint > ParseUInt32 ( string token ) => uint.TryParse ( token , DefaultNumberStyles , DefaultCulture , out var value ) ? Succeeded< uint > ( value ) : ParseFailed< uint > ( token );
    public static ParseResult< ulong > ParseUInt64 ( string token ) => ulong.TryParse ( token , DefaultNumberStyles , DefaultCulture , out var value ) ? Succeeded< ulong > ( value ) : ParseFailed< ulong > ( token );
    public static ParseResult< ushort > ParseUInt16 ( string token ) => ushort.TryParse ( token , DefaultNumberStyles , DefaultCulture , out var value ) ? Succeeded< ushort > ( value ) : ParseFailed< ushort > ( token );
    public static ParseResult< float > ParseSingle ( string token ) => float.TryParse ( token , DefaultNumberStyles , DefaultCulture , out var value ) ? Succeeded< float > ( value ) : ParseFailed< float > ( token );
    public static ParseResult< double > ParseDouble ( string token ) => double.TryParse ( token , DefaultNumberStyles , DefaultCulture , out var value ) ? Succeeded< double > ( value ) : ParseFailed< double > ( token );
    public static ParseResult< decimal > ParseDecimal ( string token ) => decimal.TryParse ( token , DefaultNumberStyles , DefaultCulture , out var value ) ? Succeeded< decimal > ( value ) : ParseFailed< decimal > ( token );
    public static ParseResult< DateTime > ParseDateTime ( string token ) => DateTime.TryParse ( token , DefaultCulture , DateTimeStyles.None , out var value ) ? Succeeded< DateTime > ( value ) : ParseFailed< DateTime > ( token );
    public static ParseResult< Guid > ParseGuid ( string token ) => Guid.TryParse ( token , out var value ) ? Succeeded< Guid > ( value ) : ParseFailed< Guid > ( token );
    public static ParseResult< bool > ParseBoolean ( string token ) => bool.TryParse ( token , out var value ) ? Succeeded< bool > ( value ) : ParseFailed< bool > ( token );

    // public static implicit operator bool (ParseResult result) { return result.Success; }
    public static ParserBuilder< T > For < T >() where T : struct , INumber< T > => new ParserBuilder< T >();
    public static ParseResult< T > TryParse < T > ( string? cellToken ) where T : struct , INumber< T > => TryParseNullable< T > ( cellToken , out var result ) ? Succeeded< T > ( result ) : Failed< T > ( $"Cannot parse '{cellToken}' as {typeof ( T ).Name}" );
    public static T? ParseOrNull < T > ( this string? value ) where T : INumber< T > => TryParseNullable< T > ( value , out var result ) ? result : default;
    public static T? ParseOrDefault < T > ( this string? value , T? defaultValue = default ) where T : INumber< T > => TryParseNullable< T > ( value , out var result ) ? result ?? defaultValue : defaultValue;
    public static T ParseOrThrow < T > ( this string? value , string? parameterName = null ) where T : INumber< T > => TryParseNullable< T > ( value , out var result ) && result.HasValue ? result.Value : throw new FormatException ( $"Unable to parse '{value}' as {typeof ( T ).Name}" + ( parameterName != null ? $" for parameter '{parameterName}'" : "" ) );
}

public static class CsvUniversalParser
{ }

public readonly record struct ParseResult < T > ( bool Success , T? Value , string? ErrorMessage = null ) where T : struct
{
    public TResult Match < TResult > ( Func< T? , TResult > onSuccess , Func< string? , TResult > onFailure ) => Success ? onSuccess ( Value ) : onFailure ( ErrorMessage );
}

public sealed class ParserBuilder < T > where T : struct , INumber< T >
{
    private string[]? _nullValues;
    private Func< string , string >? _preprocessor;

    public ParserBuilder< T > WithNullValues ( params string[] nullValues )
    {
        _nullValues = nullValues;

        return this;
    }

    public ParserBuilder< T > WithPreprocessor ( Func< string , string > preprocessor )
    {
        _preprocessor = preprocessor ?? throw new ArgumentNullException ( nameof ( preprocessor ) );

        return this;
    }

    public ParseResult< T > Parse ( string? cellToken )
    {
        if ( IsConsideredNull ( cellToken ) ) { return ParseResult< T >.Succeeded ( null ); }

        var processedToken = _preprocessor?.Invoke ( cellToken ?? string.Empty ) ?? cellToken ?? string.Empty;

        return typeof ( T ) switch
        {
            int => ParseInt32< int > ( processedToken ) ,
            long => ParseInt64< long > ( processedToken ) ,
            short => ParseInt16< short > ( processedToken ) ,
            byte => ParseByte< byte > ( processedToken ) ,
            sbyte => ParseSByte< sbyte > ( processedToken ) ,
            uint => ParseUInt32< uint > ( processedToken ) ,
            ulong => ParseUInt64< ulong > ( processedToken ) ,
            ushort => ParseUInt16< ushort > ( processedToken ) ,
            float => ParseSingle< float > ( processedToken ) ,
            double => ParseDouble< double > ( processedToken ) ,
            decimal => ParseDecimal< decimal > ( processedToken ) ,
            DateTime => ParseDateTime< DateTime > ( processedToken ) ,
            Guid => ParseGuid< Guid > ( processedToken ) ,
            bool => ParseBoolean< bool > ( processedToken ) ,
            _ => TryGenericParse< T > ( processedToken )
        };
    }

    private bool IsConsideredNull ( string? value ) => string.IsNullOrWhiteSpace ( value ) || _nullValues != null && _nullValues.Contains ( value , StringComparer.OrdinalIgnoreCase );
}

public static class CsvAdvancedParser
{ }
