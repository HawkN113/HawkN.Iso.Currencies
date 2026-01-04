using HawkN.Iso.Currencies.Sample.WebApi.Handlers;
namespace HawkN.Iso.Currencies.Sample.WebApi.Extensions;

/// <summary>
/// Extension for currency-related endpoints.
/// </summary>
static class CurrencyEndpointExtensions
{
    public static IEndpointRouteBuilder MapCurrencyEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("/fiat/currencies", CurrencyHandler.GetFiatCurrencies)
            .WithName("GetFiatCurrencies")
            .WithOpenApi(o =>
            {
                o.Summary = "Get actual fiat currencies (EUR, USD, etc.)";
                o.Description = "Returns a short code and names.";
                return o;
            });

        app.MapGet("/fiat/currency", CurrencyHandler.GetFiatCurrency)
            .WithName("GetFiatCurrency")
            .WithOpenApi(o =>
            {
                o.Summary = "Get fiat currency by code (EUR, USD, etc.)";
                o.Description = "Returns detailed currency info.";
                return o;
            });

        app.MapGet("/preciousMetals/currencies", CurrencyHandler.GetPreciousMetals)
            .WithName("GetPreciousMetals")
            .WithOpenApi(o =>
            {
                o.Summary = "Get precious metals (XAG, XAU, XPD, XPT)";
                o.Description = "Returns a short code and names.";
                return o;
            });

        app.MapGet("/special/currencies", CurrencyHandler.GetSpecialCurrencies)
            .WithName("GetSpecialCurrencies")
            .WithOpenApi(o =>
            {
                o.Summary = "Get special currencies (XBA, XSU, XXX, etc.)";
                o.Description = "Returns a short code, names and type.";
                return o;
            });

        return app;
    }
}
