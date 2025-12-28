using HawkN.Iso.Currencies.Builders.Abstractions;
using HawkN.Iso.Currencies.Models;
namespace HawkN.Iso.Currencies.Abstractions;

/// <summary>
/// Service for working with currencies: validation, retrieval, and querying.
/// Includes support for historical (withdrawn) currencies.
/// </summary>
public interface ICurrencyService
{
    /// <summary>
    /// Tries to validate the specified currency by string value.
    /// </summary>
    bool TryValidate(string value, out ValidationResult result);

    /// <summary>
    /// Tries to validate the specified currency by enum code.
    /// </summary>
    bool TryValidate(CurrencyCode code, out ValidationResult result);

    /// <summary>
    /// Checks if a currency exists by string value.
    /// </summary>
    bool Exists(string value);

    /// <summary>
    /// Checks if a currency exists by enum code.
    /// </summary>
    bool Exists(CurrencyCode code);

    /// <summary>
    /// Gets current (active) currency information by code or name.
    /// </summary>
    Models.Currency? Get(string value);

    /// <summary>
    /// Gets current (active) currency information by enum code.
    /// </summary>
    Models.Currency? Get(CurrencyCode code);

    /// <summary>
    /// Gets historical (withdrawn) currency information by string value.
    /// </summary>
    /// <param name="value">Currency code or name.</param>
    /// <returns>Currency object or null if not found.</returns>
    Models.Currency? GetHistorical(string value);

    /// <summary>
    /// Retrieves all historical (withdrawn) currencies defined by ISO 4217.
    /// </summary>
    /// <returns>An array of <see cref="Currency"/> objects representing withdrawn currencies.</returns>
    Models.Currency[] GetAllHistorical();

    /// <summary>
    /// Starts building a query for actual currencies with fluent filtering and sorting.
    /// </summary>
    ICurrencyQueryStart Query();
}