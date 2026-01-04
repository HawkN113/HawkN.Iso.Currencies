using HawkN.Iso.Currencies.Generators.Handlers;
using HawkN.Iso.Currencies.Generators.Models;
namespace HawkN.Iso.Currencies.Generators;

internal sealed class CurrencyDataLoader : CurrencyLoaderBase
{
    private readonly CurrencyData _actualCurrencyData = new();

    public override CurrencyData ActualCurrencyData => _actualCurrencyData;

    public CurrencyDataLoader(string supplementalXml, string enXml, string replacementJson, string currencyCodesCsv)
    {
        var replacements = new JsonReplacementCurrencyHandler(replacementJson).LoadNameReplacements();
        var actual = new SupplementalDataHandler(supplementalXml).LoadCurrencies();
        var codes = new CurrencyCodesHandler(currencyCodesCsv).LoadCurrencyCodes();
        var names = new CurrencyNamesHandler(enXml).LoadNames();
        foreach (var currency in actual.Values)
        {
            if (names.TryGetValue(currency.Code, out var name))
                currency.Name = name;
            if (codes.TryGetValue(currency.Code, out var code))
                currency.NumericCode = code;
        }

        _actualCurrencyData.PublishedDate = DateTime.UtcNow.ToString("yyyy-MM-dd");
        _actualCurrencyData.Currencies = ProcessCurrencies(actual.Values.AsEnumerable(), replacements);
    }

    private static List<CurrencyRow> ProcessCurrencies(IEnumerable<CurrencyRow> currencies,
        IReadOnlyDictionary<string, string> replacements)
        => currencies
            .Where(c => !ExcludedCodes.Contains(c.Code))
            .Select(c => new CurrencyRow
            {
                Code = c.Code,
                Name = replacements.TryGetValue(c.Code, out var newName) ? newName : c.Name,
                NumericCode = c.NumericCode,
                CurrencyType = GetCurrencyType(c.Code)
            })
            .OrderBy(c => c.Code)
            .ToList();

    private static CurrencyType GetCurrencyType(string code)
    {
        if (PreciousMetalsCodes.Contains(code))
            return CurrencyType.PreciousMetal;
        if (SpecialReserveCodes.Contains(code))
            return CurrencyType.SpecialReserve;
        return SpecialUnits.Contains(code) ? CurrencyType.SpecialUnit : CurrencyType.Fiat;
    }
}