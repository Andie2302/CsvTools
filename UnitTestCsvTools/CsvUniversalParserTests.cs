using System.Globalization;
using CsvTools.Parsers;
using Xunit;

namespace UnitTestCsvTools;

/// <summary>
/// Comprehensive test suite for CsvUniversalParser functionality
/// </summary>
public class CsvUniversalParserTests
{
    private static readonly CultureInfo GermanCulture = CultureInfo.GetCultureInfo("de-DE");
    private static readonly CultureInfo EnglishCulture = CultureInfo.GetCultureInfo("en-US");
    private static readonly CultureInfo InvariantCulture = CultureInfo.InvariantCulture;

    #region Numeric Types Tests

    [Theory]
    [InlineData("123", 123)]
    [InlineData("0", 0)]
    [InlineData("-456", -456)]
    [InlineData("2147483647", 2147483647)] // Int32.MaxValue
    [InlineData("-2147483648", -2147483648)] // Int32.MinValue
    public void TryParseNullable_Int32_ValidValues_ShouldSucceed(string input, int expected)
    {
        // Act
        var result = CsvUniversalParser.TryParseNullable<int>(input, out var actual);

        // Assert
        Assert.True(result);
        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData("1.234", 1234)] // German culture - dot as thousand separator
    [InlineData("1,234", 1234)] // English culture - comma as thousand separator
    public void TryParseNullable_Int32_WithCulture_ShouldSucceed(string input, int expected)
    {
        // Act - German culture
        var resultGerman = CsvUniversalParser.TryParseNullable<int>(input, GermanCulture, out var actualGerman);
        // Act - English culture  
        var resultEnglish = CsvUniversalParser.TryParseNullable<int>(input, EnglishCulture, out var actualEnglish);

        // Assert
        if (input.Contains('.'))
        {
            Assert.True(resultGerman);
            Assert.Equal(expected, actualGerman);
        }
        else
        {
            Assert.True(resultEnglish);
            Assert.Equal(expected, actualEnglish);
        }
    }

    [Theory]
    [InlineData("123.45", 123.45)]
    [InlineData("0.0", 0.0)]
    [InlineData("-456.789", -456.789)]
    [InlineData("1.7976931348623157E+308", double.MaxValue)]
    [InlineData("-1.7976931348623157E+308", double.MinValue)]
    public void TryParseNullable_Double_ValidValues_ShouldSucceed(string input, double expected)
    {
        // Act
        var result = CsvUniversalParser.TryParseNullable<double>(input, EnglishCulture, out var actual);

        // Assert
        Assert.True(result);
        Assert.Equal(expected, actual.GetValueOrDefault(), precision: 10);
    }

    [Theory]
    [InlineData("1.234,56", 1234.56)] // German format
    [InlineData("1,234.56", 1234.56)] // English format
    [InlineData("0,5", 0.5)] // German decimal
    [InlineData("0.5", 0.5)] // English decimal
    public void TryParseNullable_Double_CultureSpecific_ShouldSucceed(string input, double expected)
    {
        // Act
        var culture = input.Contains(',') && input.IndexOf(',') > input.LastIndexOf('.') ? GermanCulture : EnglishCulture;
        var result = CsvUniversalParser.TryParseNullable<double>(input, culture, out var actual);

        // Assert
        Assert.True(result);
        Assert.Equal(expected, actual.GetValueOrDefault(), precision: 2);
    }

