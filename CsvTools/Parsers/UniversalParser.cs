using System.Globalization;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Diagnostics.CodeAnalysis;

namespace CsvTools.Parsers;

/// <summary>
/// Provides universal parsing capabilities for CSV cell values with strong type safety and null handling.
/// </summary>
public static class CsvUniversalParser
{
    private static readonly CultureInfo DefaultCulture = CultureInfo.InvariantCulture;
    private static readonly NumberStyles DefaultNumberStyles = NumberStyles.Any;
    private static readonly DateTimeStyles DefaultDateTimeStyles = DateTimeStyles.None;

    #region Numeric Types (INumber<T>)
    /// <summary>
    /// Attempts to parse a nullable numeric value with full control over culture and number styles.
    /// </summary>
    [ MethodImpl ( MethodImplOptions.AggressiveInlining ) ]
    public static bool TryParseNullable < T > ( string? cellToken , CultureInfo culture , NumberStyles numberStyles , [ NotNullWhen ( true ) ] out T? result ) where T : struct , INumber< T >
    {
        result = null;

        if ( IsNullOrEmpty ( cellToken ) ) {
            return true;
        }

        if ( !T.TryParse ( cellToken , numberStyles , culture , out var value ) ) {
            return false;
        }

        result = value;

        return true;
    }

    /// <summary>
    /// Attempts to parse a nullable numeric value using default number styles.
    /// </summary>
    [ MethodImpl ( MethodImplOptions.AggressiveInlining ) ]
    public static bool TryParseNullable < T > ( string? cellToken , CultureInfo culture , [ NotNullWhen ( true ) ] out T? result ) where T : struct , INumber< T > => TryParseNullable ( cellToken , culture , DefaultNumberStyles , out result );

    /// <summary>
    /// Attempts to parse a nullable numeric value using invariant culture.
    /// </summary>
    [ MethodImpl ( MethodImplOptions.AggressiveInlining ) ]
    public static bool TryParseNullable < T > ( string? cellToken , [ NotNullWhen ( true ) ] out T? result ) where T : struct , INumber< T > => TryParseNullable ( cellToken , DefaultCulture , DefaultNumberStyles , out result );
    #endregion

    #region DateTime Types
    /// <summary>
    /// Attempts to parse a nullable DateTime with culture and style control.
    /// </summary>
    public static bool TryParseNullable ( string? cellToken , CultureInfo culture , DateTimeStyles dateTimeStyles , [ NotNullWhen ( true ) ] out DateTime? result ) => TryParseNullableCore ( cellToken , token => DateTime.TryParse ( token , culture , dateTimeStyles , out var value ) ? value : null , out result );

    /// <summary>
    /// Attempts to parse a nullable DateTime using default styles.
    /// </summary>
    public static bool TryParseNullable ( string? cellToken , CultureInfo culture , [ NotNullWhen ( true ) ] out DateTime? result ) => TryParseNullable ( cellToken , culture , DefaultDateTimeStyles , out result );

    /// <summary>
    /// Attempts to parse a nullable DateTime using invariant culture.
    /// </summary>
    public static bool TryParseNullable ( string? cellToken , [ NotNullWhen ( true ) ] out DateTime? result ) => TryParseNullable ( cellToken , DefaultCulture , DefaultDateTimeStyles , out result );

#if NET6_0_OR_GREATER
    /// <summary>
    /// Attempts to parse a nullable DateOnly with culture and style control.
    /// </summary>
    public static bool TryParseNullable ( string? cellToken , CultureInfo culture , DateTimeStyles dateTimeStyles , [ NotNullWhen ( true ) ] out DateOnly? result ) => TryParseNullableCore ( cellToken , token => DateOnly.TryParse ( token , culture , dateTimeStyles , out var value ) ? value : null , out result );

    /// <summary>
    /// Attempts to parse a nullable DateOnly using default styles.
    /// </summary>
    public static bool TryParseNullable ( string? cellToken , CultureInfo culture , [ NotNullWhen ( true ) ] out DateOnly? result ) => TryParseNullable ( cellToken , culture , DefaultDateTimeStyles , out result );

