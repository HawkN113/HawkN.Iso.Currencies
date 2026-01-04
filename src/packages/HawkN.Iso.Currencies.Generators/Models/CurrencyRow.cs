namespace HawkN.Iso.Currencies.Generators.Models;

internal sealed class CurrencyRow
{
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string NumericCode { get; set; } = string.Empty;
    public CurrencyType? CurrencyType { get; set; }
}