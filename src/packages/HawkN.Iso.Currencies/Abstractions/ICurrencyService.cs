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
    Currency? Get(string value);

    /// <summary>
    /// Gets current (active) currency information by enum code.
    /// </summary>
    Currency? Get(CurrencyCode code);

    /// <summary>
    /// Starts building a query for actual currencies with fluent filtering and sorting.
    /// </summary>
    ICurrencyQueryStart Query();
}