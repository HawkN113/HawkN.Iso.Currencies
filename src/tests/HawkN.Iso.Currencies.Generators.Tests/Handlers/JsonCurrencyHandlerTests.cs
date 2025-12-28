using HawkN.Iso.Currencies.Generators.Handlers;
namespace HawkN.Iso.Currencies.Generators.Tests.Handlers;

public class JsonCurrencyHandlerTests
{
    private const string ActualJson = """
                                      {
                                        "ISO_4217": {
                                          "_Pblshd": "2025-05-12",
                                          "CcyTbl": {
                                            "CcyNtry": [
                                              {
                                                "CtryNm": "AUSTRIA",
                                                "CcyNm": "Euro",
                                                "Ccy": "EUR",
                                                "CcyNbr": "978"
                                              },
                                              {
                                                "CtryNm": "UNITED STATES",
                                                "CcyNm": { "__text": "US Dollar" },
                                                "Ccy": "USD",
                                                "CcyNbr": "840"
                                              },
                                              {
                                                "CtryNm": "JAPAN",
                                                "CcyNm": "Yen",
                                                "Ccy": "JPY",
                                                "CcyNbr": "392"
                                              },
                                              {
                                                "CtryNm": "UNITED KINGDOM",
                                                "CcyNm": "Pound Sterling",
                                                "Ccy": "GBP",
                                                "CcyNbr": "826"
                                              },
                                              {
                                                "CtryNm": "SWITZERLAND",
                                                "CcyNm": "Swiss Franc",
                                                "Ccy": "CHF",
                                                "CcyNbr": "756"
                                              }
                                            ]
                                          }
                                        }
                                      }
                                      """;

    [Fact]
    public void LoadCurrencies_ShouldParseValidJson()
    {
        // Arrange
        var handler = new JsonCurrencyHandler(ActualJson);

        // Act
        var result = handler.LoadCurrencies();

        // Assert
        Assert.NotNull(result);
        Assert.Equal("2025-05-12", result.PublishedDate);
        Assert.Equal(5, result.Currencies.Count);

        var eur = result.Currencies.First(c => c.Code == "EUR");
        Assert.Equal("Euro", eur.Name);
        Assert.Equal("AUSTRIA", eur.Country);
        Assert.Equal("978", eur.NumericCode);
        Assert.Null(eur.WithdrawalDate);
    }

    [Fact]
    public void LoadCurrencies_ShouldHandleNestedTextName()
    {
        // Arrange
        var handler = new JsonCurrencyHandler(ActualJson);

        // Act
        var result = handler.LoadCurrencies();

        // Assert
        var usd = result.Currencies.First(c => c.Code == "USD");
        Assert.Equal("US Dollar", usd.Name);
        Assert.Equal("UNITED STATES", usd.Country);
        Assert.Equal("840", usd.NumericCode);
        Assert.Null(usd.WithdrawalDate);
    }

    [Fact]
    public void LoadCurrencies_ShouldHandleAllCurrenciesCorrectly()
    {
        // Arrange
        var handler = new JsonCurrencyHandler(ActualJson);

        // Act
        var result = handler.LoadCurrencies();

        // Assert
        Assert.Collection(result.Currencies,
            c =>
            {
                Assert.Equal("EUR", c.Code);
                Assert.Equal("Euro", c.Name);
                Assert.Equal("AUSTRIA", c.Country);
                Assert.Equal("978", c.NumericCode);
                Assert.Null(c.WithdrawalDate);
            },
            c =>
            {
                Assert.Equal("USD", c.Code);
                Assert.Equal("US Dollar", c.Name);
                Assert.Equal("UNITED STATES", c.Country);
                Assert.Equal("840", c.NumericCode);
                Assert.Null(c.WithdrawalDate);
            },
            c =>
            {
                Assert.Equal("JPY", c.Code);
                Assert.Equal("Yen", c.Name);
                Assert.Equal("JAPAN", c.Country);
                Assert.Equal("392", c.NumericCode);
                Assert.Null(c.WithdrawalDate);
            },
            c =>
            {
                Assert.Equal("GBP", c.Code);
                Assert.Equal("Pound Sterling", c.Name);
                Assert.Equal("UNITED KINGDOM", c.Country);
                Assert.Equal("826", c.NumericCode);
                Assert.Null(c.WithdrawalDate);
            },
            c =>
            {
                Assert.Equal("CHF", c.Code);
                Assert.Equal("Swiss Franc", c.Name);
                Assert.Equal("SWITZERLAND", c.Country);
                Assert.Equal("756", c.NumericCode);
                Assert.Null(c.WithdrawalDate);
            });
    }
}