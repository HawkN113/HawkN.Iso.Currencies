using System.Xml.Linq;
using HawkN.Iso.Currencies.Generators.Models;
namespace HawkN.Iso.Currencies.Generators.Handlers;

internal sealed class SupplementalDataHandler(string xmlContent)
{
    private readonly XDocument _doc = XDocument.Parse(xmlContent);

    public Dictionary<string, CurrencyRow> LoadCurrencies()
    {
        var currencies = new Dictionary<string, CurrencyRow>(StringComparer.OrdinalIgnoreCase);

        ParseRegionCurrencies(currencies);

        return currencies;
    }

    private void ParseRegionCurrencies(Dictionary<string, CurrencyRow> currencies)
    {
        var regionCurrencies =
            _doc.Descendants("currencyData")
                .Descendants("region")
                .SelectMany(region =>
                    region.Elements("currency")
                        .Where(c => !string.IsNullOrEmpty(c.Attribute("iso4217")?.Value) &&
                                    string.IsNullOrEmpty(c.Attribute("to")?.Value))
                        .Select(c => new
                        {
                            Code = c.Attribute("iso4217")?.Value,
                        }))
                .Distinct();

        foreach (var code in regionCurrencies.Select(entry => entry.Code))
        {
            if (string.IsNullOrEmpty(code))
                continue;

            if (currencies.TryGetValue(code!, out var currency)) continue;

            currency = new CurrencyRow
            {
                Code = code!
            };
            currencies[code!] = currency;
        }
    }
}