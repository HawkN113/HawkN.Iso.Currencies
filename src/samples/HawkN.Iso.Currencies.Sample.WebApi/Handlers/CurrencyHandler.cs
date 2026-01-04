using HawkN.Iso.Currencies.Abstractions;
using HawkN.Iso.Currencies.Models;
using Microsoft.AspNetCore.Mvc;
namespace HawkN.Iso.Currencies.Sample.WebApi.Handlers;

public static class CurrencyHandler
{
    internal static IResult GetFiatCurrencies([FromServices] ICurrencyService service) =>
        Results.Ok(service.Query()
            .Includes
            .Type(CurrencyType.Fiat)
            .Build()
            .Select(c => $"{c.Code} - {c.Name}")
            .ToArray());

    internal static IResult GetFiatCurrency([FromServices] ICurrencyService service, [FromQuery] string currencyCode)
    {
        service.TryValidate(currencyCode, out var result);
        return !result.IsValid ? Results.BadRequest(result.Reason) : Results.Ok(service.Get(currencyCode));
    }

    internal static IResult GetPreciousMetals([FromServices] ICurrencyService service) =>
        Results.Ok(service.Query()
            .Includes
            .Type(CurrencyType.PreciousMetal)
            .Build()
            .Select(c => $"{c.Code} - {c.Name}")
            .ToArray());

    internal static IResult GetSpecialCurrencies([FromServices] ICurrencyService service) =>
        Results.Ok(service.Query()
            .Includes
            .Type(CurrencyType.SpecialReserve)
            .Type(CurrencyType.SpecialUnit)
            .Build()
            .Select(c => $"{c.Code} - {c.Name} ({c.CurrencyType})")
            .ToArray());
}