namespace HawkN.Iso.Currencies.Generators.Models;

internal sealed class CurrencyRow
{
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string? Country { get; set; }
    public string? NumericCode { get; set; }
    public CurrencyType? CurrencyType { get; set; }
    public bool IsHistoric { get; set; }
    public string? WithdrawalDate { get; set; }
}