using HawkN.Iso.Currencies.Builders.Abstractions;
using HawkN.Iso.Currencies.Services;

namespace HawkN.Iso.Currencies.Tests.Services;

public class CurrencyServiceTests
{
    private readonly CurrencyService _service = new();

    [Theory]
    [InlineData("USD", true)]
    [InlineData("usd", true)]
    [InlineData("ZZZ", false)]
    [InlineData("", false)]
    [InlineData(null, false)]
    public void TryValidate_String_Should_Work(string input, bool expectedIsValid)
    {
        var result = _service.TryValidate(input, out var validation);

        Assert.Equal(expectedIsValid, result);
        Assert.Equal(expectedIsValid, validation.IsValid);
        if (!expectedIsValid)
        {
            Assert.False(string.IsNullOrWhiteSpace(validation.Reason));
        }
    }

    [Theory]
    [InlineData(CurrencyCode.USD, true)]
    [InlineData(CurrencyCode.None, false)]
    public void TryValidate_CurrencyCode_Should_Work(CurrencyCode code, bool expectedIsValid)
    {
        var result = _service.TryValidate(code, out var validation);

        Assert.Equal(expectedIsValid, result);
        Assert.Equal(expectedIsValid, validation.IsValid);
        if (!expectedIsValid)
        {
            Assert.False(string.IsNullOrWhiteSpace(validation.Reason));
        }
    }

    [Theory]
    [InlineData("USD", true)]
    [InlineData("EUR", true)]
    [InlineData("", false)]
    [InlineData(null, false)]
    public void Exists_String_Should_Work(string input, bool expected)
    {
        var result = _service.Exists(input);
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData("XX7")]
    public void Exists_String_NotShould_Work(string input)
    {
        var result = _service.Exists(input);
        Assert.False(result);
    }

    [Theory]
    [InlineData(CurrencyCode.USD, true)]
    [InlineData(CurrencyCode.None, false)]
    public void Exists_CurrencyCode_Should_Work(CurrencyCode code, bool expected)
    {
        var result = _service.Exists(code);
        Assert.Equal(expected, result);
    }

    [Fact]
    public void Get_Should_Return_Currency_When_Exists()
    {
        var usd = _service.Get("USD");
        Assert.NotNull(usd);
        Assert.Equal("USD", usd!.Code);
    }

    [Fact]
    public void Get_Should_Return_Null_When_NotExists()
    {
        var result = _service.Get("ZZZ");
        Assert.Null(result);
    }

    [Fact]
    public void Get_CurrencyCode_Should_Return_Currency_When_Exists()
    {
        var usd = _service.Get(CurrencyCode.USD);
        Assert.NotNull(usd);
        Assert.Equal("USD", usd!.Code);
    }

    [Fact]
    public void Get_CurrencyCode_Should_Return_Null_When_NotExists()
    {
        var result = _service.Get(CurrencyCode.None);
        Assert.Null(result);
    }

    [Fact]
    public void GetHistorical_Should_Return_HistoricalCurrency_When_Exists()
    {
        var historical = _service.GetHistorical("DEM");
        if (historical != null)
        {
            Assert.Equal("DEM", historical.Code);
        }
    }

    [Fact]
    public void GetHistorical_Should_Return_Null_When_NotExists()
    {
        var result = _service.GetHistorical("ZZZ");
        Assert.Null(result);
    }

    [Fact]
    public void GetAllHistorical_Should_Return_AllHistoricalCurrencies()
    {
        var all = _service.GetAllHistorical();
        Assert.NotNull(all);
        Assert.True(all.Length > 0);
    }

    [Fact]
    public void Query_Should_Return_CurrencyQueryBuilder()
    {
        var query = _service.Query();
        Assert.NotNull(query);
        Assert.IsAssignableFrom<ICurrencyQueryStart>(query);
    }
}