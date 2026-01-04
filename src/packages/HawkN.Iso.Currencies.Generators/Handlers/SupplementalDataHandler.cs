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
        var regionCurrencies = _doc
            .Descendants("currencyData")
            .Descendants("region")
            .SelectMany(region =>
                region.Elements("currency")
                    .Where(c => !string.IsNullOrEmpty(c.Attribute("iso4217")?.Value) &&
                                string.IsNullOrEmpty(c.Attribute("to")?.Value))
                    .Select(c => c.Attribute("iso4217")!.Value))
            .Where(code => !string.IsNullOrEmpty(code))
            .Distinct(StringComparer.OrdinalIgnoreCase);

        foreach (var code in regionCurrencies)
        {
            if (!currencies.TryGetValue(code, out _))
                currencies[code] = new CurrencyRow { Code = code };
        }
    }
}