    /// <summary>
    /// Attempts to parse a nullable DateOnly using invariant culture.
    /// </summary>
    public static bool TryParseNullable ( string? cellToken , [ NotNullWhen ( true ) ] out DateOnly? result ) => TryParseNullable ( cellToken , DefaultCulture , DefaultDateTimeStyles , out result );

    /// <summary>
    /// Attempts to parse a nullable TimeOnly with culture and style control.
    /// </summary>
    public static bool TryParseNullable ( string? cellToken , CultureInfo culture , DateTimeStyles dateTimeStyles , [ NotNullWhen ( true ) ] out TimeOnly? result ) => TryParseNullableCore ( cellToken , token => TimeOnly.TryParse ( token , culture , dateTimeStyles , out var value ) ? value : null , out result );

    /// <summary>
    /// Attempts to parse a nullable TimeOnly using default styles.
    /// </summary>
    public static bool TryParseNullable ( string? cellToken , CultureInfo culture , [ NotNullWhen ( true ) ] out TimeOnly? result ) => TryParseNullable ( cellToken , culture , DefaultDateTimeStyles , out result );

    /// <summary>
    /// Attempts to parse a nullable TimeOnly using invariant culture.
    /// </summary>
    public static bool TryParseNullable ( string? cellToken , [ NotNullWhen ( true ) ] out TimeOnly? result ) => TryParseNullable ( cellToken , DefaultCulture , DefaultDateTimeStyles , out result );
#endif
    #endregion

    #region Special Types
    /// <summary>
    /// Attempts to parse a nullable Guid.
    /// </summary>
    public static bool TryParseNullable ( string? cellToken , [ NotNullWhen ( true ) ] out Guid? result ) => TryParseNullableCore ( cellToken , token => Guid.TryParse ( token , out var value ) ? value : null , out result );

    /// <summary>
    /// Attempts to parse a nullable boolean value.
    /// </summary>
    public static bool TryParseNullable ( string? cellToken , [ NotNullWhen ( true ) ] out bool? result ) => TryParseNullableCore ( cellToken , token => bool.TryParse ( token , out var value ) ? value : null , out result );

    /// <summary>
    /// Attempts to parse a nullable character.
    /// </summary>
    public static bool TryParseNullable ( string? cellToken , [ NotNullWhen ( true ) ] out char? result ) => TryParseNullableCore ( cellToken , token => char.TryParse ( token , out var value ) ? value : null , out result );
    #endregion

    #region Enum Types
    /// <summary>
    /// Attempts to parse a nullable enum with case sensitivity control.
    /// </summary>
    public static bool TryParseNullableEnum < T > ( string? cellToken , bool ignoreCase , [ NotNullWhen ( true ) ] out T? result ) where T : struct , Enum => TryParseNullableCore ( cellToken , token => Enum.TryParse ( token , ignoreCase , out T value ) ? value : null , out result );

    /// <summary>
    /// Attempts to parse a nullable enum with case sensitivity.
    /// </summary>
    public static bool TryParseNullableEnum < T > ( string? cellToken , [ NotNullWhen ( true ) ] out T? result ) where T : struct , Enum => TryParseNullableEnum ( cellToken , ignoreCase : false , out result );
    #endregion

    #region Core Helper Methods
    /// <summary>
    /// Core parsing logic that handles null/empty validation and delegates to specific parsers.
    /// </summary>
    [ MethodImpl ( MethodImplOptions.AggressiveInlining ) ]
    private static bool TryParseNullableCore < T > ( string? cellToken , Func< string , T? > parseFunc , [ NotNullWhen ( true ) ] out T? result ) where T : struct
    {
        result = null;

        if ( IsNullOrEmpty ( cellToken ) ) {
            return true;
        }

        result = parseFunc ( cellToken );

        return result.HasValue;
    }

    /// <summary>
    /// Optimized null/empty check that handles common CSV empty representations.
    /// </summary>
    [ MethodImpl ( MethodImplOptions.AggressiveInlining ) ]
    private static bool IsNullOrEmpty ( [ NotNullWhen ( false ) ] string? value ) => string.IsNullOrWhiteSpace ( value );
    #endregion
}

