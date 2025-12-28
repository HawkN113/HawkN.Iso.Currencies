namespace HawkN.Iso.Currencies.Models;

/// <summary>
/// Represents the classification of a currency code according to ISO 4217.
/// </summary>
public enum CurrencyType
{
    /// <summary>
    /// Standard fiat currency issued by a sovereign state or monetary union.
    /// </summary>
    Fiat,

    /// <summary>
    /// Precious metal currencies, expressed in troy ounces (e.g., XAU for gold, XAG for silver).
    /// </summary>
    PreciousMetal,

    /// <summary>
    /// Special reserved codes not assigned to any specific currency (e.g., XXX for no currency, XTS for testing).
    /// </summary>
    SpecialReserve,

    /// <summary>
    /// Special units of account defined by international organizations (e.g., XDR, XSU, XUA).
    /// </summary>
    SpecialUnit
}