    [Theory]
    [InlineData("123.45", "123.45")]
    [InlineData("0.0", "0.0")]
    [InlineData("999999999999999999999999999.99", "999999999999999999999999999.99")]
    public void TryParseNullable_Decimal_ValidValues_ShouldSucceed(string input, string expectedStr)
    {
        // Arrange
        var expected = decimal.Parse(expectedStr, InvariantCulture);

        // Act
        var result = CsvUniversalParser.TryParseNullable<decimal>(input, InvariantCulture, out var actual);

        // Assert
        Assert.True(result);
        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData("123.45", "123.45")]
    [InlineData("0.0", "0.0")]
    [InlineData("3.40282347E+38", "3.40282347E+38")] // float.MaxValue
    public void TryParseNullable_Float_ValidValues_ShouldSucceed(string input, string expectedStr)
    {
        // Arrange
        var expected = float.Parse(expectedStr, InvariantCulture);

        // Act
        var result = CsvUniversalParser.TryParseNullable<float>(input, InvariantCulture, out var actual);

        // Assert
        Assert.True(result);
        Assert.Equal(expected, actual.GetValueOrDefault(), precision: 5);
    }

    [Theory]
    [InlineData("9223372036854775807", 9223372036854775807L)] // long.MaxValue
    [InlineData("-9223372036854775808", -9223372036854775808L)] // long.MinValue
    [InlineData("0", 0L)]
    public void TryParseNullable_Int64_ValidValues_ShouldSucceed(string input, long expected)
    {
        // Act
        var result = CsvUniversalParser.TryParseNullable<long>(input, out var actual);

        // Assert
        Assert.True(result);
        Assert.Equal(expected, actual);
    }

    #endregion

