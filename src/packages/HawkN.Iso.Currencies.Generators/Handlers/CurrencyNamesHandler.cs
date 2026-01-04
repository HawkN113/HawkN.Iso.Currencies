using System.Xml.Linq;
namespace HawkN.Iso.Currencies.Generators.Handlers;

internal sealed class CurrencyNamesHandler(string xmlContent)
{
    private readonly XDocument _doc = XDocument.Parse(xmlContent);

    public Dictionary<string, string> LoadNames()
    {
        return _doc
            .Descendants("currency")
            .Where(c => c.Attribute("type") != null)
            .Select(c => new
            {
                Code = c.Attribute("type")!.Value,
                Name = c.Element("displayName")?.Value
            })
            .Where(x => !string.IsNullOrEmpty(x.Name))
            .GroupBy(x => x.Code)
            .ToDictionary(
                g => g.Key,
                g => g.First().Name!,
                StringComparer.OrdinalIgnoreCase);
    }
}