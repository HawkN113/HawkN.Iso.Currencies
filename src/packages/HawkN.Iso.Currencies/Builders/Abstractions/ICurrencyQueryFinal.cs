using HawkN.Iso.Currencies.Models;
namespace HawkN.Iso.Currencies.Builders.Abstractions;

/// <summary>
/// Final interface for building a currency query.
/// Use <see cref="Build"/> to get the resulting collection of currencies.
/// </summary>
public interface ICurrencyQueryFinal
{
    /// <summary>
    /// Builds and returns a collection of currencies matching the configured filters.
    /// </summary>
    /// <returns>A read-only collection of <see cref="Currency"/> objects.</returns>
    IReadOnlyList<Currency> Build();
}