    #region Null and Empty Values Tests

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData("\t")]
    [InlineData("\n")]
    public void TryParseNullable_EmptyInput_ShouldReturnNullSuccessfully(string input)
    {
        // Act
        var result = CsvUniversalParser.TryParseNullable<int>(input, out var actual);

        // Assert
        Assert.True(result);
        Assert.Null(actual);
    }

    [Fact]
    public void TryParseNullable_NullInput_ShouldReturnNullSuccessfully()
    {
        // Act
        var result = CsvUniversalParser.TryParseNullable<int>(null, out var actual);

        // Assert
        Assert.True(result);
        Assert.Null(actual);
    }

    #endregion

    #region Invalid Values Tests

    [Theory]
    [InlineData("abc")]
    [InlineData("123abc")]
    [InlineData("12.34.56")]
    [InlineData("++123")]
    [InlineData("123-")]
    public void TryParseNullable_InvalidInput_ShouldFail(string input)
    {
        // Act
        var result = CsvUniversalParser.TryParseNullable<int>(input, out var actual);

        // Assert
        Assert.False(result);
        Assert.Null(actual);
    }

    [Fact]
    public void TryParseNullable_ComplexInvalidInput_ShouldFail()
    {
        // Test some edge cases that might be parsed differently
        var testCases = new[] { "12.34.56", "++123", "123-", "abc123def" };
        
        foreach (var testCase in testCases)
        {
            var result = CsvUniversalParser.TryParseNullable<int>(testCase,CultureInfo.CurrentCulture, NumberStyles.Integer|NumberStyles.Float , out var actual);
            Assert.False(result, $"Input '{testCase}' should fail parsing but succeeded");
            Assert.Null(actual);
        }
    }

    [Theory]
    [InlineData("2147483648")] // Int32.MaxValue + 1
    [InlineData("-2147483649")] // Int32.MinValue - 1
    public void TryParseNullable_Int32_OutOfRange_ShouldFail(string input)
    {
        // Act
        var result = CsvUniversalParser.TryParseNullable<int>(input, out var actual);

        // Assert
        Assert.False(result);
        Assert.Null(actual);
    }

    #endregion

    #region DateTime Tests

    [Theory]
    [InlineData("2023-12-25", "2023-12-25")]
    [InlineData("12/25/2023", "2023-12-25")]
    [InlineData("25.12.2023", "2023-12-25")]
    public void TryParseNullable_DateTime_ValidValues_ShouldSucceed(string input, string expectedStr)
    {
        // Arrange
        var expected = DateTime.Parse(expectedStr, InvariantCulture);
        var culture = input.Contains('.') ? GermanCulture : EnglishCulture;

        // Act
        var result = CsvUniversalParser.TryParseNullable(input, culture, out DateTime? actual);

        // Assert
        Assert.True(result);
        Assert.NotNull(actual);
        Assert.Equal(expected.Date, actual.Value.Date);
    }

    #endregion

    #region Special Types Tests

    [Theory]
    [InlineData("true", true)]
    [InlineData("false", false)]
    [InlineData("True", true)]
    [InlineData("False", false)]
    [InlineData("TRUE", true)]
    [InlineData("FALSE", false)]
    public void TryParseNullable_Boolean_ValidValues_ShouldSucceed(string input, bool expected)
    {
        // Act
        var result = CsvUniversalParser.TryParseNullable(input, out bool? actual);

        // Assert
        Assert.True(result);
        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData("6ba7b810-9dad-11d1-80b4-00c04fd430c8")]
    [InlineData("6BA7B810-9DAD-11D1-80B4-00C04FD430C8")]
    [InlineData("{6ba7b810-9dad-11d1-80b4-00c04fd430c8}")]
    public void TryParseNullable_Guid_ValidValues_ShouldSucceed(string input)
    {
        // Arrange
        var expected = Guid.Parse("6ba7b810-9dad-11d1-80b4-00c04fd430c8");

        // Act
        var result = CsvUniversalParser.TryParseNullable(input, out Guid? actual);

        // Assert
        Assert.True(result);
        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData("A", 'A')]
    [InlineData("z", 'z')]
    [InlineData("5", '5')]
    public void TryParseNullable_Char_ValidValues_ShouldSucceed(string input, char expected)
    {
        // Act
        var result = CsvUniversalParser.TryParseNullable(input, out char? actual);

        // Assert
        Assert.True(result);
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void TryParseNullable_Char_WhitespaceInput_ShouldReturnNull()
    {
        // Act - Whitespace is considered null/empty by the parser
        var result = CsvUniversalParser.TryParseNullable(" ", out char? actual);

        // Assert
        Assert.True(result);
        Assert.Null(actual); // Whitespace should be treated as null, not as ' ' character
    }

    #endregion

    #region Enum Tests

    public enum TestEnum
    {
        Value1,
        Value2,
        CamelCaseValue
    }

    [Theory]
    [InlineData("Value1", TestEnum.Value1)]
    [InlineData("Value2", TestEnum.Value2)]
    [InlineData("CamelCaseValue", TestEnum.CamelCaseValue)]
    public void TryParseNullableEnum_ValidValues_ShouldSucceed(string input, TestEnum expected)
    {
        // Act
        var result = CsvUniversalParser.TryParseNullableEnum<TestEnum>(input, out var actual);

        // Assert
        Assert.True(result);
        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData("value1", TestEnum.Value1)]
    [InlineData("VALUE2", TestEnum.Value2)]
    [InlineData("camelcasevalue", TestEnum.CamelCaseValue)]
    public void TryParseNullableEnum_IgnoreCase_ShouldSucceed(string input, TestEnum expected)
    {
        // Act
        var result = CsvUniversalParser.TryParseNullableEnum<TestEnum>(input, ignoreCase: true, out var actual);

        // Assert
        Assert.True(result);
        Assert.Equal(expected, actual);
    }

    #endregion
}

/// <summary>
/// Test suite for the advanced fluent parser API
/// </summary>
public class CsvAdvancedParserTests
{
    #region ParseResult Tests

    [Fact]
    public void ParseResult_Succeeded_ShouldHaveCorrectProperties()
    {
        // Arrange & Act
        var result = CsvAdvancedParser.ParseResult<int>.Succeeded(42);

        // Assert
        Assert.True(result.Success);
        Assert.Equal(42, result.Value);
        Assert.Null(result.ErrorMessage);
        Assert.True(result); // Implicit bool conversion
    }

    [Fact]
    public void ParseResult_Failed_ShouldHaveCorrectProperties()
    {
        // Arrange & Act
        var result = CsvAdvancedParser.ParseResult<int>.Failed("Test error");

        // Assert
        Assert.False(result.Success);
        Assert.Null(result.Value);
        Assert.Equal("Test error", result.ErrorMessage);
        Assert.False(result); // Implicit bool conversion
    }

