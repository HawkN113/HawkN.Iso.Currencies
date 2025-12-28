using HawkN.Iso.Currencies.Models;
namespace HawkN.Iso.Currencies.Builders.Abstractions;

/// <summary>
/// Interface for filtering a currency query.
/// Allows selecting currency types and including or excluding specific codes, names, or numeric codes.
/// </summary>
public interface ICurrencyQueryFilter : ICurrencyQueryFinal
{
    /// <summary>
    /// Adds a currency type to the filter.
    /// </summary>
    /// <param name="type">The <see cref="CurrencyType"/> to include.</param>
    /// <returns>The current <see cref="ICurrencyQueryFilter"/> instance.</returns>
    ICurrencyQueryFilter Type(CurrencyType type);

    /// <summary>
    /// Includes specific currencies by configuring an include filter.
    /// </summary>
    /// <param name="configure">Action to configure included codes, names, or numeric codes.</param>
    /// <returns>The current <see cref="ICurrencyQueryNoWhereFilter"/> instance.</returns>
    ICurrencyQueryNoWhereFilter With(Action<IIncludeFilterBuilder> configure);

    /// <summary>
    /// Excludes specific currencies by configuring an exclude filter.
    /// </summary>
    /// <param name="configure">Action to configure excluded codes, names, or numeric codes.</param>
    /// <returns>The current <see cref="ICurrencyQueryNoWhereFilter"/> instance.</returns>
    ICurrencyQueryNoWhereFilter Without(Action<IExcludeFilterBuilder> configure);

    /// <summary>
    /// Adds a custom predicate filter to the query (after Type).
    /// </summary>
    ICurrencyQueryFinal Where(Func<Currency, bool> predicate);
}