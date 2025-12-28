using HawkN.Iso.Currencies.Generators.Handlers;
namespace HawkN.Iso.Currencies.Generators.Tests.Handlers;

public class JsonHistoricalCurrencyHandlerTests
{
    private const string HistoricalJson = """
                                          {
                                              "ISO_4217": {
                                                "_Pblshd": "2025-05-12",
                                                "HstrcCcyTbl": {
                                                  "HstrcCcyNtry": [
                                                        {
                                                          "CtryNm": "AFGHANISTAN",
                                                          "CcyNm": "Afghani",
                                                          "Ccy": "AFA",
                                                          "CcyNbr": "004",
                                                          "WthdrwlDt": "2003-01"
                                                        },
                                                        {
                                                          "CtryNm": "ÅLAND ISLANDS",
                                                          "CcyNm": "Markka",
                                                          "Ccy": "FIM",
                                                          "CcyNbr": "246",
                                                          "WthdrwlDt": "2002-03"
                                                        },
                                                        {
                                                          "CtryNm": "ALBANIA",
                                                          "CcyNm": "Old Lek",
                                                          "Ccy": "ALK",
                                                          "CcyNbr": "008",
                                                          "WthdrwlDt": "1989-12"
                                                        },
                                                        {
                                                          "CtryNm": "ANDORRA",
                                                          "CcyNm": "Andorran Peseta",
                                                          "Ccy": "ADP",
                                                          "CcyNbr": "020",
                                                          "WthdrwlDt": "2003-07"
                                                        },
                                                        {
                                                          "CtryNm": "ANDORRA",
                                                          "CcyNm": "Spanish Peseta",
                                                          "Ccy": "ESP",
                                                          "CcyNbr": "724",
                                                          "WthdrwlDt": "2002-03"
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
        var handler = new JsonHistoricalCurrencyHandler(HistoricalJson);

        // Act
        var result = handler.LoadCurrencies();

        // Assert
        Assert.NotNull(result);
        Assert.Equal("2025-05-12", result.PublishedDate);
        Assert.Equal(5, result.Currencies.Count);

        var tst = result.Currencies.First(c => c.Code == "FIM");
        Assert.Equal("Markka", tst.Name);
        Assert.Equal("ÅLAND ISLANDS", tst.Country);
        Assert.Equal("246", tst.NumericCode);
        Assert.Equal("2002-03", tst.WithdrawalDate);
    }

    [Fact]
    public void LoadCurrencies_ShouldHandleComplexNameFormat()
    {
        // Arrange
        var handler = new JsonHistoricalCurrencyHandler(HistoricalJson);

        // Act
        var result = handler.LoadCurrencies();

        // Assert
        var currency = result.Currencies[0];
        Assert.Equal("Afghani", currency.Name);
        Assert.Equal("AFGHANISTAN", currency.Country);
        Assert.Equal("004", currency.NumericCode);
        Assert.Equal("2003-01", currency.WithdrawalDate);
    }
}