    [Fact]
    public void ParseResult_Match_ShouldExecuteCorrectFunction()
    {
        // Arrange
        var successResult = CsvAdvancedParser.ParseResult<int>.Succeeded(42);
        var failureResult = CsvAdvancedParser.ParseResult<int>.Failed("Error");

        // Act & Assert
        var successValue = successResult.Match(
            onSuccess: val => $"Success: {val}",
            onFailure: err => $"Failure: {err}"
        );
        Assert.Equal("Success: 42", successValue);

        var failureValue = failureResult.Match(
            onSuccess: val => $"Success: {val}",
            onFailure: err => $"Failure: {err}"
        );
        Assert.Equal("Failure: Error", failureValue);
    }

    #endregion

    #region Fluent API Tests

    [Fact]
    public void FluentParser_BasicNumericTypes_ShouldSucceed()
    {
        // Test Int32
        var intResult = CsvAdvancedParser.For<int>().Parse("123");
        Assert.True(intResult.Success);
        Assert.Equal(123, intResult.Value);

        // Test Double
        var doubleResult = CsvAdvancedParser.For<double>().Parse("123.45");
        Assert.True(doubleResult.Success);
        Assert.Equal(123.45, doubleResult.Value);

        // Test Decimal
        var decimalResult = CsvAdvancedParser.For<decimal>().Parse("123.45");
        Assert.True(decimalResult.Success);
        Assert.Equal(123.45m, decimalResult.Value);
    }

    [Fact]
    public void FluentParser_WithCulture_GermanNumbers_ShouldSucceed()
    {
        // Arrange
        var parser = CsvAdvancedParser.For<double>()
            .WithCulture(CultureInfo.GetCultureInfo("de-DE"));

        // Act
        var result = parser.Parse("1.234,56");

        // Assert
        Assert.True(result.Success);
        Assert.Equal(1234.56, result.Value);
    }

    [Fact]
    public void FluentParser_WithPreprocessor_ShouldRemoveCurrencySymbol()
    {
        // Arrange
        var parser = CsvAdvancedParser.For<double>()
            .WithCulture(CultureInfo.GetCultureInfo("de-DE"))
            .WithPreprocessor(s => s.Replace("€", ""));

        // Act
        var result = parser.Parse("1.234,56€");

        // Assert
        Assert.True(result.Success);
        Assert.Equal(1234.56, result.Value);
    }

    [Theory]
    [InlineData("N/A")]
    [InlineData("NULL")]
    [InlineData("-")]
    [InlineData("")]
    [InlineData("   ")]
    public void FluentParser_WithNullValues_ShouldReturnNull(string input)
    {
        // Arrange
        var parser = CsvAdvancedParser.For<double>()
            .WithNullValues("N/A", "NULL", "-");

        // Act
        var result = parser.Parse(input);

        // Assert
        Assert.True(result.Success);
        Assert.Null(result.Value);
    }

    [Fact]
    public void FluentParser_ComplexScenario_ShouldHandleAllFeatures()
    {
        // Arrange
        var parser = CsvAdvancedParser.For<decimal>()
            .WithCulture(CultureInfo.GetCultureInfo("en-US"))
            .WithNullValues("N/A", "NULL", "n/a")
            .WithPreprocessor(s => s.Replace("$", "").Replace(",", ""));

        // Act & Assert
        var result1 = parser.Parse("$1,234.56");
        Assert.True(result1.Success);
        Assert.Equal(1234.56m, result1.Value);

        var result2 = parser.Parse("N/A");
        Assert.True(result2.Success);
        Assert.Null(result2.Value);

        var result3 = parser.Parse("$999,999.99");
        Assert.True(result3.Success);
        Assert.Equal(999999.99m, result3.Value);
    }

