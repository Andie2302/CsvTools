using CsvTools.Values;
using CsvTools.Values.Extensions;

namespace UnitTestCsvTools.Tests;

public class CsvValueExtensionsFluentApiTests
{
    [Fact]
    public void WithNewValue_SetsNewCurrentValue()
    {
        var originalValue = new CsvValue<int>("123", 123);
        var newValue = 456;
        var result = originalValue.WithNewValue(newValue);
        Assert.Equal(123, result.OriginalValue);
        Assert.Equal(newValue, result.CurrentValue);
        Assert.Equal("123", result.OriginalString);
        Assert.True(result.IsModified);
    }

    [Fact]
    public void WithNewValue_WithNull_SetsCurrentValueToNull()
    {
        var originalValue = new CsvValue<string?>("test", "test");
        var result = originalValue.WithNewValue(null);
        Assert.Equal("test", result.OriginalValue);
        Assert.Null(result.CurrentValue);
        Assert.True(result.IsModified);
    }

    [Fact]
    public void WithNewValue_WithSameValue_StillCreatesNewInstance()
    {
        var originalValue = new CsvValue<int>("123", 123);
        var result = originalValue.WithNewValue(123);
        Assert.Equal(123, result.OriginalValue);
        Assert.Equal(123, result.CurrentValue);
        Assert.False(result.IsModified);
        Assert.Equal(originalValue, result);
    }

    [Fact]
    public void ResetToOriginal_RestoresOriginalValue()
    {
        var originalValue = new CsvValue<int>("123", 123);
        var modifiedValue = originalValue.WithNewValue(456);
        var result = modifiedValue.ResetToOriginal();
        Assert.Equal(123, result.OriginalValue);
        Assert.Equal(123, result.CurrentValue);
        Assert.False(result.IsModified);
    }

    [Fact]
    public void ResetToOriginal_WithNullOriginal_SetsCurrentToNull()
    {
        var originalValue = new CsvValue<string?>(null, null);
        var modifiedValue = originalValue.WithNewValue("test");
        var result = modifiedValue.ResetToOriginal();
        Assert.Null(result.OriginalValue);
        Assert.Null(result.CurrentValue);
        Assert.False(result.IsModified);
    }
}