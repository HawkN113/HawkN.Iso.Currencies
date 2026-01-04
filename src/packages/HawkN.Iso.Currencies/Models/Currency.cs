namespace HawkN.Iso.Currencies.Models;

/// <summary>
/// Represents an ISO 4217 currency, including code, name, and metadata.
/// </summary>
/// <param name="Code">Alphabetic ISO 4217 currency code (e.g., "USD").</param>
/// <param name="Name">The full name of the currency (e.g., "US Dollar").</param>
/// <param name="NumericCode">Numeric ISO 4217 code (e.g., 840).</param>
/// <param name="CurrencyType">The classification of the currency (Fiat, PreciousMetal, etc.).</param>
public sealed record Currency(
    string Code,
    string Name,
    int NumericCode,
    CurrencyType? CurrencyType)
{
    /// <summary>
    /// Numeric code as string with leading zeros (3 digits), e.g., "840".
    /// </summary>
    public string NumericCodeString => NumericCode.ToString("D3");
}