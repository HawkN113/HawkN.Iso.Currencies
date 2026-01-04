namespace HawkN.Iso.Currencies.Builders.Abstractions;

/// <summary>
/// Interface for filtering a currency query without allowing .Where().
/// Used after .With() or .Without().
/// </summary>
public interface ICurrencyQueryNoWhereFilter : ICurrencyQueryFinal
{
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
}