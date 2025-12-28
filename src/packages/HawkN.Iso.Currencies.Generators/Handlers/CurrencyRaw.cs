namespace HawkN.Iso.Currencies.Generators.Handlers;

internal sealed class CurrencyRaw
{
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
    public string? NumericCode { get; set; }
    public string? WithdrawalDate { get; set; }
}