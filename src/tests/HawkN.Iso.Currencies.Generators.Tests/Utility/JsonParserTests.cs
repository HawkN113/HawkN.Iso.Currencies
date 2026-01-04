using HawkN.Iso.Currencies.Generators.Utility;
namespace HawkN.Iso.Currencies.Generators.Tests.Utility;

public class JsonParserTests
{
    [Fact]
    public void ExtractPublishedDate_ShouldReturnValue_WhenValidJson()
    {
        // Arrange
        var json = """{ "_Pblshd": "2025-10-28" }""";

        // Act
        var result = JsonParser.ExtractPublishedDate(json);

        // Assert
        Assert.Equal("2025-10-28", result);
    }

    [Fact]
    public void ExtractPublishedDate_ShouldThrow_WhenKeyMissing()
    {
        // Arrange
        var json = """{ "Other": "value" }""";

        // Act & Assert
        var ex = Assert.Throws<InvalidOperationException>(() => JsonParser.ExtractPublishedDate(json));
        Assert.Contains("Published date", ex.Message);
    }

    [Fact]
    public void ExtractPublishedDate_ShouldThrow_WhenJsonIsNull()
    {
        Assert.Throws<ArgumentNullException>(() => JsonParser.ExtractPublishedDate(null!));
    }

    [Fact]
    public void ExtractArray_ShouldReturnArrayContent_WhenValidJson()
    {
        // Arrange
        var json = """{ "Data": [ { "Value": 1 }, { "Value": 2 } ] }""";

        // Act
        var result = JsonParser.ExtractArray(json, "Data");

        // Assert
        Assert.Contains(@"""Value"": 1", result);
        Assert.Contains(@"""Value"": 2", result);
    }

    [Fact]
    public void ExtractArray_ShouldThrow_WhenKeyNotFound()
    {
        // Arrange
        var json = """{ "Other": [] }""";

        // Act
        var ex = Assert.Throws<InvalidOperationException>(() => JsonParser.ExtractArray(json, "Data"));
        Assert.Contains("required in the JSON content", ex.Message);
    }

    [Fact]
    public void ExtractArray_ShouldThrow_WhenArrayIsMalformed()
    {
        // Arrange
        var json = """{ "Data": [ { "A": 1 }""";

        // Act
        var ex = Assert.Throws<InvalidOperationException>(() => JsonParser.ExtractArray(json, "Data"));
        Assert.Contains("Cannot locate end", ex.Message);
    }

    [Fact]
    public void ExtractArray_ShouldThrow_WhenJsonIsNull()
    {
        Assert.Throws<ArgumentNullException>(() => JsonParser.ExtractArray(null!, "Key"));
    }

    [Fact]
    public void ExtractArray_ShouldThrow_WhenKeyIsNull()
    {
        Assert.Throws<ArgumentNullException>(() => JsonParser.ExtractArray("{}", null!));
    }

    [Fact]
    public void Extract_ShouldReturnString_WhenKeyIsString()
    {
        // Arrange
        var json = """{ "Name": "Euro" }""";

        // Act
        var result = JsonParser.Extract(json, "Name");
        Assert.Equal("Euro", result);
    }

    [Fact]
    public void Extract_ShouldReturnNumber_WhenKeyIsNumeric()
    {
        // Arrange
        var json = """{ "Code": 840 }""";

        // Act
        var result = JsonParser.Extract(json, "Code");
        Assert.Equal("840", result);
    }

    [Fact]
    public void Extract_ShouldReturnNull_WhenKeyNotFound()
    {
        // Arrange
        var json = """{ "X": 1 }""";

        // Act
        var result = JsonParser.Extract(json, "Y");
        Assert.Null(result);
    }

    [Fact]
    public void Extract_ShouldReturnNormalizedString_WithSingleQuotes()
    {
        // Arrange
        var json = """{ "Text": "He said \"Hello\"" }""";

        // Act
        var result = JsonParser.Extract(json, "Text");
        Assert.Equal("He said 'Hello'", result);
    }

    [Fact]
    public void HandleEscapeChar_ShouldToggleStringState()
    {
        // Arrange
        bool inString = false, escape = false;

        // Act & Assert
        Assert.True(JsonParser.HandleEscapeChar('"', ref inString, ref escape));
        Assert.True(inString);
        Assert.True(JsonParser.HandleEscapeChar('"', ref inString, ref escape));
        Assert.False(inString);
    }

    [Fact]
    public void HandleEscapeChar_ShouldHandleBackslash()
    {
        // Arrange
        bool inString = false, escape = false;

        // Act & Assert
        Assert.True(JsonParser.HandleEscapeChar('\\', ref inString, ref escape));
        Assert.True(escape);
    }

    [Fact]
    public void HandleEscapeChar_ShouldReturnFalseForOtherChars()
    {
        // Arrange
        bool inString = false, escape = false;

        // Act & Assert
        var result = JsonParser.HandleEscapeChar('x', ref inString, ref escape);
        Assert.False(result);
    }
}