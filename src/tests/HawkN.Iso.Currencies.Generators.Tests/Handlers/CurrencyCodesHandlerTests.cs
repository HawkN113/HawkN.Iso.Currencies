using HawkN.Iso.Currencies.Generators.Handlers;
namespace HawkN.Iso.Currencies.Generators.Tests.Handlers;

public class CurrencyCodesHandlerTests
{
    private const string ValidCsv = """
                                    AlphabeticCode,NumericCode
                                    USD,840
                                    EUR,978
                                    JPY,392
                                    """;

    [Fact]
    public void LoadCurrencyCodes_ShouldParseValidCsv()
    {
        // Arrange
        var handler = new CurrencyCodesHandler(ValidCsv);

        // Act
        var result = handler.LoadCurrencyCodes();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(3, result.Count);
        Assert.Equal("840", result["USD"]);
        Assert.Equal("978", result["EUR"]);
        Assert.Equal("392", result["JPY"]);
    }

    [Fact]
    public void LoadCurrencyCodes_ShouldIgnoreInvalidRows()
    {
        // Arrange
        var csv = """
                  AlphabeticCode,NumericCode
                  USD,840
                  US,840
                  EUR,
                  JPY,392
                  """;

        var handler = new CurrencyCodesHandler(csv);

        // Act
        var result = handler.LoadCurrencyCodes();

        // Assert
        Assert.Equal(2, result.Count);
        Assert.True(result.ContainsKey("USD"));
        Assert.True(result.ContainsKey("JPY"));
    }

    [Fact]
    public void LoadCurrencyCodes_ShouldSupportQuotedFields()
    {
        // Arrange
        var csv = """
                  AlphabeticCode,NumericCode
                  "USD","840"
                  "EUR","978"
                  """;

        var handler = new CurrencyCodesHandler(csv);

        // Act
        var result = handler.LoadCurrencyCodes();

        // Assert
        Assert.Equal("840", result["USD"]);
        Assert.Equal("978", result["EUR"]);
    }

    [Fact]
    public void LoadCurrencyCodes_ShouldOverrideDuplicateCodes()
    {
        // Arrange
        var csv = """
                  AlphabeticCode,NumericCode
                  USD,840
                  USD,999
                  """;

        var handler = new CurrencyCodesHandler(csv);

        // Act
        var result = handler.LoadCurrencyCodes();

        // Assert
        Assert.Single(result);
        Assert.Equal("999", result["USD"]);
    }

    [Fact]
    public void LoadCurrencyCodes_ShouldThrow_WhenRequiredHeadersMissing()
    {
        // Arrange
        var csv = """
                  Code,Number
                  USD,840
                  """;

        var handler = new CurrencyCodesHandler(csv);

        // Act & Assert
        Assert.Throws<InvalidDataException>(() => handler.LoadCurrencyCodes());
    }

    [Fact]
    public void LoadCurrencyCodes_ShouldReturnEmptyDictionary_WhenInputIsEmpty()
    {
        // Arrange
        var handler = new CurrencyCodesHandler(string.Empty);

        // Act
        var result = handler.LoadCurrencyCodes();

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
    }

    [Fact]
    public void LoadCurrencyCodes_ShouldHandleCaseInsensitiveHeaders()
    {
        // Arrange
        var csv = """
                  alphabeticcode,numericcode
                  USD,840
                  """;

        var handler = new CurrencyCodesHandler(csv);

        // Act
        var result = handler.LoadCurrencyCodes();

        // Assert
        Assert.Single(result);
        Assert.Equal("840", result["USD"]);
    }
}
