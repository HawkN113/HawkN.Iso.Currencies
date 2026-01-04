using HawkN.Iso.Currencies.Generators.Handlers;
namespace HawkN.Iso.Currencies.Generators.Tests.Handlers;

public class CurrencyNamesHandlerTests
{
    private const string ValidXml = """
                                    <ldml>
                                      <numbers>
                                        <currencies>
                                          <currency type="USD">
                                            <displayName>US Dollar</displayName>
                                          </currency>
                                          <currency type="EUR">
                                            <displayName>Euro</displayName>
                                          </currency>
                                          <currency type="JPY">
                                            <displayName>Japanese Yen</displayName>
                                          </currency>
                                        </currencies>
                                      </numbers>
                                    </ldml>
                                    """;

    [Fact]
    public void LoadNames_ShouldParseValidXml()
    {
        // Arrange
        var handler = new CurrencyNamesHandler(ValidXml);

        // Act
        var result = handler.LoadNames();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(3, result.Count);
        Assert.Equal("US Dollar", result["USD"]);
        Assert.Equal("Euro", result["EUR"]);
        Assert.Equal("Japanese Yen", result["JPY"]);
    }

    [Fact]
    public void LoadNames_ShouldIgnoreCurrenciesWithoutDisplayName()
    {
        // Arrange
        var xml = """
                  <ldml>
                    <numbers>
                      <currencies>
                        <currency type="USD">
                          <displayName>US Dollar</displayName>
                        </currency>
                        <currency type="EUR" />
                      </currencies>
                    </numbers>
                  </ldml>
                  """;

        var handler = new CurrencyNamesHandler(xml);

        // Act
        var result = handler.LoadNames();

        // Assert
        Assert.Single(result);
        Assert.Equal("US Dollar", result["USD"]);
    }

    [Fact]
    public void LoadNames_ShouldIgnoreCurrenciesWithoutTypeAttribute()
    {
        // Arrange
        var xml = """
                  <ldml>
                    <numbers>
                      <currencies>
                        <currency>
                          <displayName>Unknown</displayName>
                        </currency>
                        <currency type="USD">
                          <displayName>US Dollar</displayName>
                        </currency>
                      </currencies>
                    </numbers>
                  </ldml>
                  """;

        var handler = new CurrencyNamesHandler(xml);

        // Act
        var result = handler.LoadNames();

        // Assert
        Assert.Single(result);
        Assert.Equal("US Dollar", result["USD"]);
    }

    [Fact]
    public void LoadNames_ShouldUseFirstEntry_WhenDuplicateCurrencyCodesExist()
    {
        // Arrange
        var xml = """
                  <ldml>
                    <numbers>
                      <currencies>
                        <currency type="USD">
                          <displayName>US Dollar</displayName>
                        </currency>
                        <currency type="USD">
                          <displayName>US Dollar Duplicate</displayName>
                        </currency>
                      </currencies>
                    </numbers>
                  </ldml>
                  """;

        var handler = new CurrencyNamesHandler(xml);

        // Act
        var result = handler.LoadNames();

        // Assert
        Assert.Single(result);
        Assert.Equal("US Dollar", result["USD"]);
    }

    [Fact]
    public void LoadNames_ShouldReturnEmptyDictionary_WhenNoCurrenciesFound()
    {
        // Arrange
        var xml = """
                  <ldml>
                    <numbers>
                      <currencies />
                    </numbers>
                  </ldml>
                  """;

        var handler = new CurrencyNamesHandler(xml);

        // Act
        var result = handler.LoadNames();

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
    }
}