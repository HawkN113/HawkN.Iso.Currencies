namespace HawkN.Iso.Currencies.Generators.Models;

internal sealed class CurrencyData
{
    public string PublishedDate { get; set; } = string.Empty;
    public List<Currency> Currencies { get; set; } = [];
}