/// <summary>
/// Advanced CSV parser with fluent API and result pattern.
/// </summary>
public static class CsvAdvancedParser
{
    /// <summary>
    /// Represents the result of a parsing operation.
    /// </summary>
    /// <typeparam name="T">The target type.</typeparam>
    public readonly record struct ParseResult < T > ( bool Success , T? Value , string? ErrorMessage = null ) where T : struct
    {
        public static implicit operator bool ( ParseResult< T > result ) => result.Success;
        public static ParseResult< T > Succeeded ( T? value ) => new( true , value );
        public static ParseResult< T > Failed ( string? errorMessage = null ) => new( false , default , errorMessage );
        public TResult Match < TResult > ( Func< T? , TResult > onSuccess , Func< string? , TResult > onFailure ) => Success ? onSuccess ( Value ) : onFailure ( ErrorMessage );
    }

    /// <summary>
    /// Fluent parser builder for complex parsing scenarios.
    /// </summary>
    /// <typeparam name="T">The target type.</typeparam>
    public sealed class ParserBuilder < T > where T : struct
    {
        private CultureInfo _culture = CultureInfo.InvariantCulture;
        private string[]? _nullValues;
        private Func< string , string >? _preprocessor;

        public ParserBuilder< T > WithCulture ( CultureInfo culture )
        {
            _culture = culture ?? throw new ArgumentNullException ( nameof ( culture ) );

            return this;
        }

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
            if ( IsConsideredNull ( cellToken ) ) {
                return ParseResult< T >.Succeeded ( null );
            }

            var processedToken = _preprocessor?.Invoke ( cellToken! ) ?? cellToken!;

            if ( typeof ( T ) == typeof ( int ) ) {
                var success = int.TryParse ( processedToken , NumberStyles.Any , _culture , out var intValue );

                return success ? ParseResult< T >.Succeeded ( (T?) (object?) intValue ) : ParseResult< T >.Failed ( $"Cannot parse '{processedToken}' as {typeof ( T ).Name}" );
            }

            // Weitere Typen können hier hinzugefügt werden
            return ParseResult< T >.Failed ( $"Type {typeof ( T ).Name} is not supported by fluent parser" );
        }

        private bool IsConsideredNull ( string? value )
        {
            if ( string.IsNullOrWhiteSpace ( value ) ) {
                return true;
            }

            if ( _nullValues != null ) {
                return _nullValues.Contains ( value , StringComparer.OrdinalIgnoreCase );
            }

            return false;
        }
    }

    /// <summary>
    /// Creates a fluent parser builder for the specified type.
    /// </summary>
    public static ParserBuilder< T > For < T >() where T : struct => new();

    /// <summary>
    /// Quick parse with result pattern.
    /// </summary>
    public static ParseResult< T > TryParse < T > ( string? cellToken ) where T : struct , INumber< T >
    {
        if ( CsvUniversalParser.TryParseNullable< T > ( cellToken , out var result ) ) {
            return ParseResult< T >.Succeeded ( result );
        }

        return ParseResult< T >.Failed ( $"Cannot parse '{cellToken}' as {typeof ( T ).Name}" );
    }
}

/// <summary>
/// Extension methods for enhanced usability.
/// </summary>
public static class CsvParserExtensions
{
    /// <summary>
    /// Tries to parse the string as the specified nullable type.
    /// </summary>
    public static T? ParseOrNull < T > ( this string? value ) where T : struct , INumber< T > => CsvUniversalParser.TryParseNullable< T > ( value , out var result ) ? result : null;

    /// <summary>
    /// Tries to parse the string as the specified type with a fallback value.
    /// </summary>
    public static T ParseOrDefault < T > ( this string? value , T defaultValue = default ) where T : struct , INumber< T > => CsvUniversalParser.TryParseNullable< T > ( value , out var result ) ? result ?? defaultValue : defaultValue;

    /// <summary>
    /// Parses the string and throws an exception if parsing fails.
    /// </summary>
    public static T ParseOrThrow < T > ( this string? value , string? parameterName = null ) where T : struct , INumber< T >
    {
        if ( CsvUniversalParser.TryParseNullable< T > ( value , out var result ) && result.HasValue ) {
            return result.Value;
        }

        throw new FormatException ( $"Unable to parse '{value}' as {typeof ( T ).Name}" + ( parameterName != null ? $" for parameter '{parameterName}'" : "" ) );
    }
}