    [Fact]
    public void FluentParser_InvalidInput_ShouldFail()
    {
        // Arrange
        var parser = CsvAdvancedParser.For<int>();

        // Act
        var result = parser.Parse("invalid");

        // Assert
        Assert.False(result.Success);
        Assert.Null(result.Value);
        Assert.Contains("Cannot parse", result.ErrorMessage);
    }

    // Removed tests for unsupported types (DateTime, Guid, Boolean)
    // as these don't implement INumber<T>

    #endregion

    #region Quick Parse Tests

    [Fact]
    public void TryParse_QuickAPI_ShouldWork()
    {
        // Act
        var result = CsvAdvancedParser.TryParse<int>("123");

        // Assert
        Assert.True(result.Success);
        Assert.Equal(123, result.Value);
    }

    #endregion

    #region Edge Cases

    [Theory]
    [InlineData("0")]
    [InlineData("0.0")]
    [InlineData("0,0")]
    public void FluentParser_ZeroValues_ShouldSucceed(string input)
    {
        // Arrange
        var parser = CsvAdvancedParser.For<double>()
            .WithCulture(input.Contains(',') ? 
                CultureInfo.GetCultureInfo("de-DE") : 
                CultureInfo.GetCultureInfo("en-US"));

        // Act
        var result = parser.Parse(input);

        // Assert
        Assert.True(result.Success);
        Assert.Equal(0.0, result.Value);
    }

    [Theory]
    [InlineData("123.456789")]
    [InlineData("123,456789")]
    public void FluentParser_HighPrecision_ShouldMaintainPrecision(string input)
    {
        // Arrange
        var parser = CsvAdvancedParser.For<decimal>()
            .WithCulture(input.Contains(',') ? 
                CultureInfo.GetCultureInfo("de-DE") : 
                CultureInfo.GetCultureInfo("en-US"));

        // Act
        var result = parser.Parse(input);

        // Assert
        Assert.True(result.Success);
        Assert.Equal(123.456789m, result.Value);
    }

    #endregion

    #region Real World Scenarios

    [Fact]
    public void FluentParser_CurrencyValues_ShouldHandleCorrectly()
    {
        // EUR with German culture
        var eurParser = CsvAdvancedParser.For<decimal>()
            .WithCulture(CultureInfo.GetCultureInfo("de-DE"))
            .WithPreprocessor(s => s.Replace("€", "").Trim());

        var eurResult = eurParser.Parse("1.234,56 €");
        Assert.True(eurResult.Success);
        Assert.Equal(1234.56m, eurResult.Value);

        // USD with US culture
        var usdParser = CsvAdvancedParser.For<decimal>()
            .WithCulture(CultureInfo.GetCultureInfo("en-US"))
            .WithPreprocessor(s => s.Replace("$", ""));

        var usdResult = usdParser.Parse("$1,234.56");
        Assert.True(usdResult.Success);
        Assert.Equal(1234.56m, usdResult.Value);
    }

    [Fact]
    public void FluentParser_PercentageValues_ShouldWork()
    {
        // Arrange
        var parser = CsvAdvancedParser.For<double>()
            .WithCulture(CultureInfo.InvariantCulture)
            .WithPreprocessor(s => s.Replace("%", ""));

        // Act
        var result = parser.Parse("15.5%");

        // Assert
        Assert.True(result.Success);
        Assert.Equal(15.5, result.Value);
    }

    #endregion
}

