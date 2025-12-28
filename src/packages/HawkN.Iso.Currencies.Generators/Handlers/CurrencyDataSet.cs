namespace HawkN.Iso.Currencies.Generators.Handlers;

internal sealed class CurrencyDataSet
{
    public string PublishedDate { get; set; } = string.Empty;
    public List<CurrencyRaw> Currencies { get; set; } = [];
}