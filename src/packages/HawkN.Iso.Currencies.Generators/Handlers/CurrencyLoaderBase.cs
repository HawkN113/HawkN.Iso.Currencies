using HawkN.Iso.Currencies.Generators.Models;
namespace HawkN.Iso.Currencies.Generators.Handlers;

internal abstract class CurrencyLoaderBase
{
    protected static readonly HashSet<string> PreciousMetalsCodes = new(StringComparer.OrdinalIgnoreCase)
        { "XAG", "XAU", "XPD", "XPT" };

    protected static readonly HashSet<string> SpecialReserveCodes = new(StringComparer.OrdinalIgnoreCase)
        { "XXX", "XTS", "XDR" };

    protected static readonly HashSet<string> SpecialUnits = new(StringComparer.OrdinalIgnoreCase)
        { "XBA", "XBB", "XBC", "XBD", "XSU", "XUA" };

    protected static readonly HashSet<string> ExcludedCodes = new(StringComparer.OrdinalIgnoreCase)
        { "VED", "XAD", "XCG", "ZWG", "CNH" };

    public virtual CurrencyData ActualCurrencyData { get; } = null!;
}