/// <summary>
/// Test suite for extension methods
/// </summary>
public class CsvParserExtensionsTests
{
    [Theory]
    [InlineData("123", 123)]
    [InlineData("", null)]
    [InlineData("invalid", null)]
    public void ParseOrNull_ShouldReturnExpectedValue(string input, int? expected)
    {
        // Act
        var result = input.ParseOrNull<int>();

        // Assert
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData("123", 123)]
    [InlineData("", 999)]
    [InlineData("invalid", 999)]
    public void ParseOrDefault_ShouldReturnExpectedValue(string input, int expected)
    {
        // Act
        var result = input.ParseOrDefault<int>(999);

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void ParseOrThrow_ValidInput_ShouldReturnValue()
    {
        // Act
        var result = "123".ParseOrThrow<int>();

        // Assert
        Assert.Equal(123, result);
    }

    [Fact]
    public void ParseOrThrow_InvalidInput_ShouldThrowException()
    {
        // Act & Assert
        var exception = Assert.Throws<FormatException>(() => "invalid".ParseOrThrow<int>("testParam"));
        Assert.Contains("Unable to parse", exception.Message);
        Assert.Contains("testParam", exception.Message);
    }

    [Fact]
    public void ParseOrThrow_NullInput_ShouldThrowException()
    {
        // Act & Assert
        var exception = Assert.Throws<FormatException>(() => ((string?)null).ParseOrThrow<int>());
        Assert.Contains("Unable to parse", exception.Message);
    }
}

/// <summary>
/// Performance and stress tests
/// </summary>
public class CsvParserPerformanceTests
{
    [Fact]
    public void Parser_LargeNumberOfOperations_ShouldPerformWell()
    {
        // Arrange
        const int iterations = 10000;
        var parser = CsvAdvancedParser.For<double>()
            .WithCulture(CultureInfo.GetCultureInfo("en-US"));

        // Act
        var stopwatch = System.Diagnostics.Stopwatch.StartNew();
        
        for (int i = 0; i < iterations; i++)
        {
            var result = parser.Parse($"{i}.{i % 100:00}");
            Assert.True(result.Success);
        }
        
        stopwatch.Stop();

        // Assert - Should complete in reasonable time (less than 1 second)
        Assert.True(stopwatch.ElapsedMilliseconds < 1000, 
            $"Performance test took {stopwatch.ElapsedMilliseconds}ms for {iterations} operations");
    }

    [Theory]
    [InlineData(1000)]
    [InlineData(10000)]
    public void UniversalParser_RepeatedCalls_ShouldBeFast(int iterations)
    {
        // Arrange
        var stopwatch = System.Diagnostics.Stopwatch.StartNew();

        // Act
        for (int i = 0; i < iterations; i++)
        {
            var success = CsvUniversalParser.TryParseNullable<int>(i.ToString(), out var result);
            Assert.True(success);
            Assert.Equal(i, result);
        }

        stopwatch.Stop();

        // Assert - Performance should be reasonable
        Assert.True(stopwatch.ElapsedMilliseconds < 500, 
            $"Universal parser took {stopwatch.ElapsedMilliseconds}ms for {iterations} operations");
    }
}

/// <summary>
/// Integration tests that verify the original problem from the GitHub issue
/// </summary>
public class CsvParserOriginalProblemTests
{
    [Fact]
    public void OriginalProblem_GermanCurrencyParsing_ShouldWork()
    {
        // This test reproduces the original problem: "1.234,56€" → 1234.56
        var result = CsvAdvancedParser.For<double>()
            .WithCulture(CultureInfo.GetCultureInfo("de-DE"))
            .WithNullValues("N/A", "NULL", "-")
            .WithPreprocessor(s => s.Replace("€", ""))
            .Parse("1.234,56€");

        Assert.True(result.Success);
        Assert.Equal(1234.56, result.Value);
        Assert.Null(result.ErrorMessage);
    }

    [Theory]
    [InlineData("1.234,56€", 1234.56)]
    [InlineData("123,45€", 123.45)]
    [InlineData("0,50€", 0.50)]
    [InlineData("-1.000,00€", -1000.00)]
    public void OriginalProblem_VariousGermanCurrencies_ShouldWork(string input, double expected)
    {
        var parser = CsvAdvancedParser.For<double>()
            .WithCulture(CultureInfo.GetCultureInfo("de-DE"))
            .WithPreprocessor(s => s.Replace("€", ""));

        var result = parser.Parse(input);

        Assert.True(result.Success);
        Assert.Equal(expected, result.Value);
    }
}