using HawkN.Iso.Currencies.Generators.Handlers;
namespace HawkN.Iso.Currencies.Generators.Tests.Handlers;

public class SupplementalDataHandlerTests
{
    private const string ValidXml = """
                                    <supplementalData>
                                      <currencyData>
                                        <region iso3166="US">
                                          <currency iso4217="USD"/>
                                        </region>
                                        <region iso3166="DE">
                                          <currency iso4217="EUR"/>
                                        </region>
                                        <region iso3166="JP">
                                          <currency iso4217="JPY"/>
                                        </region>
                                      </currencyData>
                                    </supplementalData>
                                    """;

    [Fact]
    public void LoadCurrencies_ShouldParseValidCurrencies()
    {
        // Arrange
        var handler = new SupplementalDataHandler(ValidXml);

        // Act
        var result = handler.LoadCurrencies();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(3, result.Count);

        Assert.True(result.ContainsKey("USD"));
        Assert.True(result.ContainsKey("EUR"));
        Assert.True(result.ContainsKey("JPY"));

        Assert.Equal("USD", result["USD"].Code);
        Assert.Equal("EUR", result["EUR"].Code);
        Assert.Equal("JPY", result["JPY"].Code);
    }

    [Fact]
    public void LoadCurrencies_ShouldIgnoreCurrenciesWithToAttribute()
    {
        // Arrange
        var xml = """
                  <supplementalData>
                    <currencyData>
                      <region iso3166="RU">
                        <currency iso4217="RUR" to="1998-01-01"/>
                        <currency iso4217="RUB"/>
                      </region>
                    </currencyData>
                  </supplementalData>
                  """;

        var handler = new SupplementalDataHandler(xml);

        // Act
        var result = handler.LoadCurrencies();

        // Assert
        Assert.Single(result);
        Assert.True(result.ContainsKey("RUB"));
        Assert.False(result.ContainsKey("RUR"));
    }

    [Fact]
    public void LoadCurrencies_ShouldIgnoreCurrenciesWithoutIso4217Code()
    {
        // Arrange
        var xml = """
                  <supplementalData>
                    <currencyData>
                      <region iso3166="XX">
                        <currency />
                        <currency iso4217="XXX"/>
                      </region>
                    </currencyData>
                  </supplementalData>
                  """;

        var handler = new SupplementalDataHandler(xml);

        // Act
        var result = handler.LoadCurrencies();

        // Assert
        Assert.Single(result);
        Assert.True(result.ContainsKey("XXX"));
    }

    [Fact]
    public void LoadCurrencies_ShouldDeduplicateCurrenciesAcrossRegions()
    {
        // Arrange
        var xml = """
                  <supplementalData>
                    <currencyData>
                      <region iso3166="US">
                        <currency iso4217="USD"/>
                      </region>
                      <region iso3166="EC">
                        <currency iso4217="USD"/>
                      </region>
                    </currencyData>
                  </supplementalData>
                  """;

        var handler = new SupplementalDataHandler(xml);

        // Act
        var result = handler.LoadCurrencies();

        // Assert
        Assert.Single(result);
        Assert.Equal("USD", result["USD"].Code);
    }

    [Fact]
    public void LoadCurrencies_ShouldBeCaseInsensitiveByCurrencyCode()
    {
        // Arrange
        var xml = """
                  <supplementalData>
                    <currencyData>
                      <region iso3166="US">
                        <currency iso4217="usd"/>
                      </region>
                      <region iso3166="EC">
                        <currency iso4217="USD"/>
                      </region>
                    </currencyData>
                  </supplementalData>
                  """;

        var handler = new SupplementalDataHandler(xml);

        // Act
        var result = handler.LoadCurrencies();

        // Assert
        Assert.Single(result);
        Assert.True(result.ContainsKey("USD"));
    }

    [Fact]
    public void LoadCurrencies_ShouldReturnEmptyDictionary_WhenNoCurrencyDataExists()
    {
        // Arrange
        var xml = """
                  <supplementalData>
                    <currencyData />
                  </supplementalData>
                  """;

        var handler = new SupplementalDataHandler(xml);

        // Act
        var result = handler.LoadCurrencies();

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
    }
}