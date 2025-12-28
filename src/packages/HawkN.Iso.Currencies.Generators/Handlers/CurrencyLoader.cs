using HawkN.Iso.Currencies.Generators.Models;
namespace HawkN.Iso.Currencies.Generators.Handlers;

internal sealed class CurrencyLoader
{
    private readonly CurrencyData _actualCurrencyData = new();
    private readonly CurrencyData _historicalCurrencyData = new();

    private static readonly HashSet<string> PreciousMetalsCodes = new(StringComparer.OrdinalIgnoreCase)
        { "XAG", "XAU", "XPD", "XPT" };

    private static readonly HashSet<string> SpecialReserveCodes = new(StringComparer.OrdinalIgnoreCase)
        { "XXX", "XTS" };

    private static readonly HashSet<string> SpecialUnits = new(StringComparer.OrdinalIgnoreCase)
        { "XBA", "XBB", "XBC", "XBD", "XSU", "XUA" };

    private static readonly HashSet<string> ExcludedCodes = new(StringComparer.OrdinalIgnoreCase)
        { "VED", "XAD", "XCG", "ZWG" };

    public CurrencyData ActualCurrencyData => _actualCurrencyData;
    public CurrencyData HistoricalCurrencyData => _historicalCurrencyData;

    public CurrencyLoader(string actualJson, string replacementJson, string historicalJson)
    {
        var replacements = new JsonReplacementCurrencyHandler(replacementJson).LoadNameReplacements();
        var actual = new JsonCurrencyHandler(actualJson).LoadCurrencies();
        var historical = new JsonHistoricalCurrencyHandler(historicalJson).LoadCurrencies();

        _actualCurrencyData.PublishedDate = actual.PublishedDate;
        _historicalCurrencyData.PublishedDate = historical.PublishedDate;

        _actualCurrencyData.Currencies = ProcessCurrencies(actual.Currencies, replacements);
        _historicalCurrencyData.Currencies = ProcessCurrencies(historical.Currencies, replacements);
    }

    private static List<CurrencyRow> ProcessCurrencies(IEnumerable<CurrencyRaw> currencies,
        IReadOnlyDictionary<string, string> replacements)
        => currencies
            .Where(c => !ExcludedCodes.Contains(c.Code))
            .Select(c => new CurrencyRow
            {
                Code = c.Code,
                Name = replacements.TryGetValue(c.Code, out var newName) ? newName : c.Name,
                Country = c.Country,
                NumericCode = c.NumericCode,
                CurrencyType = GetCurrencyType(c.Code),
                IsHistoric = !string.IsNullOrEmpty(c.WithdrawalDate),
                WithdrawalDate = c.WithdrawalDate
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