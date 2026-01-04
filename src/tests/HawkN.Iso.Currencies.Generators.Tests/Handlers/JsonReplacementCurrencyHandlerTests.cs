using HawkN.Iso.Currencies.Generators.Handlers;
namespace HawkN.Iso.Currencies.Generators.Tests.Handlers;

public class JsonReplacementCurrencyHandlerTests
{
    private const string ReplacementJson = """
                                           [
                                               { "Ccy": "USD", "CcyNm": "US Dollar Updated" },
                                               { "Ccy": "EUR", "CcyNm": "Euro Modified" },
                                               { "Ccy": "JPY", "CcyNm": "Japanese Yen Updated" }
                                           ]
                                           """;

    [Fact]
    public void LoadNameReplacements_ShouldParseValidJson()
    {
        // Arrange
        var handler = new JsonReplacementCurrencyHandler(ReplacementJson);

        // Act
        var result = handler.LoadNameReplacements();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(3, result.Count);
        Assert.Equal("US Dollar Updated", result["USD"]);
        Assert.Equal("Euro Modified", result["EUR"]);
        Assert.Equal("Japanese Yen Updated", result["JPY"]);
    }

    [Fact]
    public void LoadNameReplacements_ShouldIgnoreEntriesWithoutCode()
    {
        // Arrange
        var json = """
                   [
                       { "CcyNm": "No Code Here" },
                       { "Ccy": "GBP", "CcyNm": "Pound Updated" }
                   ]
                   """;

        var handler = new JsonReplacementCurrencyHandler(json);

        // Act
        var result = handler.LoadNameReplacements();

        // Assert
        Assert.Single(result);
        Assert.Equal("Pound Updated", result["GBP"]);
    }

    [Fact]
    public void LoadNameReplacements_ShouldReturnEmptyDictionary_WhenNoMatchesFound()
    {
        // Arrange
        var json = """{ "SomeOtherData": [] }""";
        var handler = new JsonReplacementCurrencyHandler(json);

        // Act
        var result = handler.LoadNameReplacements();

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
    }
}