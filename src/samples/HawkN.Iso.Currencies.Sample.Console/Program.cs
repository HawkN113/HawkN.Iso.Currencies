using HawkN.Iso.Currencies;
using HawkN.Iso.Currencies.Abstractions;
using HawkN.Iso.Currencies.Extensions;
using HawkN.Iso.Currencies.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        // ---- Register Currency service ----
        services.AddCurrencyService();
    })
    .Build();
try
{
    var container = host.Services;
    using var scope = container.CreateScope();
    // ---- Retrieve Currency service instance ----
    var currencyService = scope.ServiceProvider.GetRequiredService<ICurrencyService>();

    // -----------------
    // ---- Queries ----
    // -----------------

    Console.WriteLine(" ---- Queries ---- ");

    // ---- Get all existing currencies ----
    Console.WriteLine(" ---- All existing currencies (special, metal, reserve, fiat) ---- ");
    foreach (var currency in currencyService!.Query()
                 .Includes
                 .Type(CurrencyType.Fiat)
                 .Type(CurrencyType.SpecialUnit)
                 .Type(CurrencyType.SpecialReserve)
                 .Type(CurrencyType.PreciousMetal)
                 .Build())
    {
        Console.WriteLine(
            $"\t{currency.Code} - {currency.Name}");
    }

    // ---- Get fiat currencies ---- 
    Console.WriteLine(" ---- Fiat currencies ---- ");
    foreach (var currency in currencyService!
                 .Query()
                 .Includes.Type(CurrencyType.Fiat)
                 .Build())
    {
        Console.WriteLine($"\t {currency.Code} - {currency.Name}");
    }

    // ---- Get currencies by query ---- 
    Console.WriteLine(" ---- Query: Includes only `EUR` and `USD` in the list ---- ");
    foreach (var currency in currencyService!.Query()
                 .Includes
                 .Type(CurrencyType.Fiat)
                 .With(w => w.Codes(nameof(CurrencyCode.EUR), nameof(CurrencyCode.USD)))
                 .Build())
    {
        Console.WriteLine(
            $"\t{currency.Code} - {currency.Name}");
    }

    // ---- Get currencies by advanced query (LINQ) ---- 
    Console.WriteLine(" ---- Advanced Query (LINQ): Includes only `EUR` and `USD` in the list ---- ");
    foreach (var currency in currencyService!.Query()
                 .Includes
                 .Type(CurrencyType.Fiat)
                 .Where(q => q.Code is nameof(CurrencyCode.EUR) or nameof(CurrencyCode.USD))
                 .Build())
    {
        Console.WriteLine(
            $"\t{currency.Code} - {currency.Name}");
    }

    // ---- Get historical currencies ---- 
    Console.WriteLine(" ---- Historical (Withdrawal) currencies ---- ");
    foreach (var currency in currencyService.GetAllHistorical())
    {
        var withdrawalDate = currency.WithdrawalDate.HasValue
            ? currency.WithdrawalDate.Value.ToString("yyyy MMMM dd")
            : "Unknown date";
        Console.WriteLine(
            $"\t{currency.Code} - {currency.Name} ({withdrawalDate})");
    }

    // -------------------------
    // ---- Lookup currency ----
    // -------------------------

    Console.WriteLine(" ---- Lookup currency ---- ");
    Console.WriteLine(
        $"\tLookup by string 'AFN': {currencyService.Get("AFN")!.Code}");
    Console.WriteLine(
        $"\tLookup by currency code 'CurrencyCode.AFN': {currencyService.Get(CurrencyCode.AFN)!.Code}");

    // --------------------
    // ---- Validation ----
    // --------------------

    Console.WriteLine(" ---- Validation ----");
    currencyService.TryValidate("ESP", out var invalidateResult);
    Console.WriteLine(
        $"\tValidation code 'ESP': {invalidateResult.Reason}, validation status: {invalidateResult.IsValid}");
    currencyService.TryValidate("USD", out var validateResult);
    Console.WriteLine(
        $"\tValidation code 'USD': {validateResult.Reason}, validation status: {validateResult.IsValid}");

    await host.RunAsync();
}
catch (Exception ex)
{
    Console.WriteLine(ex);
}