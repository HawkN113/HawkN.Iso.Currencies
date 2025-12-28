namespace HawkN.Iso.Currencies.Models;

/// <summary>
/// Represents an ISO 4217 currency, including code, name, and metadata.
/// </summary>
/// <param name="Code">Alphabetic ISO 4217 currency code (e.g., "USD").</param>
/// <param name="Name">The full name of the currency (e.g., "US Dollar").</param>
/// <param name="CountryName">The country or region that issues the currency.</param>
/// <param name="NumericCode">Numeric ISO 4217 code (e.g., "840").</param>
/// <param name="IsHistoric">True if the currency is no longer in use.</param>
/// <param name="WithdrawalDate">The date when the currency was withdrawn, if applicable.</param>
/// <param name="CurrencyType">The classification of the currency (Fiat, PreciousMetal, etc.).</param>
public sealed record Currency(
    string Code,
    string Name,
    string? CountryName,
    string? NumericCode,
    bool IsHistoric,
    DateOnly? WithdrawalDate,
    CurrencyType? CurrencyType);